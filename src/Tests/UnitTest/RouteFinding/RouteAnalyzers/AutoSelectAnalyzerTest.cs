using QSP.LibraryExtension;
using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers;
using System.Linq;
using static UnitTest.RouteFinding.Common;
using static UnitTest.RouteFinding.RouteAnalyzers.Util;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestFixture]
    public class AutoSelectAnalyzerTest
    {
        [Test]
        public void WhenPreferredLatLonAreBadShouldStillFindsResult()
        {
            // setup
            var wpts = new[] 
            {
                new Waypoint("P01", 0.0, 15.0),
                new Waypoint("P02", 0.0, 16.0),
                new Waypoint("P03", 0.0, 17.0),
                new Waypoint("P01", 50.0, -30.0)
            };

            var wptList = new WaypointList();
            var indices = wpts.Select(w => wptList.AddWaypoint(w)).ToList();
           
            wptList.AddNeighbor(indices[0], "A01", indices[1]);
            wptList.AddNeighbor(indices[1], "A02", indices[2]);

            // Added so that there are 2 airways to choose from at P03.
            wptList.AddNeighbor(indices[1], "A03", indices[3]);

            var analyzer = new AutoSelectAnalyzer(
                GetRouteString("P01", "A01", "P02", "A02", "P03"),
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
            var wpts = new[]
            {
                new Waypoint("P01", 20.0, 15.0),
                new Waypoint("P01", 40.0, 35.0),
                new Waypoint("P01", 60.0, 55.0),
                new Waypoint("P02", 40.0, 33.0)
            };
            
            var wptList = new WaypointList();
            wpts.ForEach(w => wptList.AddWaypoint(w));
            
            var analyzer = new AutoSelectAnalyzer(
                GetRouteString("P01", "P02"),
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
