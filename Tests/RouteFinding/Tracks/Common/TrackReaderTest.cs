using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Tracks.Interaction;
using System.Linq;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.AviationTools.Coordinates;
namespace Tests.RouteFinding.Tracks.Common
{
    [TestClass]
    public class TrackReaderTest
    {
        [TestMethod]
        public void AddsMainRouteCorrectly()
        {
            // Arrange
            var p1 = new Waypoint("P1", 0.0, 0.0);
            var p2 = new Waypoint("P2", 5.0, 5.0);

            var wptList = new WaypointList();
            wptList.AddWaypoint(p1);
            wptList.AddWaypoint(p2);

            var reader = new TrackReader<PacificTrack>(wptList);

            // Act
            var nodes = reader.Read(new PacificTrack(
                                          PacotDirection.Westbound,
                                          "A",
                                          "",
                                          "",
                                          "",
                                          new List<string> { "P1", "P2" }.AsReadOnly(),
                                          new List<string[]>().AsReadOnly(),
                                          new List<string[]>().AsReadOnly(),
                                          new LatLon(0.0, 0.0)));
            // Assert
            var route = nodes.MainRoute;

            Assert.AreEqual(2, route.Count);

            var n = route.First;

            Assert.IsTrue(n.Waypoint.Equals(p1));
            Assert.IsTrue(n.AirwayToNext == "DCT" && n.DistanceToNext == p1.DistanceFrom(p2));
            Assert.IsTrue(route.Last.Waypoint.Equals(p2));
        }

        [TestMethod]
        public void ConnectionRoutesAddedCorrectly()
        {
            // Arrange
            var p1 = new Waypoint("P1", 0.0, 0.0);
            var p2 = new Waypoint("P2", 5.0, 5.0);

            var q1 = new Waypoint("Q1", 3.0, 3.0);
            var q2 = new Waypoint("Q2", 2.0, 3.0);
            var q3 = new Waypoint("Q3", 1.0, 3.0);

            var wptList = new WaypointList();
            int p1Index = wptList.AddWaypoint(p1);
            wptList.AddWaypoint(p2);
            int q1Index = wptList.AddWaypoint(q1);
            int q2Index = wptList.AddWaypoint(q2);
            int q3Index = wptList.AddWaypoint(q3);
            wptList.AddNeighbor(q1Index, q2Index, new Neighbor("A1", q1.DistanceFrom(q2)));

            var reader = new TrackReader<PacificTrack>(wptList);
            string[] routeFrom = new string[] { "Q1", "A1", "Q2", "UPR", "Q3", "P1" };

            // Act
            var nodes = reader.Read(new PacificTrack(
                                          PacotDirection.Westbound,
                                          "A",
                                          "",
                                          "",
                                          "",
                                          new List<string> { "P1", "P2" }.AsReadOnly(),
                                          new List<string[]> { routeFrom }.AsReadOnly(),
                                          new List<string[]>().AsReadOnly(),
                                          new LatLon(0.0, 0.0)));
            // Assert
            var pairs = nodes.PairsToAdd;
            Assert.AreEqual(1, pairs.Count);

            var pair = pairs.First();
            Assert.AreEqual(q3Index, pair.IndexFrom);
            Assert.AreEqual(p1Index, pair.IndexTo);
        }
    }
}
