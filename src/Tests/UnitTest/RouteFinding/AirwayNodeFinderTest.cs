using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System.Linq;

namespace UnitTest.RouteFinding
{
    [TestClass()]
    public class AirwayNodeFinderTest
    {
        [TestMethod()]
        public void TwoWayAirway_FindBothDirections()
        {
            var finder = new AirwayNodeFinder(4, "A001", "WP008", genTwoWayAirway());
            var expected = Enumerable.Range(5, 4);

            Assert.IsTrue(Enumerable.SequenceEqual(expected, finder.GetWaypointIndices()));

            // another dir
            var finder2 = new AirwayNodeFinder(6, "A001", "WP002", genTwoWayAirway());
            var exp2 = Enumerable.Range(2, 4).Reverse();

            Assert.IsTrue(Enumerable.SequenceEqual(exp2, finder2.GetWaypointIndices()));
        }

        [TestMethod()]
        public void OneWayAirway_FindResult()
        {
            var finder = new AirwayNodeFinder(9, "A001", "WP004", genOneWayAirway());
            var expected = Enumerable.Range(4, 5).Reverse();

            Assert.IsTrue(Enumerable.SequenceEqual(expected, finder.GetWaypointIndices()));
        }

        [TestMethod()]
        public void OneWayAirway_WrongDir_CannotFind()
        {
            var finder = new AirwayNodeFinder(4, "A001", "WP009", genOneWayAirway());
            Assert.IsNull(finder.GetWaypointIndices());
        }

        [TestMethod()]
        public void TwoWayAirway_AirwayDoesnotExist()
        {
            var finder = new AirwayNodeFinder(4, "B123", "WP008", genTwoWayAirway());
            Assert.IsNull(finder.GetWaypointIndices());
        }

        [TestMethod()]
        public void TwoWayAirway_TargetWptID_DoesnotExist()
        {
            var finder = new AirwayNodeFinder(4, "A001", "WP128", genTwoWayAirway());
            Assert.IsNull(finder.GetWaypointIndices());
        }

        private WaypointList genOneWayAirway()
        {
            var wpts = new WaypointList();

            //index = 0
            wpts.AddWaypoint(new Waypoint(wptIDGenerator(1)));

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddWaypoint(new Waypoint(wptIDGenerator(i)));
            }

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddNeighbor(i, i - 1, new Neighbor("A001", 10));
            }

            return wpts;
        }

        private WaypointList genTwoWayAirway()
        {
            var wpts = new WaypointList();

            //index = 0
            wpts.AddWaypoint(new Waypoint(wptIDGenerator(1)));

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddWaypoint(new Waypoint(wptIDGenerator(i)));
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
            return "WP" + x.ToString().PadLeft(3, '0');
        }

    }
}
