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

            var wptList = new WaypointList();
            wptList.AddWaypoint(p1);
            wptList.AddWaypoint(p2);

            var route = new Route();
            route.AddLastWaypoint(p1);
            route.AddLastWaypoint(p2, "DCT");
            var nodes = new TrackNodes("A", "NATA", route, new List<WptPair>());

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
            Assert.IsTrue(neighbor.Distance == p1.Distance(p2));
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

        // TODO: Add tests to make sure InnerWaypoints is correctly added.
    }
}
