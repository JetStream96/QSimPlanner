using System.Collections.Generic;
using NUnit.Framework;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Tracks.Interaction;
using System.Linq;
using QSP.RouteFinding.Data.Interfaces;
using static QSP.LibraryExtension.Lists;

namespace UnitTest.RouteFinding.Tracks.Common
{
    [TestFixture]
    public class TrackAdderTest
    {
        [Test]
        public void AddsMainRouteCorrectly()
        {
            // Arrange
            var p1 = new Waypoint("P1", 0.0, 0.0);
            var p2 = new Waypoint("P2", 0.0, 2.0);
            var p3 = new Waypoint("P3", 0.0, 4.0);

            var wptList = new WaypointList();
            wptList.AddWaypoint(p1);
            wptList.AddWaypoint(p2);
            wptList.AddWaypoint(p3);

            var route = new Route();
            route.AddLastWaypoint(p1);
            route.AddLastWaypoint(p2, "DCT");
            route.AddLastWaypoint(p3, "DCT");
            var nodes = new TrackNodes("A", "NATA", route, new WptPair[0]);

            var adder = new TrackAdder(
                wptList,
                wptList.GetEditor(),
                new StatusRecorder(),
                TrackType.Nats);

            // Act
            adder.AddToWaypointList(new TrackNodes[] { nodes });

            // Assert
            int indexP1 = wptList.FindByWaypoint(p1);
            Assert.AreEqual(1, wptList.EdgesFromCount(indexP1));
            
            var edge = wptList.EdgesFrom(indexP1).First();
            var neighbor = wptList.GetEdge(edge).Value;
            Assert.IsTrue(neighbor.Airway == "NATA");

            Assert.IsTrue(Enumerable.SequenceEqual(neighbor.InnerWaypoints,
                CreateList(p2)));
            Assert.AreEqual(NeighborType.Track, neighbor.Type);

            var distance = p1.Distance(p2) + p2.Distance(p3);
            Assert.AreEqual(distance, neighbor.Distance);
        }

        [Test]
        public void WhenMainRouteWptDoesNotExistShouldRecordFailure()
        {
            // Arrange
            var p1 = new Waypoint("P1", 0.0, 0.0);
            var p2 = new Waypoint("P2", 0.0, 2.0);

            var wptList = new WaypointList();

            var route = new Route();
            route.AddLastWaypoint(p1);
            route.AddLastWaypoint(p2, "DCT");
            var nodes = new TrackNodes("A", "NATA", route, new List<WptPair>());

            var recorder = new StatusRecorder();

            var adder = new TrackAdder(
                wptList,
                wptList.GetEditor(),
                recorder,
                TrackType.Nats);

            // Act
            adder.AddToWaypointList(new TrackNodes[] { nodes });

            // Assert
            Assert.AreEqual(1, recorder.Records.Count);
        }
    }
}
