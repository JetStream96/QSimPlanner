using NUnit.Framework;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Lists;
using static UnitTest.Common.Constants;
using static UnitTest.RouteFinding.Common;

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

                // No inner waypoints.
                Assert.AreEqual(0, edge.Value.InnerWaypoints.Count);

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

            // Check the SID has been added with correct total distance.
            Assert.AreEqual(2, wptList.EdgesFromCount(rwyIndex));

            // Check the edges of last wpt 
            foreach (var i in wptList.EdgesFrom(rwyIndex))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("SID1", edge.Value.Airway);
                Assert.AreEqual(InnerWaypointsType.Terminal, edge.Value.Type);

                var expectedDis = distance + 
                    wpt102.Distance(wptList[edge.ToNodeIndex]);

                Assert.AreEqual(
                    expectedDis, edge.Value.Distance, DistanceEpsilon);
            }
        }

        [Test]
        public void AddToWptListCase3()
        {
            AddToWptListCase3And5(Case3WptList());
        }

        private void AddToWptListCase3And5(WaypointList wptList)
        {
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

            // Check the SID has been added with correct total distance.
            var edges = wptList.EdgesFrom(rwyIndex).ToList();
            Assert.IsTrue(edges.Count > 0);
            Assert.IsTrue(edges.Select(e => wptList.GetEdge(e))
                .All(e =>
                    Enumerable.SequenceEqual(e.Value.InnerWaypoints, 
                    CreateList(wpt101, wpt102, wpt103)) &&
                    e.Value.Type == InnerWaypointsType.Terminal));

            double dis = CreateList(rwy, wpt101, wpt102, wpt103, wpt104)
                .TotalDistance();

            Assert.IsTrue(SidIsAdded(
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
                var neighbor = edge.Value;
                Assert.AreEqual("DCT", neighbor.Airway);
                Assert.AreEqual(0, neighbor.InnerWaypoints.Count);

                var expectedDis =wpt104.Distance(wptList[edge.ToNodeIndex]);

                Assert.AreEqual(
                    expectedDis,
                    neighbor.Distance,
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

            wptList.AddNeighbor(index1, "AIRWAY1", index2);
            wptList.AddNeighbor(index2, "AIRWAY1", index1);

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
            AddToWptListCase3And5(Case5WptList());
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
            var edges = wptList.EdgesFrom(rwyIndex).ToList();
            Assert.AreEqual(1, edges.Count);
            var edge = wptList.GetEdge(edges[0]);

            Assert.IsTrue(Enumerable.SequenceEqual(
                edge.Value.InnerWaypoints,
                CreateList(wpt01)));
            Assert.AreEqual(InnerWaypointsType.Terminal, edge.Value.Type);

            var dis = CreateList(rwy, wpt01, wptCoord).TotalDistance();

            Assert.IsTrue(SidIsAdded(
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

            wptList.AddNeighbor(index, "AIRWAY1", indexNeighbor);

            return wptList;
        }

        private AirportManager GetAirportManager()
        {
            var rwy = GetRwyData("18", 25.0003, 50.0001);
            var airport = GetAirport("AXYZ", rwy);
            return new AirportManager(new Airport[] { airport });
        }

        private bool SidIsAdded(
            int rwyIndex, string name, double dis, WaypointList wptList)
        {
            return wptList.EdgesFrom(rwyIndex)
                .Any(i =>
                {
                    var edge = wptList.GetEdge(i);

                    return edge.Value.Airway == name &&
                    Math.Abs(dis - edge.Value.Distance) <= DistanceEpsilon;
                });
        }
    }
}
