using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using System.Linq;

namespace UnitTest.RouteFinding.Routes
{
    [TestFixture]
    public class RouteExtensionsTest
    {
        private Route getRoute1()
        {
            var route = new Route();

            route.AddLastWaypoint(new Waypoint("1", 0.0, 0.0));
            route.AddLastWaypoint(new Waypoint("2", 0.0, 0.0), "A");
            route.AddLastWaypoint(new Waypoint("3", 0.0, 0.0), "B");

            return route;
        }

        private Route getRoute2()
        {
            var route = new Route();

            route.AddLastWaypoint(new Waypoint("2", 0.0, 0.0));
            route.AddLastWaypoint(new Waypoint("4", 0.0, 0.0), "C");
            route.AddLastWaypoint(new Waypoint("3", 0.0, 0.0), "D");

            return route;
        }

        [Test]
        public void InsertRouteTest()
        {
            var route = getRoute1();

            route.Nodes.InsertRoute(getRoute2().Nodes, "B");

            Assert.IsTrue(Enumerable.SequenceEqual(
                route.Nodes.Select(n => n.Waypoint.ID),
                new string[] { "1", "2", "4", "3" }));

            Assert.IsTrue(Enumerable.SequenceEqual(
                route.Nodes.Select(n => n.AirwayToNext).Take(3),
                new string[] { "A", "C", "D" }));
        }

        [Test]
        public void InsertRouteNoMatchShouldLeaveRouteUntouched()
        {
            var route = getRoute1();

            route.Nodes.InsertRoute(getRoute2().Nodes, "A");

            Assert.IsTrue(Enumerable.SequenceEqual(
                route, getRoute1()));
        }
    }
}
