using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
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
using static Tests.Common.Utilities;

namespace Tests.RouteFinding.RouteAnalyzers
{
    [TestClass]
    public class AnalyzerWithCommandsTest
    {
        private AirportManager airportList;
        private SidCollection sids;
        private StarCollection stars;

        private void initObjects1()
        {
            var ac = new AirportCollection();

            ac.Add(new Airport("ABCD", "", 0.0, 0.0, 0, 0, 0, 0,
                               new List<RwyData>() { new RwyData("05L", "", 0, 0, true, "", "", 25.0, 121.0, 0, 0.0, 0, 0, 0) }));
            ac.Add(new Airport("EFGH", "", 0.0, 0.0, 0, 0, 0, 0,
                               new List<RwyData>() { new RwyData("07L", "", 0, 0, true, "", "", 22.0, 113.0, 0, 0.0, 0, 0, 0) }));
            airportList = new AirportManager(ac);

            sids = new SidCollection(new List<SidEntry>() {
                                        new SidEntry("05L",
                                                     "SID1",
                                                     new List<Waypoint>(){new Waypoint("P1",24.0,120.0)},
                                                     EntryType.RwySpecific,
                                                     false) });

            stars = new StarCollection(new List<StarEntry>() {
                                        new StarEntry("07L",
                                                      "STAR1",
                                                      new List<Waypoint>(){new Waypoint("Q1",23.0,114.0)},
                                                      EntryType.RwySpecific) });
        }

        #region Group 1 - Same route

        private AnalyzerWithCommands getAnalyzer1(string[] route)
        {
            initObjects1();

            var wptList = new WaypointList();
            int p1 = wptList.AddWaypoint(new Waypoint("P1", 24.0, 120.0));
            int q1 = wptList.AddWaypoint(new Waypoint("Q1", 23.0, 114.0));
            wptList.AddNeighbor(p1, q1, new Neighbor("A1", GreatCircleDistance(24.0, 120.0, 23.0, 114.0)));

            return new AnalyzerWithCommands(route,
                                            "ABCD",
                                            "05L",
                                            "EFGH",
                                            "07L",
                                            airportList,
                                            wptList,
                                            new SidHandler("ABCD", sids, wptList, wptList.GetEditor(), airportList),
                                            new StarHandler("EFGH", stars, wptList, wptList.GetEditor(), airportList));
        }

        private void assert1(Route route)
        {
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("ABCD05L", 25.0, 121.0)) &&
                          node.Value.AirwayToNext == "SID1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.0, 121.0, 24.0, 120.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("P1", 24.0, 120.0)) &&
                          node.Value.AirwayToNext == "A1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(24.0, 120.0, 23.0, 114.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("Q1", 23.0, 114.0)) &&
                          node.Value.AirwayToNext == "STAR1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(23.0, 114.0, 22.0, 113.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("EFGH07L", 22.0, 113.0)) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void AutoAtFirstShouldFindSid()
        {
            // Setup            
            var analyzer = getAnalyzer1(new string[] { "AUTO", "P1", "A1", "Q1", "STAR1" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assert1(route);
        }

        [TestMethod]
        public void AutoAtLastShouldFindStar()
        {
            // Setup            
            var analyzer = getAnalyzer1(new string[] { "SID1", "P1", "A1", "Q1", "AUTO" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assert1(route);
        }

        [TestMethod]
        public void AutoAtMiddleShouldFindRoute()
        {
            // Setup            
            var analyzer = getAnalyzer1(new string[] { "SID1", "P1", "AUTO", "Q1", "STAR1" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            assert1(route);
        }

        #endregion

        #region Group 2 - Only coordinates

        private void initObjects2()
        {
            var ac = new AirportCollection();

            ac.Add(new Airport("ABCD", "", 0.0, 0.0, 0, 0, 0, 0,
                               new List<RwyData>() { new RwyData("05L", "", 0, 0, true, "", "", 25.0, 120.0, 0, 0.0, 0, 0, 0) }));
            ac.Add(new Airport("EFGH", "", 0.0, 0.0, 0, 0, 0, 0,
                               new List<RwyData>() { new RwyData("07L", "", 0, 0, true, "", "", 43.0, 107.0, 0, 0.0, 0, 0, 0) }));
            airportList = new AirportManager(ac);
        }

        private AnalyzerWithCommands getAnalyzer2(string[] route)
        {
            initObjects2();

            var wptList = new WaypointList();

            return new AnalyzerWithCommands(route,
                                            "ABCD",
                                            "05L",
                                            "EFGH",
                                            "07L",
                                            airportList,
                                            new WaypointList(),
                                            new SidHandler("ABCD", new SidCollection(new List<SidEntry>()), wptList, wptList.GetEditor(), airportList),
                                            new StarHandler("EFGH", new StarCollection(new List<StarEntry>()), wptList, wptList.GetEditor(), airportList));
        }

        [TestMethod]
        public void RandAtFirstShouldDirectFromRwy()
        {
            // Setup            
            var analyzer = getAnalyzer2(new string[] { "RAND", "N37E112" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("ABCD05L", 25.0, 120.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.0, 120.0, 30.0, 117.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(30.0, 117.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(30.0, 117.0, 37.0, 112.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(37.0, 112.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(37.0, 112.0, 43.0, 107.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("EFGH07L", 43.0, 107.0)) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void RandAtLastShouldDirectToRwy()
        {
            // Setup            
            var analyzer = getAnalyzer2(new string[] { "N30E117", "RAND" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("ABCD05L", 25.0, 120.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.0, 120.0, 30.0, 117.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(30.0, 117.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(30.0, 117.0, 33.0, 115.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(33.0, 115.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(33.0, 115.0, 37.0, 112.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(37.0, 112.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(37.0, 112.0, 43.0, 107.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("EFGH07L", 43.0, 107.0)) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void RandAtMiddleShouldFindRandomRoute()
        {
            // Setup            
            var analyzer = getAnalyzer2(new string[] { "N30E117", "RAND", "N37E112" });

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("ABCD05L", 25.0, 120.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.0, 120.0, 30.0, 117.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(30.0, 117.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(30.0, 117.0, 37.0, 112.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.LatLon.Equals(new LatLon(37.0, 112.0)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(37.0, 112.0, 43.0, 107.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("EFGH07L", 43.0, 107.0)) &&
                          node == route.LastNode);
        }

        #endregion

    }
}
