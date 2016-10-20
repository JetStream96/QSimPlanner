using NUnit.Framework;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.RouteAnalyzers;
using System;
using System.Collections.Generic;
using static UnitTest.RouteFinding.Common;
using static UnitTest.RouteFinding.RouteAnalyzers.Util;

namespace UnitTest.RouteFinding.RouteAnalyzers
{
    [TestFixture]
    public class BasicRouteAnalyzerTest
    {
        [Test]
        public void SingleEntryInRoute()
        {
            // setup
            var wptList = new WaypointList();
            var p = new Waypoint("P", 20.0, 100.0);
            wptList.AddWaypoint(p);

            var analyzer = new BasicRouteAnalyzer(
                GetRouteString("P"),
                wptList,
                wptList.FindById("P"));

            // invoke
            var route = analyzer.Analyze();

            // assert
            Assert.AreEqual(1, route.Count);
            Assert.IsTrue(route.FirstWaypoint.Equals(p));
        }

        [Test]
        public void WhenRouteUseAirwaysAnalyzeCorrectness()
        {
            // setup
            var wpts = new Waypoint[]{
                new Waypoint("P01", 0.0, 15.0),
                new Waypoint("P02", 0.0, 16.0),
                new Waypoint("P03", 0.0, 17.0),
                new Waypoint("P04", 0.0, 18.0)};

            var indices = new List<int>();
            var wptList = new WaypointList();

            for (int i = 0; i < wpts.Length; i++)
            {
                indices.Add(wptList.AddWaypoint(wpts[i]));
            }

            wptList.AddNeighbor(indices[0], "A01", indices[1]);
            wptList.AddNeighbor(indices[1], "A02", indices[2]);

            // Added so that there are 2 airways to choose from at P03.
            wptList.AddNeighbor(indices[1], "A03", indices[3]);

            var analyzer = new BasicRouteAnalyzer(
                GetRouteString("P01", "A01", "P02", "A02", "P03"),
                wptList,
                wptList.FindById("P01"));

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
        public void WhenRouteUseDirectAnalyzeCorrectness()
        {
            // setup
            var wpts = new Waypoint[]{
                new Waypoint("P01", 0.0, 15.0),
                new Waypoint("P02", 0.0, 16.0),
                new Waypoint("P03", 0.5, 16.5)};

            var wptList = new WaypointList();

            foreach (var i in wpts)
            {
                wptList.AddWaypoint(i);
            }

            var analyzer = new BasicRouteAnalyzer(
                GetRouteString("P01", "P02", "P03"),
                wptList,
                wptList.FindById("P01"));

            // invoke 
            var route = analyzer.Analyze();

            // assert
            var expected = GetRoute(
                wpts[0], "DCT", -1.0,
                wpts[1], "DCT", -1.0,
                wpts[2]);

            Assert.IsTrue(route.Equals(expected));
        }

        [Test]
        public void WhenRouteUseCoordAnalyzeCorrectness()
        {
            // setup
            var analyzer = new BasicRouteAnalyzer(
                GetRouteString("N41W050", "N41.30W50.55"),
                new WaypointList(),
                -1);

            // invoke 
            var route = analyzer.Analyze();

            // assert
            var expected = GetRoute(
                new Waypoint("N41W050", 41.0, -50.0), "DCT", -1.0,
                new Waypoint("N41.30W50.55", 41.30, -50.55));

            Assert.IsTrue(route.Equals(expected));
        }

        [Test]
        public void WhenIdentDoesNotExistShouldThrowException()
        {
            // setup
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("P01", 3.051, 20.0));

            var analyzer = new BasicRouteAnalyzer(
                GetRouteString("P01", "P02"),
                wptList,
                0);

            // invoke 
            Assert.Throws<ArgumentException>(() =>
            analyzer.Analyze());
        }

        [Test]
        public void WhenFirstWptIndexIsWrongShouldThrowException()
        {
            // setup
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("P00", 3.051, 20.0));
            wptList.AddWaypoint(new Waypoint("P00", 3.051, 20.0));

            Assert.Throws<ArgumentException>(() =>
            {
                var analyzer = new BasicRouteAnalyzer(
                    GetRouteString("P01", "P02"),
                    wptList,
                    0);
            });
        }
    }
}
