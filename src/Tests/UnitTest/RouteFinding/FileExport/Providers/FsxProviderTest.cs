using NUnit.Framework;
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
            var abcd = Common.GetAirport("ABCD");
            var efgh = Common.GetAirport("EFGH");
            var manager = Common.GetAirportManager(abcd, efgh);

            var route = Common.GetRoute(
                new Waypoint("ABCD02", 0.0, 0.0), "A", -1.0,
                new Waypoint("WPT1", 0.0, 1.0), "B", -1.0,
                new Waypoint("WPT2", 0.0, 2.0), "C", -1.0,
                new Waypoint("EFGH18", 0.0, 3.0));

            var provider = new PmdgProvider(route, manager);
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
            var orig = route.First.Value;
            var dest = route.Last.Value;
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

            Assert.IsTrue(
                wpts[0].Attribute("id").Value == abcd.Icao &&
                wpts[0].Element("ATCWaypointType").Value == "Airport" &&
                wpts[0].Element("WorldPosition").Value == origLatLonAlt &&
                wpts[0].Element("ICAO").Element("ICAOIdent").Value == abcd.Icao);

            var wpt1 = route.First.Next.Value.Waypoint;

            Assert.IsTrue(
                wpts[1].Element("ATCWaypointType").Value == "Intersection" &&
                wpts[1].Element("WorldPosition").Value == LatLonAlt(wpt1, 0.0) &&
                wpts[1].Element("ICAO").Element("ICAOIdent").Value == wpt1.ID);

            var wpt2 = route.Last.Previous.Value.Waypoint;

            Assert.IsTrue(
                wpts[0].Element("ATCWaypointType").Value == "Intersection" &&
                wpts[0].Element("WorldPosition").Value == LatLonAlt(wpt2, 0.0) &&
                wpts[0].Element("ICAO").Element("ICAOIdent").Value == wpt2.ID);

            Assert.IsTrue(
                wpts[3].Attribute("id").Value == efgh.Icao &&
                wpts[3].Element("ATCWaypointType").Value == "Airport" &&
                wpts[3].Element("WorldPosition").Value == destLatLonAlt &&
                wpts[3].Element("ICAO").Element("ICAOIdent").Value == efgh.Icao);
        }

        private static bool CanConvertToDouble(string s)
        {
            double d;
            return double.TryParse(s, out d);
        }

    }
}
