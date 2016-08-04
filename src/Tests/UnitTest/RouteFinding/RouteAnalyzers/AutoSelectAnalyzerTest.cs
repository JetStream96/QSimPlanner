using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers;
using System.Collections.Generic;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestFixture]
    public class AutoSelectAnalyzerTest
    {
        [Test]
        public void WhenPreferredLatLonAreBadShouldStillFindsResult()
        {
            // setup
            var wpts = new Waypoint[]{
                new Waypoint("P01", 0.0, 15.0),
                new Waypoint("P02", 0.0, 16.0),
                new Waypoint("P03", 0.0, 17.0),
                new Waypoint("P01", 50.0, -30.0)};

            var indices = new List<int>();
            var wptList = new WaypointList();

            for (int i = 0; i < wpts.Length; i++)
            {
                indices.Add(wptList.AddWaypoint(wpts[i]));
            }

            wptList.AddNeighbor(indices[0], "A01",
                AirwayType.Enroute, indices[1]);

            wptList.AddNeighbor(indices[1], "A02",
                AirwayType.Enroute, indices[2]);

            // Added so that there are 2 airways to choose from at P03.
            wptList.AddNeighbor(indices[1], "A03",
                AirwayType.Enroute, indices[3]);

            var analyzer = new AutoSelectAnalyzer(
                new string[] { "P01", "A01", "P02", "A02", "P03" },
                new LatLon(50.0, -30.0),
                new LatLon(50.0, -30.0),
                wptList);

            // invoke 
            var route = analyzer.Analyze();

            // assert
            var expected = GetRoute(
                wpts[0], "A01", -1.0,
                wpts[1], "A02", -1.0,
                wpts[2]);

            Assert.IsTrue(route.Equals(expected));
        }

        [Test]
        public void WhenMultipleWptsHaveSameIdentShouldSortBasedOnDistance()
        {
            // setup
            var wpts = new Waypoint[]{
                new Waypoint("P01", 20.0, 15.0),
                new Waypoint("P01", 40.0, 35.0),
                new Waypoint("P01", 60.0, 55.0),
                new Waypoint("P02", 40.0, 33.0)};

            var indices = new List<int>();
            var wptList = new WaypointList();

            for (int i = 0; i < wpts.Length; i++)
            {
                indices.Add(wptList.AddWaypoint(wpts[i]));
            }

            var analyzer = new AutoSelectAnalyzer(
                new string[] { "P01", "P02" },
                new LatLon(45.0, 40.0),
                new LatLon(45.0, 40.0),
                wptList);

            // invoke 
            var route = analyzer.Analyze();

            // assert
            var expected = GetRoute(
                wpts[1], "DCT", -1.0,
                wpts[3]);

            Assert.IsTrue(route.Equals(expected));
        }
    }
}
