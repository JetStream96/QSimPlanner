using NUnit.Framework;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using static UnitTest.RouteFinding.RouteAnalyzers.Common;

namespace UnitTest.RouteFinding.RouteAnalyzers.Extractors
{
    [TestFixture]
    public class StarExtractorTest
    {
        [Test]
        public void WhenInputIsEmptyLinkedListShouldReturnRwy()
        {
            var route = new LinkedList<string>();
            var rwyWpt = new Waypoint("VHHH07L", 20.0, 120.0);
            var extractor = new StarExtractor(
                route, "", "", rwyWpt, null, null);

            var destRoute = extractor.Extract();

            Assert.AreEqual(1, destRoute.Count);
            Assert.IsTrue(destRoute.FirstWaypoint.Equals(rwyWpt));
        }

        [Test]
        public void WhenExistsShouldRemoveIcao()
        {
            var route = new LinkedList<string>(
                new string[] { "ELATO", "VHHH" });

            var extractor = new StarExtractor(
                route,
                "VHHH",
                "",
                new Waypoint("VHHH07L", 18.0, 118.0),
                null,
                null);

            var destRoute = extractor.Extract();

            Assert.AreEqual(1, route.Count);
            Assert.IsFalse(destRoute.LastWaypoint.ID == "VHHH");
        }

        [Test]
        public void WhenStarExistsShouldRemoveAndAddToDestRoute()
        {
            // Setup
            var route = new LinkedList<string>(
                new string[] { "ELATO", "SIERA", "STAR1" });

            var wptList = new WaypointList();
            var wpt = new Waypoint("SIERA", 18.0, 115.0);
            var rwy = new Waypoint("VHHH07L", 18.0, 118.0);
            wptList.AddWaypoint(wpt);

            var stars = new StarCollection(
                new List<StarEntry>() {
                    new StarEntry(
                        "07L",
                        "STAR1",
                        new List<Waypoint>(){ wpt },
                        EntryType.RwySpecific) });

            var extractor = new StarExtractor(
                route,
                "VHHH",
                "07L",
                rwy,
                wptList,
                stars);

            // Invoke
            var destRoute = extractor.Extract();

            // Assert            
            Assert.AreEqual(2, route.Count);
            Assert.IsTrue(route.Last.Value == "SIERA");

            var expected = GetRoute(
                wpt, "STAR1", -1.0,
                rwy);

            Assert.IsTrue(destRoute.Equals(expected));
        }

        [Test]
        public void WhenStarLastWptNotInWptListShouldRemoveFromRoute()
        {
            // Setup
            var route = new LinkedList<string>(
                new string[] { "ELATO", "SIERA", "P1", "STAR1" });
            var wptList = new WaypointList();

            var wpt1 = new Waypoint("SIERA", 18.0, 115.0);
            var p1 = new Waypoint("P1", 19.0, 119.0);
            var rwy = new Waypoint("VHHH07L", 18.0, 118.0);

            wptList.AddWaypoint(wpt1);

            var sids =
                new StarCollection(new List<StarEntry>() {
                    new StarEntry(
                        "07L",
                        "STAR1",
                        new List<Waypoint>(){ p1 },
                        EntryType.RwySpecific) });

            var extractor = new StarExtractor(
                route,
                "VHHH",
                "07L",
                rwy,
                wptList,
                sids);

            // Invoke
            var destRoute = extractor.Extract();

            // Assert
            Assert.AreEqual(2, route.Count);
            Assert.IsTrue(route.Last.Value == "SIERA");

            var expected = GetRoute(
                p1, "STAR1", -1.0,
                rwy);

            Assert.IsTrue(destRoute.Equals(expected));
        }
    }
}
