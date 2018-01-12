using NUnit.Framework;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System.Linq;
using static CommonLibrary.LibraryExtension.Types;

namespace UnitTest.RouteFinding.RouteAnalyzers.Extractors
{
    [TestFixture]
    public class SidExtractorTest
    {
        [Test]
        public void Case1Test()
        {
            // Setup
            var route = new[] { "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var extractor = new SidExtractor(
                route.ToRouteString(),
                "05L",
                rwy,
                wptList,
                new SidCollection(new SidEntry[0]));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.IsTrue(route.SequenceEqual(result.RemainingRoute));

            var origRoute = result.OrigRoute;
            Assert.AreEqual(2, origRoute.Count);

            var node = origRoute.First;
            var neighbor = node.Value.Neighbor;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(neighbor.Airway == "DCT");
            Assert.AreEqual(neighbor.Distance, rwy.Distance(wpt1), 1E-8);
            Assert.AreEqual(0, neighbor.InnerWaypoints.Count);
            Assert.AreEqual(InnerWaypointsType.None, neighbor.Type);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpt1));
        }

        [Test]
        public void Case2Test()
        {
            // Setup
            var route = new[] { "SID1", "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var p1 = new Waypoint("P1", 21.0, 121.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var sid = new SidEntry(
                "05L",
                "SID1",
                new[] { p1 },
                EntryType.RwySpecific,
                true);

            var extractor = new SidExtractor(
                route.ToRouteString(),
                "05L",
                rwy,
                wptList,
                new SidCollection(new[] { sid }));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.IsTrue(result.RemainingRoute.SequenceEqual(new[] { "HLG", "A1", "MKG" }));

            var origRoute = result.OrigRoute;
            Assert.AreEqual(2, origRoute.Count);

            var node = origRoute.First;
            var neighbor = node.Value.Neighbor;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(neighbor.Airway == sid.Name);
            Assert.AreEqual(neighbor.Distance, List(rwy, p1, wpt1).TotalDistance(), 1E-8);
            Assert.IsTrue(neighbor.InnerWaypoints.SequenceEqual(new[] { p1 }));
            Assert.AreEqual(InnerWaypointsType.Terminal, neighbor.Type);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpt1));
        }

        [Test]
        public void Case3Test()
        {
            // Setup
            var route = new[] { "SID1", "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var p1 = new Waypoint("P1", 21.0, 121.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var sid = new SidEntry(
                "05L",
                "SID1",
                new[] { p1, wpt1 },
                EntryType.RwySpecific,
                false);

            var extractor = new SidExtractor(
                route.ToRouteString(),
                "05L",
                rwy,
                wptList,
                new SidCollection(new[] { sid }));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.IsTrue(result.RemainingRoute.SequenceEqual(new[] { "HLG", "A1", "MKG" }));

            var origRoute = result.OrigRoute;
            Assert.AreEqual(2, origRoute.Count);

            var node = origRoute.First;
            var neighbor = node.Value.Neighbor;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(neighbor.Airway == sid.Name);
            Assert.AreEqual(neighbor.Distance, List(rwy, p1, wpt1).TotalDistance(), 1E-8);
            Assert.IsTrue(neighbor.InnerWaypoints.SequenceEqual(new[] { p1 }));
            Assert.AreEqual(InnerWaypointsType.Terminal, neighbor.Type);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpt1));
        }

        [Test]
        public void Case4Test()
        {
            // Setup
            var route = new[] { "SID1", "P1", "HLG", "A1", "MKG" };

            var wptList = new WaypointList();
            var rwy = new Waypoint("RCTP05L", 20.0, 120.0);
            var wpt1 = new Waypoint("HLG", 22.0, 122.0);
            wptList.AddWaypoint(wpt1);

            var p1 = new Waypoint("P1", 21.0, 121.0);

            var sid = new SidEntry(
                "05L",
                "SID1",
                new[] { p1 },
                EntryType.RwySpecific,
                false);

            var extractor = new SidExtractor(
                route.ToRouteString(),
                "05L",
                rwy,
                wptList,
                new SidCollection(new[] { sid }));

            // Invoke
            var result = extractor.Extract();

            // Assert
            Assert.IsTrue(result.RemainingRoute.SequenceEqual(new[] { "HLG", "A1", "MKG" }));

            var origRoute = result.OrigRoute;
            Assert.AreEqual(3, origRoute.Count);

            var node = origRoute.First;
            var neighbor1 = node.Value.Neighbor;
            Assert.IsTrue(node.Value.Waypoint.Equals(rwy));
            Assert.IsTrue(neighbor1.Airway == sid.Name);
            Assert.AreEqual(neighbor1.Distance, rwy.Distance(p1), 1E-8);
            Assert.AreEqual(0, neighbor1.InnerWaypoints.Count);
            Assert.AreEqual(InnerWaypointsType.Terminal, neighbor1.Type);

            node = node.Next;
            var neighbor2 = node.Value.Neighbor;
            Assert.IsTrue(node.Value.Waypoint.Equals(p1));
            Assert.IsTrue(neighbor2.Airway == "DCT");
            Assert.AreEqual(neighbor2.Distance, p1.Distance(wpt1), 1E-8);
            Assert.AreEqual(0, neighbor2.InnerWaypoints.Count);
            Assert.AreEqual(InnerWaypointsType.None, neighbor2.Type);

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpt1));
        }
    }
}
