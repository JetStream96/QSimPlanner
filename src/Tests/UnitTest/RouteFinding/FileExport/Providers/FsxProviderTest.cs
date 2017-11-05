using Moq;
using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.FileExport.Providers;
using System.Linq;
using System.Xml.Linq;
using static QSP.RouteFinding.FileExport.Providers.FsxProvider;

namespace UnitTest.RouteFinding.FileExport.Providers
{
    [TestFixture]
    public class FsxProviderTest
    {
        [Test]
        public void GetExportTextTest()
        {
            var abcd = GetAirport("ABCD", "Name A", 5, 10, 500);
            var efgh = GetAirport("EFGH", "Name B", 50, 15, 1500);
            var manager = Common.GetAirportManager(abcd, efgh);

            var route = Common.GetRoute(
                new Waypoint("ABCD02", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT", 0.0, 1.0), "B", -1.0,
                new Waypoint("EFGH18", 0.0, 3.0));

            var provider = new FsxProvider(route, manager);
            var text = provider.GetExportText();

            // AssertFlightPlan
            var doc = XDocument.Parse(text);
            var root = doc.Root;

            Assert.IsTrue(
                root.Name == "SimBase.Document" &&
                root.Attribute("Type").Value == "AceXML" &&
                root.Attribute("version").Value == "1,0" &&
                root.Element("Descr").Value == "AceXML Document");

            var main = root.Element("FlightPlan.FlightPlan");
            var orig = route.First.Value.Waypoint;
            var dest = route.Last.Value.Waypoint;
            var origLatLonAlt = LatLonAlt(orig, abcd.Elevation);
            var destLatLonAlt = LatLonAlt(dest, efgh.Elevation);

            Assert.IsTrue(
                main.Elements("Title").Any() &&
                main.Element("FPType").Value == "IFR" &&
                CanConvertToDouble(main.Element("CruisingAlt").Value) &&
                main.Element("DepartureID").Value == abcd.Icao &&
                main.Element("DepartureLLA").Value == origLatLonAlt &&
                main.Element("DestinationID").Value == efgh.Icao &&
                main.Element("DestinationLLA").Value == destLatLonAlt &&
                main.Element("DepartureName").Value == abcd.Name &&
                main.Element("DestinationName").Value == efgh.Name);

            var ver = main.Element("AppVersion");

            Assert.IsTrue(
                ver.Element("AppVersionMajor").Value == "10" &&
                ver.Element("AppVersionBuild").Value == "61637");

            var wpts = main.Elements("ATCWaypoint").ToList();
            Assert.IsTrue(wpts.Count >= 2);
            Assert.IsTrue(wpts.All(w =>
            w.Attribute("id").Value == GetIdent(w)));

            Assert.IsTrue(
                wpts[0].Element("ATCWaypointType").Value == "Airport" &&
                wpts[0].Element("WorldPosition").Value == origLatLonAlt &&
                GetIdent(wpts[0]) == abcd.Icao);

            var wpt = route.First.Next.Value.Waypoint;

            Assert.IsTrue(
                wpts[1].Element("ATCWaypointType").Value == "Intersection" &&
                wpts[1].Element("WorldPosition").Value == LatLonAlt(wpt, 0.0) &&
                GetIdent(wpts[1]) == wpt.ID);

            Assert.IsTrue(
                wpts[2].Element("ATCWaypointType").Value == "Airport" &&
                wpts[2].Element("WorldPosition").Value == destLatLonAlt &&
                GetIdent(wpts[2]) == efgh.Icao);
        }

        private static IAirport GetAirport(string icao, string name,
            double lat, double lon, int elevation)
        {
            var a = new Mock<IAirport>();
            a.Setup(i => i.Icao).Returns(icao);
            a.Setup(i => i.Name).Returns(name);
            a.Setup(i => i.Lat).Returns(lat);
            a.Setup(i => i.Lon).Returns(lon);
            a.Setup(i => i.Elevation).Returns(elevation);
            return a.Object;
        }

        private static string GetIdent(XElement node)
        {
            return node.Element("ICAO").Element("ICAOIdent").Value;
        }

        private static bool CanConvertToDouble(string s)
        {
            return double.TryParse(s, out var d);
        }

        [Test]
        public void LatLonAltTest()
        {
            Assert.IsTrue("N25° 4' 23.28\",E121° 12' 58.26\",+000106.99"
                == LatLonAlt(new LatLon(25.073133333, 121.216183333), 106.99));
        }
    }
}
