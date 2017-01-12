using NUnit.Framework;
using QSP.RouteFinding;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Data.Interfaces;
using QSP.WindAloft;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFinding
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
            wptList.AddNeighbor(i1, "A", i2);
            wptList.AddNeighbor(i2, "B", i3);

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
            wptList.AddNeighbor(i1, "12", i2);
            wptList.AddNeighbor(i1, "13", i3);
            wptList.AddNeighbor(i2, "24", i4);
            wptList.AddNeighbor(i3, "34", i4);

            var avoid = new CountryCodeCollection(new[] { 0 });
            var route = new RouteFinder(wptList, avoid).FindRoute(i1, i4);

            var expected = GetRoute(
                w1, "13", -1.0,
                w3, "34", -1.0,
                w4);

            Assert.IsTrue(expected.Equals(route));
        }

        [Test]
        public void ComputesEdgeDistanceWithInnerWaypointsCorrectly()
        {
            var w0 = new Waypoint("0", 0.0, 0.0);
            var w1 = new Waypoint("1", 1.0, 1.0);
            var w2 = new Waypoint("2", 1.0, -0.9999);
            var w3 = new Waypoint("3", 0.0, 2.0);

            var wptList = GetWptList(w0, w1, w2, w3);
            int i0 = wptList.FindByWaypoint(w0);
            int i1 = wptList.FindByWaypoint(w1);
            int i2 = wptList.FindByWaypoint(w2);
            int i3 = wptList.FindByWaypoint(w3);
            var n = new Neighbor(
                "03", 
                new [] { w0, w1, w2 }.TotalDistance(), 
                new [] { w1 }, InnerWaypointsType.Track);

            wptList.AddNeighbor(i0, i2, n);
            wptList.AddNeighbor(i0, "02", i2);
            wptList.AddNeighbor(i2, "23", i3);

            var route = new RouteFinder(wptList).FindRoute(i0, i3);

            var expected = GetRoute(
                w0, "02", -1.0,
                w2, "23", -1.0,
                w3);

            Assert.IsTrue(expected.Equals(route));
        }

        [Test]
        public void CanUtilizeWind()
        {
            var w1 = new Waypoint("1", 0.0, 0.0);
            var w2 = new Waypoint("2", 0.999, 1.0);
            var w3 = new Waypoint("3", 0.999, 2.0);
            var w4 = new Waypoint("4", -1.0, 1.0);
            var w5 = new Waypoint("5", -1.0, 2.0);
            var w6 = new Waypoint("6", 0.0, 3.0);

            var wptList = GetWptList(w1, w2, w3, w4, w5, w6);
            int i1 = wptList.FindByWaypoint(w1);
            int i2 = wptList.FindByWaypoint(w2);
            int i3 = wptList.FindByWaypoint(w3);
            int i4 = wptList.FindByWaypoint(w4);
            int i5 = wptList.FindByWaypoint(w5);
            int i6 = wptList.FindByWaypoint(w6);
            wptList.AddNeighbor(i1, "12", i2);
            wptList.AddNeighbor(i2, "23", i3);
            wptList.AddNeighbor(i3, "36", i6);
            wptList.AddNeighbor(i1, "14", i4);
            wptList.AddNeighbor(i4, "45", i5);
            wptList.AddNeighbor(i5, "56", i6);

            var stub = new WindTableStub();
            var calc = new AvgWindCalculator(stub, 460.0, 36000.0);
            var route = new RouteFinder(wptList, null, calc).FindRoute(i1, i6);

            var expected = GetRoute(
                w1, "14", -1.0,
                w4, "45", -1.0, 
                w5, "56", -1.0,
                w6);

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
