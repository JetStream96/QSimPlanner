using NUnit.Framework;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using static QSP.LibraryExtension.Lists;
using static UnitTest.Common.Constants;
using static UnitTest.RouteFinding.RouteAnalyzers.Common;

namespace UnitTest.RouteFindingTest.TerminalProceduresTest.Sid
{
    [TestFixture]
    public class SidAdderTest
    {
        private readonly Waypoint rwy =
            new Waypoint("AXYZ18", 25.0003, 50.0001);

        private readonly Waypoint wpt101 =
            new Waypoint("WPT101", 25.0125, 50.0300);

        private readonly Waypoint wpt102 =
            new Waypoint("WPT102", 25.0150, 50.0800);

        private readonly Waypoint wpt103 =
            new Waypoint("WPT103", 25.0175, 50.1300);

        private readonly Waypoint wpt104 =
            new Waypoint("WPT104", 25.0225, 50.1800);

        [Test]
        public void AddToWptListCase1()
        {
            var wptList = Case1WptList();
            var adder = new SidAdder(
                "AXYZ",
                new SidCollection(new List<SidEntry>()),
                wptList,
                wptList.GetEditor(),
                GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", new List<string>());

            // Check the SID is added as an edge
            Assert.AreEqual(2, wptList.EdgesFromCount(rwyIndex));

            foreach (var j in wptList.EdgesFrom(rwyIndex))
            {
                var edge = wptList.GetEdge(j);

                // Name is DCT 
                Assert.AreEqual("DCT", edge.Value.Airway);

                // Distance is correct
                Assert.AreEqual(
                    wptList.Distance(rwyIndex, edge.ToNodeIndex),
                    edge.Value.Distance,
                    DistanceEpsilon);
            }
        }

        private WaypointList Case1WptList()
        {
            return BasicWptList();
        }

        private WaypointList Case2WptList()
        {
            return Case1WptList();
        }

        [Test]
        public void AddToWptListCase2()
        {
            var wptList = Case2WptList();

            var entry = new SidEntry(
                "18",
                "SID1",
                CreateList(wpt101, wpt102),
                EntryType.RwySpecific,
                true);

            var adder = new SidAdder(
                "AXYZ",
                new SidCollection(CreateList(entry)),
                wptList,
                wptList.GetEditor(),
                GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            var distance = CreateList(
                rwy,
                wpt101,
                wpt102)
                .TotalDistance();

            // Check the SID3 has been added with correct total distance.
            Assert.AreEqual(2, wptList.EdgesFromCount(rwyIndex));

            // Check the edges of last wpt 

            foreach (var i in wptList.EdgesFrom(rwyIndex))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("SID1", edge.Value.Airway);

                var expectedDis = distance +
                wpt102.LatLon.Distance(wptList[edge.ToNodeIndex].LatLon);

                Assert.AreEqual(
                    expectedDis, edge.Value.Distance, DistanceEpsilon);
            }
        }

        [Test]
        public void AddToWptListCase3()
        {
            var wptList = Case3WptList();

            var entry = new SidEntry(
                "18",
                "SID1",
                CreateList(wpt101, wpt102, wpt103, wpt104),
                EntryType.RwySpecific,
                false);

            var adder =
                new SidAdder(
                    "AXYZ",
                    new SidCollection(
                        CreateList(entry)),
                        wptList,
                        wptList.GetEditor(),
                        GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesFromCount(rwyIndex) > 0);

            double dis = CreateList(rwy, wpt101, wpt102, wpt103, wpt104)
                .TotalDistance();

            Assert.IsTrue(sidIsAdded(
                rwyIndex,
                "SID1",
                dis,
                wptList));

            // Check the edges of last wpt 
            int index = wptList.FindByWaypoint(wpt104);
            Assert.AreEqual(2, wptList.EdgesFromCount(index));

            foreach (var i in wptList.EdgesFrom(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);

                var expectedDis =
                    wpt104.DistanceFrom(wptList[edge.ToNodeIndex]);

                Assert.AreEqual(
                    expectedDis,
                    edge.Value.Distance,
                    DistanceEpsilon);
            }
        }

        private WaypointList BasicWptList()
        {
            var wptList = new WaypointList();
            int index1 = wptList.AddWaypoint(
                new Waypoint("25N050E", 25.0, 50.0));

            int index2 = wptList.AddWaypoint(
                new Waypoint("27N050E", 27.0, 50.0));

            AddNeighbor(wptList, index1, "AIRWAY1", index2);
            AddNeighbor(wptList, index2, "AIRWAY1", index1);

            return wptList;
        }

        private WaypointList Case3WptList()
        {
            var wptList = BasicWptList();
            wptList.AddWaypoint(new Waypoint("WPT104", 25.0225, 50.1800));
            return wptList;
        }

        private WaypointList Case5WptList()
        {
            return BasicWptList();
        }

        [Test]
        public void AddToWptListCase5()
        {
            var wptList = Case5WptList();

            var entry = new SidEntry(
                "18",
                "SID1",
                CreateList(wpt101, wpt102, wpt103, wpt104),
                EntryType.RwySpecific,
                false);

            var adder =
                new SidAdder(
                    "AXYZ",
                    new SidCollection(
                        CreateList(entry)),
                        wptList,
                        wptList.GetEditor(),
                        GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesFromCount(rwyIndex) > 0);

            double dis = CreateList(rwy, wpt101, wpt102, wpt103, wpt104)
                .TotalDistance();

            Assert.IsTrue(sidIsAdded(
                rwyIndex,
                "SID1",
                dis,
                wptList));

            // Check the edges of last wpt 
            int index = wptList.FindByWaypoint(wpt104);

            Assert.IsTrue(index >= 0);
            Assert.AreEqual(2, wptList.EdgesFromCount(index));

            foreach (var i in wptList.EdgesFrom(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);

                var expectedDis =
                    wpt104.DistanceFrom(wptList[edge.ToNodeIndex]);

                Assert.AreEqual(
                    expectedDis,
                    edge.Value.Distance,
                    DistanceEpsilon);
            }
        }

        [Test]
        public void AddToWptListCase4()
        {
            var wptList = Case4WptList();
            var wpt01 = new Waypoint("WPT01", 25.0, 50.0);
            var wptCoord = new Waypoint("26N050E", 26.0, 50.0);

            var entry = new SidEntry(
                "18",
                "SID1",
                CreateList(wpt01, wptCoord),
                EntryType.RwySpecific,
                false);

            var adder = new SidAdder(
                "AXYZ",
                new SidCollection(CreateList(entry)),
                wptList,
                wptList.GetEditor(),
                GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesFromCount(rwyIndex) > 0);

            var dis = CreateList(rwy, wpt01, wptCoord).TotalDistance();

            Assert.IsTrue(sidIsAdded(
                rwyIndex,
                "SID1",
                dis,
                wptList));
        }

        private WaypointList Case4WptList()
        {
            var wptList = new WaypointList();
            int index = wptList.AddWaypoint(
                new Waypoint("26N050E", 26.0, 50.0));

            int indexNeighbor = wptList.AddWaypoint(
                new Waypoint("27N050E", 27.0, 50.0));

            AddNeighbor(wptList, index, "AIRWAY1", indexNeighbor);
            return wptList;
        }

        private AirportManager GetAirportManager()
        {
            var airports = new AirportCollection();

            var rwy = new RwyData("18", "180", 3500, 60, true, false,
                "0.000", "0", 25.0003, 50.0001, 15, 3.00, 50, "", 0);

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

        private bool sidIsAdded(
            int rwyIndex, string name, double dis, WaypointList wptList)
        {
            foreach (var i in wptList.EdgesFrom(rwyIndex))
            {
                var edge = wptList.GetEdge(i);

                if (edge.Value.Airway == name &&
                    Math.Abs(dis - edge.Value.Distance) <= DistanceEpsilon)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
