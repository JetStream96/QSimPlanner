using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System.Collections.Generic;
using static Tests.Common.Utilities;

namespace Tests.RouteFinding.RouteAnalyzers
{
    [TestClass]
    public class AutoSelectAnalyzerTest
    {
        [TestMethod]
        public void WhenPreferredLatLonAreBadShouldStillFindsResult()
        {
            // setup
            var wpts = new Waypoint[]{new Waypoint("P01", 0.0, 15.0),
                                      new Waypoint("P02", 0.0, 16.0),
                                      new Waypoint("P03", 0.0, 17.0),
                                      new Waypoint("P01", 50.0, -30.0)};

            var indices = new List<int>();
            var wptList = new WaypointList();

            for (int i = 0; i < wpts.Length; i++)
            {
                indices.Add(wptList.AddWpt(wpts[i]));
            }

            wptList.AddNeighbor(indices[0], indices[1], new Neighbor("A01", wptList.Distance(indices[0], indices[1])));
            wptList.AddNeighbor(indices[1], indices[2], new Neighbor("A02", wptList.Distance(indices[1], indices[2])));

            // Added so that there are 2 airways to choose from at P03.
            wptList.AddNeighbor(indices[1], indices[3], new Neighbor("A03", wptList.Distance(indices[1], indices[3])));

            var analyzer = new AutoSelectAnalyzer(new string[] { "P01", "A01", "P02", "A02", "P03" },
                                                  50.0,
                                                  -30.0,
                                                  wptList);
            // invoke 
            var route = analyzer.Analyze();

            // assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[0]) &&
                          node.Value.AirwayToNext == "A01" &&
                          WithinPrecision(node.Value.DistanceToNext, wptList.Distance(indices[0], indices[1]), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[1]) &&
                          node.Value.AirwayToNext == "A02" &&
                          WithinPrecision(node.Value.DistanceToNext, wptList.Distance(indices[1], indices[2]), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[2]) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void WhenMultipleWptsHaveSameIdentShouldSortBasedOnDistance()
        {
            // setup
            var wpts = new Waypoint[]{new Waypoint("P01", 20.0, 15.0),
                                      new Waypoint("P01", 40.0, 35.0),
                                      new Waypoint("P01", 60.0, 55.0),
                                      new Waypoint("P02", 40.0, 33.0)};

            var indices = new List<int>();
            var wptList = new WaypointList();

            for (int i = 0; i < wpts.Length; i++)
            {
                indices.Add(wptList.AddWpt(wpts[i]));
            }
            
            var analyzer = new AutoSelectAnalyzer(new string[] { "P01", "P02"},
                                                  45.0,
                                                  40.0,
                                                  wptList);
            // invoke 
            var route = analyzer.Analyze();

            // assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[1]) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, wptList.Distance(indices[1], indices[3]), 1E-8));
            
            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[3]) &&
                          node == route.LastNode);
        }
    }
}
