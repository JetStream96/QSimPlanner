using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using static QSP.MathTools.Utilities;
using static UnitTest.Common.Utilities;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestClass]
    public class StandardRouteAnalyzerTest
    {
        private AirportManager airportList;
        private SidCollection sids;
        private StarCollection stars;

        private void initObjects()
        {
            var ac = new AirportCollection();

            ac.Add(new Airport("RCTP", "", 0.0, 0.0, 0, true, 0, 0, 0,
                               new List<RwyData>() { new RwyData("05L", "", 0, 0, true, true, "", "", 25.072894, 121.215986, 0, 0.0, 0, 0, 0) }));
            ac.Add(new Airport("VHHH", "", 0.0, 0.0, 0, true, 0, 0, 0,
                               new List<RwyData>() { new RwyData("07L", "", 0, 0, true, true, "", "", 22.310917, 113.897964, 0, 0.0, 0, 0, 0) }));
            airportList = new AirportManager(ac);

            sids = new SidCollection(new List<SidEntry>() {
                                        new SidEntry("05L",
                                                     "SID1",
                                                     new List<Waypoint>(){new Waypoint("P1",25.1,121.3)},
                                                     EntryType.RwySpecific,
                                                     false) });

            stars = new StarCollection(new List<StarEntry>() {
                                        new StarEntry("07L",
                                                      "STAR1",
                                                      new List<Waypoint>(){new Waypoint("Q1",22.4,113.5)},
                                                      EntryType.RwySpecific) });
        }

        [TestMethod]
        public void EmptyRouteShouldReturnDirect()
        {
            // Setup
            initObjects();

            var analyzer = new StandardRouteAnalyzer(new string[] { },
                                                     "RCTP",
                                                     "05L",
                                                     "VHHH",
                                                     "07L",
                                                     airportList,
                                                     null,
                                                     sids,
                                                     stars);
            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 25.072894, 121.215986)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.072894, 121.215986, 22.310917, 113.897964), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 22.310917, 113.897964)) &&
                          node == route.LastNode);
        }

        #region Group1 - Same route

        [TestMethod]
        public void FullSidStarShouldGetCorrectRoute()
        {
            // Last wpt of SID and first wpt in STAR not in wptList.

            // Setup
            var analyzer = createAnalyzer1(new string[] { "SID1", "P1", "Q1", "STAR1" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assertRoute1(route);
        }

        [TestMethod]
        public void FullSidStarWithIcaosShouldGetCorrectRoute()
        {
            // Setup
            initObjects();
            var analyzer = new StandardRouteAnalyzer(new string[] { "RCTP", "SID1", "P1", "Q1", "STAR1", "VHHH" },
                                             "RCTP",
                                             "05L",
                                             "VHHH",
                                             "07L",
                                             airportList,
                                             new WaypointList(),
                                             sids,
                                             stars); 

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 25.072894, 121.215986)) &&
                          node.Value.AirwayToNext == "SID1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.072894, 121.215986, 25.1, 121.3), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("P1", 25.1, 121.3)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.1, 121.3, 22.4, 113.5), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("Q1", 22.4, 113.5)) &&
                          node.Value.AirwayToNext == "STAR1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(22.4, 113.5, 22.310917, 113.897964), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 22.310917, 113.897964)) &&
                          node == route.LastNode);
        }

        private StandardRouteAnalyzer createAnalyzer1(string[] route)
        {
            initObjects();

            return new StandardRouteAnalyzer(route,
                                             "RCTP",
                                             "05L",
                                             "VHHH",
                                             "07L",
                                             airportList,
                                             new WaypointList(),
                                             sids,
                                             stars);
        }

        private void assertRoute1(Route route)
        {
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 25.072894, 121.215986)) &&
                          node.Value.AirwayToNext == "SID1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.072894, 121.215986, 25.1, 121.3), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("P1", 25.1, 121.3)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.1, 121.3, 22.4, 113.5), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("Q1", 22.4, 113.5)) &&
                          node.Value.AirwayToNext == "STAR1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(22.4, 113.5, 22.310917, 113.897964), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 22.310917, 113.897964)) &&
                          node == route.LastNode);
        }

        #endregion

        #region Group 2 - Same route

        private WaypointList createOneAirway()
        {
            var wptList = new WaypointList();

            int x = wptList.AddWaypoint(new Waypoint("X", 24.0, 117.0));
            int y = wptList.AddWaypoint(new Waypoint("Y", 23.0, 115.0));
            wptList.AddNeighbor(x, y, new Neighbor("A1", wptList.Distance(x, y)));

            return wptList;
        }

        [TestMethod]
        public void SidStarAirwayIcao1ShouldGetCorrectRoute()
        {
            // Setup
            var analyzer = createAnalyzer2(new string[] { "RCTP", "SID1", "P1", "X", "A1", "Y", "Q1", "STAR1", "VHHH" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assertRoute2(route);
        }

        [TestMethod]
        public void SidStarAirwayIcao2ShouldGetCorrectRoute()
        {
            // Setup
            var analyzer = createAnalyzer2(new string[] { "RCTP", "SID1", "X", "A1", "Y", "STAR1", "VHHH" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assertRoute2(route);
        }

        [TestMethod]
        public void SidStarAirwayIcao3ShouldGetCorrectRoute()
        {
            // Setup
            var analyzer = createAnalyzer2(new string[] { "SID1", "X", "A1", "Y", "STAR1" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assertRoute2(route);
        }

        private StandardRouteAnalyzer createAnalyzer2(string[] route)
        {
            initObjects();

            return new StandardRouteAnalyzer(route,
                                             "RCTP",
                                             "05L",
                                             "VHHH",
                                             "07L",
                                             airportList,
                                             createOneAirway(),
                                             sids,
                                             stars);
        }

        private void assertRoute2(Route route)
        {
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 25.072894, 121.215986)) &&
                          node.Value.AirwayToNext == "SID1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.072894, 121.215986, 25.1, 121.3), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("P1", 25.1, 121.3)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.1, 121.3, 24.0, 117.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("X", 24.0, 117.0)) &&
                          node.Value.AirwayToNext == "A1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(24.0, 117.0, 23.0, 115.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("Y", 23.0, 115.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(23.0, 115.0, 22.4, 113.5), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("Q1", 22.4, 113.5)) &&
                          node.Value.AirwayToNext == "STAR1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(22.4, 113.5, 22.310917, 113.897964), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 22.310917, 113.897964)) &&
                          node == route.LastNode);
        }

        #endregion

    }
}
