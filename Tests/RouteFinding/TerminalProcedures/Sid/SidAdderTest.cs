using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System.Collections.Generic;
using static QSP.LibraryExtension.Lists;
using static QSP.RouteFinding.Utilities;
using static Tests.Common.Utilities;
using QSP.AviationTools.Coordinates;

namespace Tests.RouteFindingTest.TerminalProceduresTest.Sid
{
    [TestClass]
    public class SidAdderTest
    {
        [TestMethod]
        public void AddToWptListCase1()
        {
            var wptList = Case1WptList();
            var adder = new SidAdder("AXYZ",
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
                Assert.IsTrue(WithinPrecisionPercent(wptList.Distance(rwyIndex, edge.ToNodeIndex),
                                                     edge.Value.Distance,
                                                     0.1));
            }
        }

        private WaypointList Case1WptList()
        {
            var wptList = new WaypointList();

            int index1 = wptList.AddWaypoint(new Waypoint("25N050E", 25.0, 50.0));
            int index2 = wptList.AddWaypoint(new Waypoint("27N050E", 27.0, 50.0));
            wptList.AddNeighbor(index1, index2, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));
            wptList.AddNeighbor(index2, index1, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));

            return wptList;
        }

        private WaypointList Case2WptList()
        {
            return Case1WptList();
        }

        [TestMethod]
        public void AddToWptListCase2()
        {
            var wptList = Case2WptList();
            var adder = new SidAdder("AXYZ",
                                     new SidCollection(
                                                    CreateList(new SidEntry("18",
                                                                            "SID1",
                                                                            CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                                       new Waypoint("WPT102", 25.0150, 50.0800)),
                                                                            EntryType.RwySpecific,
                                                                            true))),
                                     wptList,
                                     wptList.GetEditor(),
                                     GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            var distance = GetTotalDistance(CreateList(new LatLon(25.0003, 50.0001),
                                                       new LatLon(25.0125, 50.0300),
                                                       new LatLon(25.0150, 50.0800)));

            // Check the SID3 has been added with correct total distance.
            Assert.AreEqual(2, wptList.EdgesFromCount(rwyIndex));

            // Check the edges of last wpt 

            foreach (var i in wptList.EdgesFrom(rwyIndex))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("SID1", edge.Value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(distance + new LatLon(25.0150, 50.0800).Distance(wptList[edge.ToNodeIndex].LatLon),
                                                     edge.Value.Distance,
                                                     0.1));
            }
        }

        [TestMethod]
        public void AddToWptListCase3()
        {
            var wptList = Case3WptList();
            var adder = new SidAdder("AXYZ",
                                     new SidCollection(
                                                    CreateList(new SidEntry("18",
                                                                            "SID1",
                                                                            CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                                       new Waypoint("WPT102", 25.0150, 50.0800),
                                                                                       new Waypoint("WPT103", 25.0175, 50.1300),
                                                                                       new Waypoint("WPT104", 25.0225, 50.1800)),
                                                                            EntryType.RwySpecific,
                                                                            false))),
                                     wptList,
                                     wptList.GetEditor(),
                                     GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesFromCount(rwyIndex) > 0);
            Assert.IsTrue(sidIsAdded(rwyIndex,
                                     "SID1",
                                     GetTotalDistance(CreateList(new Waypoint("AXYZ18", 25.0003, 50.0001),
                                                                 new Waypoint("WPT101", 25.0125, 50.0300),
                                                                 new Waypoint("WPT102", 25.0150, 50.0800),
                                                                 new Waypoint("WPT103", 25.0175, 50.1300),
                                                                 new Waypoint("WPT104", 25.0225, 50.1800))),
                                     wptList));

            // Check the edges of last wpt 
            int index = wptList.FindByWaypoint("WPT104", 25.0225, 50.1800);
            Assert.AreEqual(2, wptList.EdgesFromCount(index));

            foreach (var i in wptList.EdgesFrom(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(new LatLon(25.0225, 50.1800).Distance(wptList[edge.ToNodeIndex].LatLon),
                                                     edge.Value.Distance,
                                                     0.1));
            }
        }

        private WaypointList Case3WptList()
        {
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("WPT104", 25.0225, 50.1800));

            int index1 = wptList.AddWaypoint(new Waypoint("25N050E", 25.0, 50.0));
            int index2 = wptList.AddWaypoint(new Waypoint("27N050E", 27.0, 50.0));
            wptList.AddNeighbor(index1, index2, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));
            wptList.AddNeighbor(index2, index1, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));

            return wptList;
        }

        private WaypointList Case5WptList()
        {
            var wptList = new WaypointList();
            int index1 = wptList.AddWaypoint(new Waypoint("25N050E", 25.0, 50.0));
            int index2 = wptList.AddWaypoint(new Waypoint("27N050E", 27.0, 50.0));
            wptList.AddNeighbor(index1, index2, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));
            wptList.AddNeighbor(index2, index1, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));

            return wptList;
        }

        [TestMethod]
        public void AddToWptListCase5()
        {
            var wptList = Case5WptList();
            var adder = new SidAdder("AXYZ",
                                     new SidCollection(
                                                    CreateList(new SidEntry("18",
                                                                            "SID1",
                                                                            CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                                       new Waypoint("WPT102", 25.0150, 50.0800),
                                                                                       new Waypoint("WPT103", 25.0175, 50.1300),
                                                                                       new Waypoint("WPT104", 25.0225, 50.1800)),
                                                                            EntryType.RwySpecific,
                                                                            false))),
                                     wptList,
                                     wptList.GetEditor(),
                                     GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesFromCount(rwyIndex) > 0);
            Assert.IsTrue(sidIsAdded(rwyIndex,
                                     "SID1",
                                     GetTotalDistance(CreateList(new Waypoint("AXYZ18", 25.0003, 50.0001),
                                                                 new Waypoint("WPT101", 25.0125, 50.0300),
                                                                 new Waypoint("WPT102", 25.0150, 50.0800),
                                                                 new Waypoint("WPT103", 25.0175, 50.1300),
                                                                 new Waypoint("WPT104", 25.0225, 50.1800))),
                                     wptList));

            // Check the edges of last wpt 
            int index = wptList.FindByWaypoint("WPT104", 25.0225, 50.1800);

            Assert.IsTrue(index >= 0);
            Assert.AreEqual(2, wptList.EdgesFromCount(index));

            foreach (var i in wptList.EdgesFrom(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(new LatLon(25.0225, 50.1800).Distance(wptList[edge.ToNodeIndex].LatLon),
                                                     edge.Value.Distance,
                                                     0.1));
            }
        }

        [TestMethod]
        public void AddToWptListCase4()
        {
            var wptList = Case4WptList();
            var adder = new SidAdder("AXYZ",
                                     new SidCollection(
                                                    CreateList(new SidEntry("18",
                                                                            "SID1",
                                                                            CreateList(new Waypoint("WPT01", 25.0, 50.0),
                                                                                       new Waypoint("26N050E", 26.0, 50.0)),
                                                                            EntryType.RwySpecific,
                                                                            false))),
                                     wptList,
                                     wptList.GetEditor(),
                                     GetAirportManager());

            int rwyIndex = adder.AddSidsToWptList("18", CreateList("SID1"));

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesFromCount(rwyIndex) > 0);
            Assert.IsTrue(sidIsAdded(rwyIndex,
                                     "SID1",
                                     GetTotalDistance(CreateList(new Waypoint("AXYZ18", 25.0003, 50.0001),
                                                                 new Waypoint("WPT01", 25.0, 50.0),
                                                                 new Waypoint("26N050E", 26.0, 50.0))),
                                     wptList));
        }

        private WaypointList Case4WptList()
        {
            var wptList = new WaypointList();
            int index = wptList.AddWaypoint(new Waypoint("26N050E", 26.0, 50.0));
            int indexNeighbor = wptList.AddWaypoint(new Waypoint("27N050E", 27.0, 50.0));

            wptList.AddNeighbor(index, indexNeighbor, new Neighbor("AIRWAY1", wptList.Distance(index, indexNeighbor)));
            return wptList;
        }

        private AirportManager GetAirportManager()
        {
            var airportDB = new AirportCollection();
            airportDB.Add(new Airport("AXYZ",
                                      "Test Airport 01",
                                      25.0,
                                      50.0,
                                      15,
                                      5000,
                                      8000,
                                      3500,
                                      CreateList(new RwyData("18", "180", 3500, 60, false, "0.000", "0", 25.0003, 50.0001, 15, 3.00, 50, 1, 0))));

            return new AirportManager(airportDB);
        }

        private bool sidIsAdded(int rwyIndex, string name, double dis, WaypointList wptList)
        {
            foreach (var i in wptList.EdgesFrom(rwyIndex))
            {
                var edge = wptList.GetEdge(i);

                if (edge.Value.Airway == name && WithinPrecisionPercent(dis, edge.Value.Distance, 0.0001))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
