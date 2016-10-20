using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Linq;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestFixture]
    public class AnalyzerWithCommandsTest
    {
        private AirportManager airportList;
        private SidCollection sids;
        private StarCollection stars;

        private void InitObjects1()
        {
            airportList = new AirportManager();
            airportList.Add(GetAirport("ABCD", GetRwyData("05L", 25.0, 121.0)));
            airportList.Add(GetAirport("EFGH", GetRwyData("07L", 22.0, 113.0)));
            
            sids = new SidCollection(
                new List<SidEntry>() {
                    new SidEntry(
                        "05L",
                        "SID1",
                        new List<Waypoint>(){new Waypoint("P1",24.0,120.0)},
                        EntryType.RwySpecific,
                        false) });

            stars = new StarCollection(
                new List<StarEntry>() {
                    new StarEntry(
                        "07L",
                        "STAR1",
                        new List<Waypoint>(){new Waypoint("Q1",23.0,114.0)},
                        EntryType.RwySpecific) });
        }

        #region Group 1 - Same route

        private AnalyzerWithCommands GetAnalyzer1(params string[] route)
        {
            InitObjects1();

            var wptList = new WaypointList();

            var wptP1 = new Waypoint("P1", 24.0, 120.0);
            var wptQ1 = new Waypoint("Q1", 23.0, 114.0);
            int p1 = wptList.AddWaypoint(wptP1);
            int q1 = wptList.AddWaypoint(wptQ1);

            var neighbor = new Neighbor("A1", wptP1.Distance(wptQ1));
            wptList.AddNeighbor(p1, q1, neighbor);

            return new AnalyzerWithCommands(
                route.ToRouteString(),
                "ABCD",
                "05L",
                "EFGH",
                "07L",
                airportList,
                wptList,
                wptList.GetEditor(),
                sids,
                stars);
        }

        private void CheckRoute1(Route route)
        {
            var expected = GetRoute(
                new Waypoint("ABCD05L", 25.0, 121.0), "SID1", -1.0,
                new Waypoint("P1", 24.0, 120.0), "A1", -1.0,
                new Waypoint("Q1", 23.0, 114.0), "STAR1", -1.0,
                new Waypoint("EFGH07L", 22.0, 113.0));

            Assert.IsTrue(route.Equals(expected));
        }

        [Test]
        public void AutoAtFirstShouldFindSid()
        {
            // Setup
            var analyzer = GetAnalyzer1("AUTO", "P1", "A1", "Q1", "STAR1");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            CheckRoute1(route);
        }

        [Test]
        public void AutoAtLastShouldFindStar()
        {
            // Setup
            var analyzer = GetAnalyzer1("SID1", "P1", "A1", "Q1", "AUTO");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            CheckRoute1(route);
        }

        [Test]
        public void AutoAtMiddleShouldFindRoute()
        {
            // Setup
            var analyzer = GetAnalyzer1("SID1", "P1", "AUTO", "Q1", "STAR1");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            CheckRoute1(route);
        }

        #endregion

        #region Group 2 - Only coordinates

        private void InitObjects2()
        {
            airportList = new AirportManager();
            var list = airportList;
            list.Add(GetAirport("ABCD", GetRwyData("05L", 25.0, 120.0)));
            list.Add(GetAirport("EFGH", GetRwyData("07L", 43.0, 107.0)));
        }

        private AnalyzerWithCommands GetAnalyzer2(params string[] route)
        {
            InitObjects2();

            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("N37E112", 37.0, 112.0));
            wptList.AddWaypoint(new Waypoint("N30E117", 30.0, 117.0));

            return new AnalyzerWithCommands(
                route.ToRouteString(),
                "ABCD",
                "05L",
                "EFGH",
                "07L",
                airportList,
                wptList,
                wptList.GetEditor(),
                new SidCollection(new SidEntry[0]),
                new StarCollection(new StarEntry[0]));
        }

        [Test]
        public void RandAtFirstShouldDirectFromRwy()
        {
            // Setup
            var analyzer = GetAnalyzer2("RAND", "N37E112");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var expected = GetRoute(
                new Waypoint("ABCD05L", 25.0, 120.0), "DCT", -1.0,
                new Waypoint("28E20", 28.0, 120.0), "DCT", -1.0,
                new Waypoint("N37E112", 37.0, 112.0), "DCT", -1.0,
                new Waypoint("EFGH07L", 43.0, 107.0));

            Assert.IsTrue(route.Equals(expected));
        }

        [Test]
        public void RandAtLastShouldDirectToRwy()
        {
            // Setup
            var analyzer = GetAnalyzer2("N30E117", "RAND");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var expected = GetRoute(
                new Waypoint("ABCD05L", 25.0, 120.0), "DCT", -1.0,
                new Waypoint("N30E117", 30.0, 117.0), "DCT", -1.0,
                new Waypoint("40E10", 40.0, 110.0), "DCT", -1.0,
                new Waypoint("EFGH07L", 43.0, 107.0));

            Assert.IsTrue(route.Equals(expected));
        }

        [Test]
        public void RandAtMiddleShouldFindRandomRoute()
        {
            // Setup
            var analyzer = GetAnalyzer2("N30E117", "RAND", "N37E112");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var expected = GetRoute(
                new Waypoint("ABCD05L", 25.0, 120.0), "DCT", -1.0,
                new Waypoint("N30E117", 30.0, 117.0), "DCT", -1.0,
                new Waypoint("N37E112", 37.0, 112.0), "DCT", -1.0,
                new Waypoint("EFGH07L", 43.0, 107.0));

            Assert.IsTrue(route.Equals(expected));
        }

        #endregion
        
        [Test]
        public void EmptyRouteShouldReturnDirectRoute()
        {
            // Setup
            var analyzer = GetAnalyzer2();

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var directRoute = GetRoute(
                new Waypoint("ABCD05L", 25.0, 120.0), "DCT", -1.0,
                new Waypoint("EFGH07L", 43.0, 107.0));

            Assert.IsTrue(route.Equals(directRoute));
        }
        
        [Test]
        public void RouteOfLatLonShouldAlsoWork()
        {
            // Setup
            var analyzer = GetAnalyzer2("N35.2E113.1", "N31.0E110.0");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var expectedDistance = new LatLon[]
            {
                new LatLon(25.0, 120.0),
                new LatLon(35.2, 113.1),
                new LatLon(31.0, 110.0),
                new LatLon(43.0, 107.0)
            }.TotalDistance();
            
            Assert.AreEqual(expectedDistance, route.GetTotalDistance(), 1E-6);
        }
    }
}
