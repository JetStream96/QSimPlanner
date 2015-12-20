using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding;
using QSP.RouteFinding.Containers;
using QSP;
using static Test.Common.Utilities;
using System.Linq;

namespace Test
{

    [TestClass()]
    public class AirwayConnectionFinderTest
    {

        [TestMethod()]
        public void TwoWayAirway_FindBothDirections()
        {
            AirwayConnectionFinder finder = new AirwayConnectionFinder(4, "A001", "WP008", genTwoWayAirway());

            List<int> expected = new List<int>();

            for (int i = 5; i <= 8; i++)
            {
                expected.Add(i);
            }

            Assert.AreEqual(true, Enumerable.SequenceEqual(expected, finder.FindWaypointIndices()));

            //'another dir
            AirwayConnectionFinder finder2 = new AirwayConnectionFinder(6, "A001", "WP002", genTwoWayAirway());
            List<int> exp2 = new List<int>();

            for (int i = 5; i >= 2; i--)
            {
                exp2.Add(i);
            }

            Assert.AreEqual(true, Enumerable.SequenceEqual(exp2, finder2.FindWaypointIndices()));

        }

        [TestMethod()]
        public void OneWayAirway_FindResult()
        {
            AirwayConnectionFinder finder = new AirwayConnectionFinder(9, "A001", "WP004", genOneWayAirway());

            List<int> expected = new List<int>();

            for (int i = 8; i >= 4; i--)
            {
                expected.Add(i);
            }

            Assert.AreEqual(true, Enumerable.SequenceEqual(expected, finder.FindWaypointIndices()));
        }

        [TestMethod()]
        public void OneWayAirway_WrongDir_CannotFind()
        {
            AirwayConnectionFinder finder = new AirwayConnectionFinder(4, "A001", "WP009", genOneWayAirway());
            Assert.IsNull(finder.FindWaypointIndices());
        }

        [TestMethod()]
        public void TwoWayAirway_AirwayDoesnotExist()
        {
            AirwayConnectionFinder finder = new AirwayConnectionFinder(4, "B123", "WP008", genTwoWayAirway());
            Assert.IsNull(finder.FindWaypointIndices());
        }

        [TestMethod()]
        public void TwoWayAirway_TargetWptID_DoesnotExist()
        {
            AirwayConnectionFinder finder = new AirwayConnectionFinder(4, "A001", "WP128", genTwoWayAirway());
            Assert.IsNull(finder.FindWaypointIndices());
        }

        private WaypointList genOneWayAirway()
        {

            WaypointList wpts = new WaypointList();

            //index = 0
            wpts.AddWpt(new Waypoint(wptIDGenerator(1)));

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddWpt(new Waypoint(wptIDGenerator(i)));
            }

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddNeighbor(i, i - 1, new Neighbor("A001", 10));
            }

            return wpts;
        }

        private WaypointList genTwoWayAirway()
        {

            WaypointList wpts = new WaypointList();

            //index = 0
            wpts.AddWpt(new Waypoint(wptIDGenerator(1)));

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddWpt(new Waypoint(wptIDGenerator(i)));
            }

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddNeighbor(i, i - 1, new Neighbor("A001", 10));
            }

            for (int i = 0; i <= 9; i++)
            {
                wpts.AddNeighbor(i, i + 1, new Neighbor("A001", 10));
            }

            return wpts;

        }

        private string wptIDGenerator(int x)
        {
            return "WP" + Convert.ToString(x).PadLeft(3, '0');
        }

    }
}
