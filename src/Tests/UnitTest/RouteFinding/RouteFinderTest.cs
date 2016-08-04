using NUnit.Framework;
using QSP.RouteFinding;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.WindAloft;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFindingTest
{
    [TestFixture]
    public class RouteFinderTest
    {
        [Test]
        public void CanFindRoute()
        {
            var w1 = new Waypoint("1", 0.0, 1.0);
            var w2 = new Waypoint("2", 0.0, 2.0);
            var w3 = new Waypoint("3", 0.0, 3.0);

            var wptList = GetWptList(w1, w2, w3);
            int i1 = wptList.FindByWaypoint(w1);
            int i2 = wptList.FindByWaypoint(w2);
            int i3 = wptList.FindByWaypoint(w3);
            wptList.AddNeighbor(i1, "A", AirwayType.Enroute, i2);
            wptList.AddNeighbor(i2, "B", AirwayType.Enroute, i3);

            var expected = GetRoute(
                w1, "A", -1.0,
                w2, "B", -1.0,
                w3);

            var route = new RouteFinder(wptList).FindRoute(i1, i3);

            Assert.IsTrue(expected.Equals(route));
        }

        [Test]
        public void CanAvoidCountry()
        {
            var w1 = new Waypoint("1", 0.0, 1.0);
            var w2 = new Waypoint("2", 0.0, 2.0, 0);
            var w3 = new Waypoint("3", 5.0, 2.0, 1);
            var w4 = new Waypoint("4", 0.0, 3.0);

            var wptList = GetWptList(w1, w2, w3, w4);
            int i1 = wptList.FindByWaypoint(w1);
            int i2 = wptList.FindByWaypoint(w2);
            int i3 = wptList.FindByWaypoint(w3);
            int i4 = wptList.FindByWaypoint(w4);
            wptList.AddNeighbor(i1, "12", AirwayType.Enroute, i2);
            wptList.AddNeighbor(i1, "13", AirwayType.Enroute, i3);
            wptList.AddNeighbor(i2, "24", AirwayType.Enroute, i4);
            wptList.AddNeighbor(i3, "34", AirwayType.Enroute, i4);

            var avoid = new CountryCodeCollection(new int[] { 0 });
            var route = new RouteFinder(wptList, avoid).FindRoute(i1, i4);

            var expected = GetRoute(
                w1, "13", -1.0,
                w3, "34", -1.0,
                w4);

            Assert.IsTrue(expected.Equals(route));
        }

        public void CanUtilizeWind()
        {
            var w1 = new Waypoint("1", -1.0, 0.0);
            var w2 = new Waypoint("2", 0.0, 1.0);
            var w3 = new Waypoint("3", 0.0, -1.0);
            var w4 = new Waypoint("4", 1.0, 0.0);

            var wptList = GetWptList(w1, w2, w3, w4);
            int i1 = wptList.FindByWaypoint(w1);
            int i2 = wptList.FindByWaypoint(w2);
            int i3 = wptList.FindByWaypoint(w3);
            int i4 = wptList.FindByWaypoint(w4);
            wptList.AddNeighbor(i1, "12", AirwayType.Enroute, i2);
            wptList.AddNeighbor(i1, "13", AirwayType.Enroute, i3);
            wptList.AddNeighbor(i2, "24", AirwayType.Enroute, i4);
            wptList.AddNeighbor(i3, "34", AirwayType.Enroute, i4);

            var stub = new WindTableStub();
            var calc = new AvgWindCalculator(stub, 460.0, 36000.0);
            var route = new RouteFinder(wptList, null, calc).FindRoute(i1, i4);

            var expected = GetRoute(
                w1, "13", -1.0,
                w3, "34", -1.0,
                w4);

            Assert.IsTrue(expected.Equals(route));
        }

        private class WindTableStub : IWindTableCollection
        {
            public WindUV GetWindUV(double lat, double lon, double altitudeFt)
            {
                if (lat >= 0.0) return new WindUV(-100.0, 0.0);
                return new WindUV(100.0, 0.0);
            }
        }
    }
}
