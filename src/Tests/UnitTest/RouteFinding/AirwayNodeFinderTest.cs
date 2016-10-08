using NUnit.Framework;
using QSP.RouteFinding;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System.Linq;

namespace UnitTest.RouteFinding
{
    [TestFixture]
    public class AirwayNodeFinderTest
    {
        [Test]
        public void TwoWayAirway_FindBothDirections()
        {
            var finder = new AirwayNodeFinder(4, "A001", "WP008", GenTwoWayAirway());
            var expected = Enumerable.Range(5, 4);

            Assert.IsTrue(Enumerable.SequenceEqual(expected, finder.GetWaypointIndices()));

            // another dir
            var finder2 = new AirwayNodeFinder(6, "A001", "WP002", GenTwoWayAirway());
            var exp2 = Enumerable.Range(2, 4).Reverse();

            Assert.IsTrue(Enumerable.SequenceEqual(exp2, finder2.GetWaypointIndices()));
        }

        [Test]
        public void OneWayAirway_FindResult()
        {
            var finder = new AirwayNodeFinder(9, "A001", "WP004", GenOneWayAirway());
            var expected = Enumerable.Range(4, 5).Reverse();

            Assert.IsTrue(Enumerable.SequenceEqual(expected, finder.GetWaypointIndices()));
        }

        [Test]
        public void OneWayAirway_WrongDir_CannotFind()
        {
            var finder = new AirwayNodeFinder(4, "A001", "WP009", GenOneWayAirway());
            Assert.IsNull(finder.GetWaypointIndices());
        }

        [Test]
        public void TwoWayAirway_AirwayDoesnotExist()
        {
            var finder = new AirwayNodeFinder(4, "B123", "WP008", GenTwoWayAirway());
            Assert.IsNull(finder.GetWaypointIndices());
        }

        [Test]
        public void TwoWayAirway_TargetWptID_DoesnotExist()
        {
            var finder = new AirwayNodeFinder(4, "A001", "WP128", GenTwoWayAirway());
            Assert.IsNull(finder.GetWaypointIndices());
        }

        private WaypointList GenOneWayAirway()
        {
            var wpts = new WaypointList();

            //index = 0
            wpts.AddWaypoint(new Waypoint(WptIdGenerator(1)));

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddWaypoint(new Waypoint(WptIdGenerator(i)));
            }

            for (int i = 1; i <= 10; i++)
            {
                var n = new Neighbor("A001", 10);
                wpts.AddNeighbor(i, i - 1, n);
            }

            return wpts;
        }

        private WaypointList GenTwoWayAirway()
        {
            var wpts = new WaypointList();

            //index = 0
            wpts.AddWaypoint(new Waypoint(WptIdGenerator(1)));

            for (int i = 1; i <= 10; i++)
            {
                wpts.AddWaypoint(new Waypoint(WptIdGenerator(i)));
            }

            for (int i = 1; i <= 10; i++)
            {
                var n = new Neighbor("A001", 10);
                wpts.AddNeighbor(i, i - 1, n);
            }

            for (int i = 0; i <= 9; i++)
            {
                var n = new Neighbor("A001", 10);
                wpts.AddNeighbor(i, i + 1, n);
            }

            return wpts;
        }

        private string WptIdGenerator(int x)
        {
            return "WP" + x.ToString().PadLeft(3, '0');
        }

    }
}
