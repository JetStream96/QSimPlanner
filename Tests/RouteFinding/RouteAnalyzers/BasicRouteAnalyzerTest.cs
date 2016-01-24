using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using static QSP.MathTools.Utilities;
using static Tests.Common.Utilities;

namespace Tests.RouteFinding.RouteAnalyzers
{
    [TestClass]
    public class BasicRouteAnalyzerTest
    {

        [TestMethod]
        public void WhenRouteUseAirwaysAnalyzeCorrectness()
        {
            // setup
            var wpts = new Waypoint[]{new Waypoint("P01", 0.0, 15.0),
                                      new Waypoint("P02", 0.0, 16.0),
                                      new Waypoint("P03", 0.0, 17.0),
                                      new Waypoint("P04", 0.0, 18.0)};

            var indices = new List<int>();
            var wptList = new WaypointList();

            for (int i = 0; i < wpts.Length; i++)
            {
                indices.Add(wptList.AddWpt(wpts[i]));
            }

            wptList.AddNeighbor(indices[0], indices[1], new Neighbor("A01", wptList.Distance(indices[0], indices[1])));
            wptList.AddNeighbor(indices[1], indices[2], new Neighbor("A02", wptList.Distance(indices[1], indices[2])));
            wptList.AddNeighbor(indices[1], indices[3], new Neighbor("A03", wptList.Distance(indices[1], indices[3])));

            var analyzer = new BasicRouteAnalyzer(new string[] { "P01", "A01", "P02", "A02", "P03" },
                wptList,
                wptList.FindByID("P01"));
            // invoke 
            var route = analyzer.Analyze();

            // assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[0]) &&
                          node.Value.AirwayToNext == "A01" &&
                          node.Value.DistanceToNext == wptList.Distance(indices[0], indices[1]));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[1]) &&
                          node.Value.AirwayToNext == "A02" &&
                          node.Value.DistanceToNext == wptList.Distance(indices[1], indices[2]));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[2]) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void WhenRouteUseDirectAnalyzeCorrectness()
        {
            // setup
            var wpts = new Waypoint[]{new Waypoint("P01", 0.0, 15.0),
                                      new Waypoint("P02", 0.0, 16.0),
                                      new Waypoint("P03", 0.5, 16.5)};

            var wptList = new WaypointList();

            var analyzer = new BasicRouteAnalyzer(new string[] { "P01", "P02", "P03" },
                wptList,
                wptList.FindByID("P01"));
            // invoke 
            var route = analyzer.Analyze();

            // assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[0]) &&
                          node.Value.AirwayToNext == "DCT" &&
                          node.Value.DistanceToNext == GreatCircleDistance(0.0, 0.0, 15.0, 16.0));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[1]) &&
                          node.Value.AirwayToNext == "DCT" &&
                          node.Value.DistanceToNext == GreatCircleDistance(0.0, 0.0, 16.0, 17.0));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(wpts[2]) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void WhenRouteUseCoordAnalyzeCorrectness()
        {
            // setup
            var analyzer = new BasicRouteAnalyzer(new string[] { "N41W050", "N41.30W50.55" },
                                                  new WaypointList(),
                                                  -1);
            // invoke 
            var route = analyzer.Analyze();

            // assert
            var node = route.FirstNode;
            Assert.IsTrue(WithinPrecision(node.Value.Waypoint.Lat, 41.0, 1E-8) &&
                          WithinPrecision(node.Value.Waypoint.Lon, -50.0, 1E-8) &&
                          node.Value.AirwayToNext == "DCT" &&
                          node.Value.DistanceToNext == GreatCircleDistance(41.0, -50.0, 41.3, -50.55));
           
            node = node.Next;
            Assert.IsTrue(WithinPrecision(node.Value.Waypoint.Lat, 41.3, 1E-8) &&
                          WithinPrecision(node.Value.Waypoint.Lon, -50.55, 1E-8) &&
                          node == route.LastNode);
        }

        [TestMethod]
        public void WhenDistanceTooFarShouldThrowException()
        {
            // setup
            var analyzer = new BasicRouteAnalyzer(new string[] { "N41W050", "N41W060" },
                                                  new WaypointList(),
                                                  -1);
            // invoke 
            var route = analyzer.Analyze();            
        }
    }
}
