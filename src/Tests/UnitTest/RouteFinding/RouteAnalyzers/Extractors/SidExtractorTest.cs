using NUnit.Framework;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System.Collections.Generic;
using System.Linq;
using static QSP.MathTools.GCDis;

namespace UnitTest.RouteFinding.RouteAnalyzers.Extractors
{
    [TestFixture]
    public class SidExtractorTest
    {

        [Test]
        public void Case1Test()
        {
            // Setup
            var route = new string[] { "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var extractor = new SidExtractor(
                route,
                "RCTP",
                "05L",
                rwy,
                wptList,
                new SidCollection(new SidEntry[0]));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.IsTrue(Enumerable.SequenceEqual(route,
                result.RemainingRoute));

            Assert.AreEqual(2, result.Sid.Count);

            var node = result.Sid.First;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(node.Value.AirwayToNext == "SID1");
            Assert.AreEqual(
                node.Value.DistanceToNext,
                Distance(20.0, 120.0, 22.0, 122.0),
                1E-8);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpt1));
            Assert.IsTrue(node == result.Sid.Last);
        }

        [Test]
        public void WhenSidExistsShouldRemoveAndAddToOrigRoute()
        {
            // Setup
            var route = new string[] { "SID1", "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var extractor = new SidExtractor(
                route,
                "RCTP",
                "05L",
                rwy,
                wptList,
                new SidCollection(new List<SidEntry>() {
                    new SidEntry(
                        "05L",
                        "SID1",
                        new Waypoint[]{ wpt1 },
                        EntryType.RwySpecific,
                        false) }));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.AreEqual(3, result.RemainingRoute.Count());
            Assert.IsTrue(result.RemainingRoute.First() == "HLG");

            Assert.AreEqual(2, result.Sid.Count);

            var node = result.Sid.First;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(node.Value.AirwayToNext == "SID1");
            Assert.AreEqual(
                node.Value.DistanceToNext,
                Distance(20.0, 120.0, 22.0, 122.0),
                1E-8);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpt1));
            Assert.IsTrue(node == result.Sid.Last);
        }

        [Test]
        public void WhenSidLastWptNotInWptListShouldRemoveFromRoute()
        {
            // Setup
            var route = new string[] { "SID1", "P1", "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var extractor = new SidExtractor(
                route,
                "RCTP",
                "05L",
                rwy,
                wptList,
                new SidCollection(new List<SidEntry>() {
                    new SidEntry(
                        "05L",
                        "SID1",
                        new Waypoint[] {new Waypoint("P1", 21.0, 121.0) },
                        EntryType.RwySpecific,
                        false) }));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.AreEqual(3, result.RemainingRoute.Count());
            Assert.IsTrue(result.RemainingRoute.First() == "HLG");

            Assert.AreEqual(2, result.Sid.Count);

            var node = result.Sid.First;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(node.Value.AirwayToNext == "SID1");
            Assert.AreEqual(
                node.Value.DistanceToNext,
                Distance(20.0, 120.0, 21.0, 121.0),
                1E-8);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(
                new Waypoint("P1", 21.0, 121.0)));
            Assert.IsTrue(node == result.Sid.Last);
        }
    }
}
