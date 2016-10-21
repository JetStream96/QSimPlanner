using NUnit.Framework;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Linq;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFinding.RouteAnalyzers.Extractors
{
    [TestFixture]
    public class StarExtractorTest
    {
        [Test]
        public void Case1Test()
        {
            var rwy = new Waypoint("VHHH07L", 18.0, 118.0);
            var wpt = new Waypoint("SIERA", 18.0, 115.0);
            var wptList = new WaypointList();
            wptList.AddWaypoint(wpt);

            var route = new string[] { "SIERA" };

            var extractor = new StarExtractor(
                route.ToRouteString(),
                "VHHH",
                "",
                rwy,
                wptList,
                new StarCollection(new StarEntry[0]));

            var result = extractor.Extract();

            Assert.IsTrue(Enumerable.SequenceEqual(
                route, result.RemainingRoute));

            var destRoute = result.DestRoute;
            Assert.AreEqual(2, destRoute.Count);

            var node1 = destRoute.First();
            var neighbor = node1.Neighbor;
            Assert.IsTrue(node1.Waypoint.Equals(wpt));
            Assert.IsTrue("DCT" == neighbor.Airway);
            Assert.AreEqual(wpt.Distance(rwy), neighbor.Distance, 1E-8);
            Assert.AreEqual(0, neighbor.InnerWaypoints.Count);

            Assert.IsTrue(destRoute.Last().Waypoint.Equals(rwy));
        }

        [Test]
        public void Case2Test()
        {
            // Setup
            var route = new string[] { "ELATO", "SIERA", "STAR1" };

            var wptList = new WaypointList();
            var wpt = new Waypoint("SIERA", 18.0, 115.0);
            wptList.AddWaypoint(wpt);

            var rwy = new Waypoint("VHHH07L", 18.0, 118.0);
            var p1 = new Waypoint("P1", 18.5, 117.0);

            var star = new StarEntry(
                "07L",
                "STAR1",
                new Waypoint[] { wpt, p1 },
                EntryType.RwySpecific);

            var stars = new StarCollection(new StarEntry[] { star });

            var extractor = new StarExtractor(
                route.ToRouteString(),
                "VHHH",
                "07L",
                rwy,
                wptList,
                stars);

            var result = extractor.Extract();

            Assert.IsTrue(Enumerable.SequenceEqual(result.RemainingRoute,
                new string[] { "ELATO", "SIERA" }));

            var destRoute = result.DestRoute;
            Assert.AreEqual(2, destRoute.Count);

            var node1 = destRoute.First();
            var neighbor = node1.Neighbor;
            Assert.IsTrue(node1.Waypoint.Equals(wpt));
            Assert.IsTrue("STAR1" == neighbor.Airway);

            double distance = new Waypoint[] { wpt, p1, rwy }.TotalDistance();
            Assert.AreEqual(distance, neighbor.Distance, 1E-8);
            Assert.IsTrue(Enumerable.SequenceEqual(neighbor.InnerWaypoints,
                new Waypoint[] { p1 }));

            Assert.IsTrue(destRoute.Last().Waypoint.Equals(rwy));
        }

        [Test]
        public void Case3Test()
        {
            // Setup
            var route = new string[] { "SIERA", "P1", "STAR1" };

            var wptList = new WaypointList();
            var wpt = new Waypoint("SIERA", 18.0, 115.0);
            wptList.AddWaypoint(wpt);

            var rwy = new Waypoint("VHHH07L", 18.0, 118.0);
            var p1 = new Waypoint("P1", 18.5, 117.0);

            var star = new StarEntry(
                "07L",
                "STAR1",
                new Waypoint[] { p1 },
                EntryType.RwySpecific);

            var stars = new StarCollection(new StarEntry[] { star });

            var extractor = new StarExtractor(
                route.ToRouteString(),
                "VHHH",
                "07L",
                rwy,
                wptList,
                stars);

            var result = extractor.Extract();

            Assert.IsTrue(Enumerable.SequenceEqual(result.RemainingRoute,
                new string[] { "SIERA" }));

            var destRoute = result.DestRoute;
            Assert.AreEqual(3, destRoute.Count);

            var node1 = destRoute.First();
            var neighbor1 = node1.Neighbor;
            Assert.IsTrue(node1.Waypoint.Equals(wpt));
            Assert.IsTrue("DCT" == neighbor1.Airway);
            Assert.AreEqual(wpt.Distance(p1), neighbor1.Distance, 1E-8);
            Assert.AreEqual(0, neighbor1.InnerWaypoints.Count);

            var node2 = destRoute.FirstNode.Next.Value;
            var neighbor2 = node2.Neighbor;
            Assert.IsTrue(node2.Waypoint.Equals(p1));
            Assert.IsTrue("STAR1" == neighbor2.Airway);
            Assert.AreEqual(p1.Distance(rwy), neighbor2.Distance, 1E-8);
            Assert.AreEqual(0, neighbor2.InnerWaypoints.Count);

            Assert.IsTrue(destRoute.Last().Waypoint.Equals(rwy));
        }
    }
}
