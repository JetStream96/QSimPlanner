using NUnit.Framework;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using static UnitTest.RouteFinding.RouteAnalyzers.Common;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestFixture]
    public class StandardRouteAnalyzerTest
    {
        private AirportManager airportList;
        private SidCollection sids;
        private StarCollection stars;

        private void InitObjects()
        {
            var ac = new AirportCollection();
            ac.Add(GetAirport(
                "RCTP", GetRwyData("05L", 25.072894, 121.215986)));

            ac.Add(GetAirport(
                "VHHH", GetRwyData("07L", 22.310917, 113.897964)));

            airportList = new AirportManager(ac);

            sids = new SidCollection(
                new List<SidEntry>() {
                    new SidEntry(
                        "05L",
                        "SID1",
                        new List<Waypoint>(){new Waypoint("P1",25.1,121.3)},
                        EntryType.RwySpecific,
                        false) });

            stars = new StarCollection(
                new List<StarEntry>() {
                    new StarEntry(
                        "07L",
                        "STAR1",
                        new List<Waypoint>(){new Waypoint("Q1",22.4,113.5)},
                        EntryType.RwySpecific) });
        }

        [Test]
        public void EmptyRouteShouldReturnDirect()
        {
            // Setup
            InitObjects();

            var analyzer = new StandardRouteAnalyzer(
                new string[] { },
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

            var expected = GetRoute(
                new Waypoint("RCTP05L", 25.072894, 121.215986), "DCT", -1.0,
                new Waypoint("VHHH07L", 22.310917, 113.897964));

            Assert.IsTrue(route.Equals(expected));
        }

        #region Group1 - Same route

        [Test]
        public void FullSidStarShouldGetCorrectRoute()
        {
            // Last wpt of SID and first wpt in STAR not in wptList.

            // Setup
            var analyzer = CreateAnalyzer1("SID1", "P1", "Q1", "STAR1");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            AssertRoute1(route);
        }

        [Test]
        public void FullSidStarWithIcaosShouldGetCorrectRoute()
        {
            // Setup
            InitObjects();
            var analyzer = new StandardRouteAnalyzer(
                new string[] { "RCTP", "SID1", "P1", "Q1", "STAR1", "VHHH" },
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
            AssertRoute1(route);
        }

        private StandardRouteAnalyzer CreateAnalyzer1(params string[] route)
        {
            InitObjects();

            return new StandardRouteAnalyzer(
                route,
                "RCTP",
                "05L",
                "VHHH",
                "07L",
                airportList,
                new WaypointList(),
                sids,
                stars);
        }

        private void AssertRoute1(Route route)
        {
            var expected = GetRoute(
                new Waypoint("RCTP05L", 25.072894, 121.215986), "SID1", -1.0,
                new Waypoint("P1", 25.1, 121.3), "DCT", -1.0,
                new Waypoint("Q1", 22.4, 113.5), "STAR1", -1.0,
                new Waypoint("VHHH07L", 22.310917, 113.897964));

            Assert.IsTrue(route.Equals(expected));
        }

        #endregion

        #region Group 2 - Same route

        private WaypointList CreateOneAirway()
        {
            var wptList = new WaypointList();

            int x = wptList.AddWaypoint(new Waypoint("X", 24.0, 117.0));
            int y = wptList.AddWaypoint(new Waypoint("Y", 23.0, 115.0));
            var neighbor = new Neighbor(
                "A1", AirwayType.Enroute, wptList.Distance(x, y));

            wptList.AddNeighbor(x, y, neighbor);

            return wptList;
        }

        [Test]
        public void SidStarAirwayIcao1ShouldGetCorrectRoute()
        {
            // Setup
            var analyzer = CreateAnalyzer2(
                "RCTP", "SID1", "P1", "X", "A1", "Y", "Q1", "STAR1", "VHHH");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            AssertRoute2(route);
        }

        [Test]
        public void SidStarAirwayIcao2ShouldGetCorrectRoute()
        {
            // Setup
            var analyzer = CreateAnalyzer2(
                "RCTP", "SID1", "X", "A1", "Y", "STAR1", "VHHH");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            AssertRoute2(route);
        }

        [Test]
        public void SidStarAirwayIcao3ShouldGetCorrectRoute()
        {
            // Setup
            var analyzer = CreateAnalyzer2(
                "SID1", "X", "A1", "Y", "STAR1");

            // Invoke
            var route = analyzer.Analyze();

            // Assert
            AssertRoute2(route);
        }

        private StandardRouteAnalyzer CreateAnalyzer2(params string[] route)
        {
            InitObjects();

            return new StandardRouteAnalyzer(
                route,
                "RCTP",
                "05L",
                "VHHH",
                "07L",
                airportList,
                CreateOneAirway(),
                sids,
                stars);
        }

        private void AssertRoute2(Route route)
        {
            var expected = GetRoute(
                new Waypoint("RCTP05L", 25.072894, 121.215986), "SID1", -1.0,
                new Waypoint("P1", 25.1, 121.3), "DCT", -1.0,
                new Waypoint("X", 24.0, 117.0), "A1", -1.0,
                new Waypoint("Y", 23.0, 115.0), "DCT", -1.0,
                new Waypoint("Q1", 22.4, 113.5), "STAR1", -1.0,
                new Waypoint("VHHH07L", 22.310917, 113.897964));

            Assert.IsTrue(route.Equals(expected));
        }

        #endregion

    }
}
