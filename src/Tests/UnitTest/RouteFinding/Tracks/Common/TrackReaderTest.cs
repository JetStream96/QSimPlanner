using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Tracks.Common;
using QSP.RouteFinding.Tracks.Pacots;
using System.Linq;

namespace UnitTest.RouteFinding.Tracks.Common
{
    [TestFixture]
    public class TrackReaderTest
    {
        [Test]
        public void AddsMainRouteCorrectly()
        {
            // Arrange
            var p1 = new Waypoint("P1", 0.0, 0.0);
            var p2 = new Waypoint("P2", 5.0, 5.0);

            var wptList = new WaypointList();
            wptList.AddWaypoint(p1);
            wptList.AddWaypoint(p2);

            var reader = new TrackReader<PacificTrack>(
                wptList, 
                new AirportManager());

            // Act
            var nodes = reader.Read(
                new PacificTrack(
                    PacotDirection.Westbound,
                    "A",
                    "",
                    "",
                    "",
                    new string[] { "P1", "P2" },
                    new string[0][],
                    new string[0][],
                    new LatLon(0.0, 0.0),
                    new LatLon(0.0, 0.0)));

            // Assert
            var route = nodes.MainRoute;

            Assert.AreEqual(2, route.Count);

            var n = route.First.Value;

            Assert.IsTrue(n.Waypoint.Equals(p1));
            Assert.IsTrue(n.Neighbor.Airway == "DCT" && 
                n.Neighbor.Distance == p1.Distance(p2));
            Assert.IsTrue(route.LastWaypoint.Equals(p2));
        }

        [Test]
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
            var neighbor = new Neighbor("A1", q1.Distance(q2));

            wptList.AddNeighbor(q1Index, q2Index, neighbor);

            var reader = new TrackReader<PacificTrack>(
                wptList, new AirportManager());

            string[] routeFrom = { "Q1", "A1", "Q2", "UPR", "Q3", "P1" };

            // Act
            var nodes = reader.Read(
                new PacificTrack(
                    PacotDirection.Westbound,
                    "A",
                    "",
                    "",
                    "",
                    new string[] { "P1", "P2" },
                    new string[][] { routeFrom },
                    new string[0][],
                    new LatLon(0.0, 0.0),
                    new LatLon(0.0, 0.0)));

            // Assert
            var pairs = nodes.ConnectionRoutes;
            Assert.AreEqual(1, pairs.Count());

            var pair = pairs.First();
            Assert.AreEqual(q3Index, pair.IndexFrom);
            Assert.AreEqual(p1Index, pair.IndexTo);
        }
    }
}
