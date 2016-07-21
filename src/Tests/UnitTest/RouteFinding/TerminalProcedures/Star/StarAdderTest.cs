using NUnit.Framework;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Lists;
using static UnitTest.Common.Constants;
using static UnitTest.RouteFinding.RouteAnalyzers.Common;

namespace UnitTest.RouteFindingTest.TerminalProceduresTest.Star
{
    [TestFixture]
    public class StarAdderTest
    {
        private readonly Waypoint wpt101 =
            new Waypoint("WPT101", 25.0125, 50.0300);

        private readonly Waypoint wpt102 =
            new Waypoint("WPT102", 25.0150, 50.0800);

        private readonly Waypoint wpt103 =
            new Waypoint("WPT103", 25.0175, 50.1300);

        private readonly Waypoint wpt104 =
            new Waypoint("WPT104", 25.0225, 50.1800);

        private readonly Waypoint rwy =
            new Waypoint("AXYZ18", 25.0003, 50.0001);

        [Test]
        public void AddToWptListCase1()
        {
            var wptList = Case1WptList();
            var adder = new StarAdder(
                "AXYZ",
                new StarCollection(new List<StarEntry>()),
                wptList,
                wptList.GetEditor(),
                GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", new List<string>());

            // Check the STAR is added 
            Assert.IsTrue(DirectAdded(
                wptList, new Waypoint("25N050E", 25.0, 50.0), rwyIndex));

            Assert.IsTrue(DirectAdded(
                wptList, new Waypoint("27N050E", 27.0, 50.0), rwyIndex));
        }

        private static bool DirectAdded(
            WaypointList wptList, Waypoint wpt, int rwyIndex)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindByWaypoint(wpt)))
            {
                var edge = wptList.GetEdge(i);
                double disDiff = wpt.DistanceFrom(wptList[rwyIndex]) -
                    edge.Value.Distance;

                if (edge.Value.Airway == "DCT" &&
                    edge.Value.AirwayType == AirwayType.Terminal &&
                    Math.Abs(disDiff) < DistanceEpsilon)
                {
                    return true;
                }
            }

            return false;
        }

        private static WaypointList Case1WptList()
        {
            return BasicWptList();
        }

        private static WaypointList BasicWptList()
        {
            var wptList = new WaypointList();

            int index1 = wptList.AddWaypoint(
                new Waypoint("25N050E", 25.0, 50.0));
            int index2 = wptList.AddWaypoint(
                new Waypoint("27N050E", 27.0, 50.0));

            AddNeighbor(wptList, index1, "AIRWAY1",
                AirwayType.Terminal, index2);
            AddNeighbor(wptList, index2, "AIRWAY1",
                AirwayType.Terminal, index1);

            return wptList;
        }

        [Test]
        public void AddToWptListCase2()
        {
            AddToWptListCase2And4(Case2WptList());
        }

        private static WaypointList Case2WptList()
        {
            var wptList = BasicWptList();
            wptList.AddWaypoint(new Waypoint("WPT101", 25.0125, 50.0300));
            return wptList;
        }

        private WaypointList Case4WptList()
        {
            return BasicWptList();
        }

        private void AddToWptListCase2And4(WaypointList wptList)
        {
            var entry = new StarEntry(
                "18",
                "STAR1",
                CreateList(wpt101, wpt102, wpt103, wpt104),
                EntryType.RwySpecific);

            var adder = new StarAdder(
                "AXYZ",
                new StarCollection(CreateList(entry)),
                wptList,
                wptList.GetEditor(),
                GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", CreateList("STAR1"));

            // Check the STAR1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesToCount(rwyIndex) > 0);

            double dis = CreateList(wpt101, wpt102, wpt103, wpt104, rwy)
               .TotalDistance();

            Assert.IsTrue(StarIsAdded(
                wptList.FindByWaypoint(wpt101),
                "STAR1",
                dis,
                wptList));

            // Check the edges of first wpt 
            int index = wptList.FindByWaypoint(wpt101);

            Assert.IsTrue(index >= 0);
            Assert.AreEqual(2, wptList.EdgesToCount(index));

            foreach (var i in wptList.EdgesTo(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);
                Assert.AreEqual(AirwayType.Terminal, edge.Value.AirwayType);

                double expectedDis =
                    wpt101.DistanceFrom(wptList[edge.FromNodeIndex]);

                Assert.AreEqual(
                    expectedDis, edge.Value.Distance, DistanceEpsilon);
            }
        }

        [Test]
        public void AddToWptListCase4()
        {
            AddToWptListCase2And4(Case4WptList());
        }

        [Test]
        public void AddToWptListCase3()
        {
            var wptList = Case4WptList();
            var wptCorrds = new Waypoint("26N050E", 26.0, 50.0);
            var wpt01 = new Waypoint("WPT01", 25.0, 50.0);

            var entry = new StarEntry(
                "18",
                "STAR1",
                CreateList(wptCorrds, wpt01),
                EntryType.RwySpecific);

            var adder = new StarAdder(
                "AXYZ",
                new StarCollection(CreateList(entry)),
                wptList,
                wptList.GetEditor(),
                GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", CreateList("STAR1"));

            // Check the STAR1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesToCount(rwyIndex) == 1);

            double dis = CreateList(wptCorrds, wpt01, rwy).TotalDistance();

            Assert.IsTrue(StarIsAdded(
                wptList.FindByWaypoint(wptCorrds),
                "STAR1",
                dis,
                wptList));
        }

        private WaypointList Case3WptList()
        {
            var wptList = new WaypointList();
            int index = wptList.AddWaypoint(
                new Waypoint("26N050E", 26.0, 50.0));
            int indexNeighbor = wptList.AddWaypoint(
                new Waypoint("27N050E", 27.0, 50.0));

            AddNeighbor(wptList, index, "AIRWAY1",
                AirwayType.Terminal, indexNeighbor);
            return wptList;
        }

        private AirportManager GetAirportManager()
        {
            var airports = new AirportCollection();

            var rwy = new RwyData(
                "18", "180", 3500, 60, true, false, "0.000", "0",
                25.0003, 50.0001, 15, 3.00, 50, "", 0);

            airports.Add(new Airport(
                "AXYZ",
                "Test Airport 01",
                25.0,
                50.0,
                15,
                true,
                5000,
                8000,
                3500,
                CreateList(rwy)));

            return new AirportManager(airports);
        }

        private bool StarIsAdded(
            int fromIndex, string name, double dis, WaypointList wptList)
        {
            return wptList.EdgesFrom(fromIndex).Any(i =>
            {
                var edge = wptList.GetEdge(i);

                return edge.Value.Airway == name &&
                edge.Value.AirwayType == AirwayType.Terminal &&
                Math.Abs(dis - edge.Value.Distance) < DistanceEpsilon;
            });
        }
    }
}
