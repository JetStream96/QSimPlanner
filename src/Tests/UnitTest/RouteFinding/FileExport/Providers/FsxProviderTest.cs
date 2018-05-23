using FakeItEasy;
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
                new Waypoint("N10.2W20.0", 10.2, -20.0), "DCT", -1.0,
                new Waypoint("EFGH18", 0.0, 3.0));

            var text = FsxProvider.GetExportText(new ExportInput()
            {
                Route = route,
                Airports = manager
            });

            // Assert flight plan.
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
                IsDouble(main.Element("CruisingAlt").Value) &&
                main.Element("DepartureID").Value == "ABCD" &&
                main.Element("DepartureLLA").Value == origLatLonAlt &&
                main.Element("DestinationID").Value == "EFGH" &&
                main.Element("DestinationLLA").Value == destLatLonAlt &&
                main.Element("DepartureName").Value == abcd.Name &&
                main.Element("DestinationName").Value == efgh.Name);

            var ver = main.Element("AppVersion");

            Assert.AreEqual(ver.Element("AppVersionMajor").Value, "10");
            Assert.AreEqual(ver.Element("AppVersionBuild").Value, "61637");

            var wpts = main.Elements("ATCWaypoint").ToList();
            Assert.AreEqual(4, wpts.Count);

            // The waypoint nodes.
            Assert.AreEqual(wpts[0].Attribute("id").Value, "ABCD");
            Assert.AreEqual(wpts[0].Element("ATCWaypointType").Value, "Airport");
            Assert.AreEqual(wpts[0].Element("WorldPosition").Value, origLatLonAlt);
            Assert.AreEqual(GetIdent(wpts[0]), "ABCD");

            Assert.AreEqual(wpts[1].Attribute("id").Value, "WPT");
            Assert.AreEqual(wpts[1].Element("ATCWaypointType").Value, "Intersection");
            Assert.AreEqual(wpts[1].Element("WorldPosition").Value, "N0° 0' 0.00\",E1° 0' 0.00\",+000000.00");
            Assert.AreEqual(GetIdent(wpts[1]), "WPT");

            // In this case the id attribute is different from ident.
            Assert.AreEqual(wpts[2].Attribute("id").Value, "1020N");
            Assert.AreEqual(wpts[2].Element("ATCWaypointType").Value, "Intersection");
            Assert.AreEqual(wpts[2].Element("WorldPosition").Value, "N10° 12' 0.00\",W20° 0' 0.00\",+000000.00");
            Assert.AreEqual(GetIdent(wpts[2]), "1012N2000W");

            Assert.AreEqual(wpts[3].Attribute("id").Value, "EFGH");
            Assert.AreEqual(wpts[3].Element("ATCWaypointType").Value, "Airport");
            Assert.AreEqual(wpts[3].Element("WorldPosition").Value, destLatLonAlt);
            Assert.AreEqual(GetIdent(wpts[3]), "EFGH");
        }

        private static IAirport GetAirport(string icao, string name,
            double lat, double lon, int elevation)
        {
            var a = A.Fake<IAirport>();
            A.CallTo(() => a.Icao).Returns(icao);
            A.CallTo(() => a.Name).Returns(name);
            A.CallTo(() => a.Lat).Returns(lat);
            A.CallTo(() => a.Lon).Returns(lon);
            A.CallTo(() => a.Elevation).Returns(elevation);
            return a;
        }

        private static string GetIdent(XElement node)
        {
            return node.Element("ICAO").Element("ICAOIdent").Value;
        }

        private static bool IsDouble(string s) => double.TryParse(s, out var d);

        [Test]
        public void LatLonAltTest()
        {
            Assert.AreEqual("N25° 4' 23.28\",E121° 12' 58.26\",+000106.99",
                LatLonAlt(new LatLon(25.073133333, 121.216183333), 106.99));
        }
    }
}
