using NUnit.Framework;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using static UnitTest.Common.Utilities;
using static QSP.MathTools.GCDis;

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
            var extractor = new StarExtractor(route, "", "", rwyWpt, null, null);

            var destRoute = extractor.Extract();

            Assert.AreEqual(1, destRoute.Count);
            Assert.IsTrue(destRoute.FirstWaypoint.Equals(rwyWpt));
        }

        [Test]
        public void WhenExistsShouldRemoveIcao()
        {
            var route = new LinkedList<string>(new string[] { "ELATO", "VHHH" });
            var extractor = new StarExtractor(route, "VHHH", "", new Waypoint("VHHH07L", 18.0, 118.0), null, null);

            var destRoute = extractor.Extract();

            Assert.AreEqual(1, route.Count);
            Assert.IsFalse(destRoute.LastWaypoint.ID == "VHHH");
        }

        [Test]
        public void WhenStarExistsShouldRemoveAndAddToDestRoute()
        {
            // Setup
            var route = new LinkedList<string>(new string[] { "ELATO", "SIERA", "STAR1" });
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("SIERA", 18.0, 115.0));

            var extractor = new StarExtractor(route,
                                             "VHHH",
                                             "07L",
                                             new Waypoint("VHHH07L", 18.0, 118.0),
                                             wptList,
                                             new StarCollection(new List<StarEntry>() {
                                                 new StarEntry("07L",
                                                              "STAR1",
                                                              new List<Waypoint>(){new Waypoint("SIERA", 18.0, 115.0) },
                                                              EntryType.RwySpecific) }));
            // Invoke
            var destRoute = extractor.Extract();

            // Assert
            Assert.AreEqual(2, route.Count);
            Assert.IsTrue(route.Last.Value == "SIERA");

            Assert.AreEqual(2, destRoute.Count);

            var node = destRoute.First;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("SIERA", 18.0, 115.0)) &&
                          node.Value.AirwayToNext == "STAR1" &&
                          WithinPrecision(node.Value.DistanceToNext, Distance(18.0, 115.0, 18.0, 118.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 18.0, 118.0)) &&
                          node == destRoute.Last);

        }

        [Test]
        public void WhenStarLastWptNotInWptListShouldRemoveFromRoute()
        {
            // Setup
            var route = new LinkedList<string>(new string[] { "ELATO", "SIERA", "P1", "STAR1" });
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("SIERA", 18.0, 115.0));

            var extractor = new StarExtractor(route,
                                              "VHHH",
                                              "07L",
                                              new Waypoint("VHHH07L", 18.0, 118.0),
                                              wptList,
                                              new StarCollection(new List<StarEntry>() {
                                                 new StarEntry("07L",
                                                               "STAR1",
                                                               new List<Waypoint>(){new Waypoint("P1", 19.0, 119.0)},
                                                               EntryType.RwySpecific) }));
            // Invoke
            var destRoute = extractor.Extract();

            // Assert
            Assert.AreEqual(2, route.Count);
            Assert.IsTrue(route.Last.Value == "SIERA");

            Assert.AreEqual(2, destRoute.Count);

            var node = destRoute.First;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("P1", 19.0, 119.0)) &&
                          node.Value.AirwayToNext == "STAR1" &&
                          WithinPrecision(node.Value.DistanceToNext, Distance(19.0, 119.0, 18.0, 118.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 18.0, 118.0)) &&
                          node == destRoute.Last);
        }
    }
}
