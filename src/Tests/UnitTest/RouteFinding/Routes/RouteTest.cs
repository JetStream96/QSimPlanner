using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;

namespace UnitTest.RouteFinding.Routes
{
    [TestFixture]
    public class RouteTest
    {
        [Test]
        public void TotalDistanceEmptyRoute()
        {
            Assert.Throws<InvalidOperationException>(() =>
            new Route().TotalDistance());
        }

        [Test]
        public void GetTotalDistanceTest()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);
            var z = new Waypoint("Z", 1.0, 3.0);

            var route = new Route();
            route.AddLastWaypoint(x);
            route.AddLastWaypoint(y, "0");
            route.AddLastWaypoint(z, "1");

            var expectedDis = x.Distance(y) + y.Distance(z);

            Assert.AreEqual(expectedDis, route.TotalDistance());
        }

        [Test]
        public void GetTotalDistanceCustomDistance()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);
            var z = new Waypoint("Z", 1.0, 3.0);

            var route = new Route();
            route.AddLastWaypoint(x);
            route.AddLastWaypoint(y, "0", 100.0);
            route.AddLastWaypoint(z, "1", 200.0);

            Assert.AreEqual(300.0, route.TotalDistance(), 1E-8);
        }

        [Test]
        public void AddFirstWaypointTest1()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);

            var route = new Route();
            route.AddLastWaypoint(y);

            route.AddFirstWaypoint(x, "0", 100.0);

            Assert.IsTrue(x.Equals(route.FirstWaypoint));
            Assert.IsTrue("0" == route.First.Value.AirwayToNext.Airway);
            Assert.AreEqual(100.0, route.First.Value.AirwayToNext.Distance, 1E-8);
            Assert.IsTrue(y.Equals(route.First.Next.Value.Waypoint));
        }

        [Test]
        public void AddFirstWaypointTest1EmptyRoute()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var route = new Route();

            route.AddFirstWaypoint(x, "0", 100.0);

            Assert.IsTrue(x.Equals(route.FirstWaypoint));
            Assert.IsTrue("0" == route.First.Value.AirwayToNext.Airway);
            Assert.AreEqual(100.0, route.First.Value.AirwayToNext.Distance, 1E-8);
        }

        [Test]
        public void AddFirstWaypointTest2()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);

            var route = new Route();
            route.AddLastWaypoint(y);

            route.AddFirstWaypoint(x, "0");

            Assert.IsTrue(x.Equals(route.FirstWaypoint));
            Assert.IsTrue("0" == route.First.Value.AirwayToNext.Airway);
            Assert.AreEqual(
                x.Distance(y), route.First.Value.AirwayToNext.Distance, 1E-8);
            Assert.IsTrue(y.Equals(route.First.Next.Value.Waypoint));
        }

        [Test]
        public void AddFirstWaypointTest2EmptyRoute()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var route = new Route();

            route.AddFirstWaypoint(x, "0");

            Assert.IsTrue(x.Equals(route.FirstWaypoint));
            Assert.IsTrue("0" == route.First.Value.AirwayToNext.Airway);
        }

        [Test]
        public void AddLastWaypointTest1()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);

            var route = new Route();
            route.AddLastWaypoint(x);

            route.AddLastWaypoint(y, "0", 100.0);

            Assert.IsTrue(y.Equals(route.LastWaypoint));
            Assert.IsTrue("0" == route.Last.Previous.Value.AirwayToNext.Airway);
            Assert.AreEqual(
                100.0, route.Last.Previous.Value.AirwayToNext.Distance, 1E-8);
            Assert.IsTrue(x.Equals(route.Last.Previous.Value.Waypoint));
        }

        [Test]
        public void AddLastWaypointTest1EmptyRoute()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var route = new Route();

            route.AddLastWaypoint(x, "0", 100.0);

            Assert.IsTrue(x.Equals(route.LastWaypoint));
        }

        [Test]
        public void AddLastWaypointTest2()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);

            var route = new Route();
            route.AddLastWaypoint(x);

            route.AddLastWaypoint(y, "0");

            Assert.IsTrue(y.Equals(route.LastWaypoint));
            Assert.IsTrue("0" == route.Last.Previous.Value.AirwayToNext.Airway);
            Assert.AreEqual(
                x.Distance(y),
                route.Last.Previous.Value.AirwayToNext.Distance,
                1E-8);

            Assert.IsTrue(x.Equals(route.Last.Previous.Value.Waypoint));
        }

        [Test]
        public void AddLastWaypointTest2EmptyRoute()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var route = new Route();

            route.AddLastWaypoint(x, "0");

            Assert.IsTrue(x.Equals(route.LastWaypoint));
        }

        [Test]
        public void AddLastWaypointTest3()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);

            var route = new Route();
            route.AddFirstWaypoint(x, "0", 100.0);

            route.AddLastWaypoint(y);

            Assert.IsTrue(y.Equals(route.LastWaypoint));
            Assert.IsTrue("0" == route.Last.Previous.Value.AirwayToNext.Airway);
            Assert.AreEqual(
                100.0,
                route.Last.Previous.Value.AirwayToNext.Distance,
                1E-8);

            Assert.IsTrue(x.Equals(route.Last.Previous.Value.Waypoint));
        }

        [Test]
        public void AddLastWaypointTest3EmptyRoute()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var route = new Route();

            route.AddLastWaypoint(x);

            Assert.IsTrue(x.Equals(route.LastWaypoint));
        }

        private Route GetRoute1()
        {
            var route = new Route();

            route.AddLastWaypoint(new Waypoint("Y", 0.0, 1.0));
            route.AddLastWaypoint(new Waypoint("Z", 0.0, 3.0), "1");

            return route;
        }

        [Test]
        public void ConnectRouteTest()
        {
            var x = new Waypoint("X", 0.0, 0.0);
            var y = new Waypoint("Y", 0.0, 1.0);
            var z = new Waypoint("Z", 0.0, 3.0);

            var route = new Route();
            route.AddLastWaypoint(x);
            route.AddLastWaypoint(y, "0");

            var expected = new Route(route);

            route.Connect(GetRoute1());

            expected.AddLastWaypoint(z, "1");

            Assert.IsTrue(expected.Equals(route));
        }

        [Test]
        public void ConnectRouteEmptyNodes()
        {
            var route = new Route();
            route.Connect(GetRoute1());

            Assert.IsTrue(route.Equals(GetRoute1()));
        }

        [Test]
        public void ConnectRouteMismatchShouldThrowException()
        {
            var x = new Waypoint("X", 0.0, 0.0);

            var route = new Route();
            route.AddLastWaypoint(x);

            Assert.Throws<ArgumentException>(() => route.Connect(GetRoute1()));
        }

        [Test]
        public void ToStringOnlyTwoWptsShowDct()
        {
            var route = Common.GetRoute(new Waypoint("VHHH07R"), "DCT", -1,
                                        new Waypoint("OMDB12L"));
            var s = route.ToString(true);
            Assert.AreEqual("DCT", s);
        }

        [Test]
        public void ToStringOnlyTwoWptsShouldStillShowDctDespiteFalse()
        {
            var route = Common.GetRoute(new Waypoint("VHHH07R"), "DCT", -1,
                                        new Waypoint("OMDB12L"));
            var s = route.ToString(false);
            Assert.AreEqual("DCT", s);
        }

        [Test]
        public void ToStringConsecutiveAirways()
        {
            var route = Common.GetRoute(new Waypoint("VHHH07R"), "DCT", -1,
                                        new Waypoint("A"), "1", -1,
                                        new Waypoint("B"), "1", -1,
                                        new Waypoint("C"), "1", -1,
                                        new Waypoint("D"), "2", -1,
                                        new Waypoint("E"), "3", -1,
                                        new Waypoint("OMDB12L"));
            var s = route.ToString(true);
            Assert.AreEqual("DCT A 1 D 2 E 3", s);
        }

        [Test]
        public void ToStringHideDct()
        {
            var route = Common.GetRoute(new Waypoint("VHHH07R"), "DCT", -1,
                                        new Waypoint("A"), "DCT", -1,
                                        new Waypoint("B"), "1", -1,
                                        new Waypoint("OMDB12L"));
            var s = route.ToString(false);
            Assert.AreEqual("A B 1", s);
        }
    }
}
