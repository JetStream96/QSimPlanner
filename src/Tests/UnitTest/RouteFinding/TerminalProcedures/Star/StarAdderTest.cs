using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using static QSP.LibraryExtension.Lists;
using static QSP.MathTools.Utilities;
using static QSP.RouteFinding.Utilities;
using static UnitTest.Common.Utilities;

namespace UnitTest.RouteFindingTest.TerminalProceduresTest.Star
{
    [TestClass]
    public class StarAdderTest
    {


        [TestMethod]
        public void AddToWptListCase1()
        {
            var wptList = Case1WptList();
            var adder = new StarAdder("AXYZ",
                                      new StarCollection(new List<StarEntry>()),
                                      wptList,
                                      wptList.GetEditor(),
                                      GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", new List<string>());

            // Check the STAR is added 
            Assert.IsTrue(directAdded(wptList, new Waypoint("25N050E", 25.0, 50.0), rwyIndex));
            Assert.IsTrue(directAdded(wptList, new Waypoint("27N050E", 27.0, 50.0), rwyIndex));
        }

        private static bool directAdded(WaypointList wptList, Waypoint wpt, int rwyIndex)
        {
            foreach (var i in wptList.EdgesFrom(wptList.FindByWaypoint(wpt)))
            {
                var edge = wptList.GetEdge(i);
                if (edge.Value.Airway == "DCT" &&
                    WithinPrecisionPercent(GreatCircleDistance(wpt.LatLon, wptList[rwyIndex].LatLon),
                                           edge.Value.Distance,
                                           0.1))
                {
                    return true;
                }
            }
            return false;
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

        [TestMethod]
        public void AddToWptListCase2()
        {
            var wptList = Case2WptList();
            var adder = new StarAdder("AXYZ",
                                     new StarCollection(
                                                    CreateList(new StarEntry("18",
                                                                             "STAR1",
                                                                             CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                                        new Waypoint("WPT102", 25.0150, 50.0800),
                                                                                        new Waypoint("WPT103", 25.0175, 50.1300),
                                                                                        new Waypoint("WPT104", 25.0225, 50.1800)),
                                                                             EntryType.RwySpecific))),
                                     wptList,
                                     wptList.GetEditor(),
                                     GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", CreateList("STAR1"));
            int index = wptList.FindByWaypoint(new Waypoint("WPT101", 25.0125, 50.0300));

            // Check the STAR1 has been added with correct total distance.
            Assert.IsTrue(starIsAdded(wptList.FindByWaypoint(new Waypoint("WPT101", 25.0125, 50.0300)),
                                      "STAR1",
                                      GetTotalDistance(CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                  new Waypoint("WPT102", 25.0150, 50.0800),
                                                                  new Waypoint("WPT103", 25.0175, 50.1300),
                                                                  new Waypoint("WPT104", 25.0225, 50.1800),
                                                                  new Waypoint("AXYZ18", 25.0003, 50.0001))),
                                      wptList));

            // Check the edges of first wpt            
            Assert.AreEqual(2, wptList.EdgesToCount(index));

            foreach (var i in wptList.EdgesTo(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(new LatLon(25.0125, 50.0300).Distance(wptList[edge.FromNodeIndex].LatLon),
                                                     edge.Value.Distance,
                                                     0.0001));
            }
        }

        private WaypointList Case2WptList()
        {
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("WPT101", 25.0125, 50.0300));

            int index1 = wptList.AddWaypoint(new Waypoint("25N050E", 25.0, 50.0));
            int index2 = wptList.AddWaypoint(new Waypoint("27N050E", 27.0, 50.0));
            wptList.AddNeighbor(index1, index2, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));
            wptList.AddNeighbor(index2, index1, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));

            return wptList;
        }

        private WaypointList Case4WptList()
        {
            var wptList = new WaypointList();
            int index1 = wptList.AddWaypoint(new Waypoint("25N050E", 25.0, 50.0));
            int index2 = wptList.AddWaypoint(new Waypoint("27N050E", 27.0, 50.0));
            wptList.AddNeighbor(index1, index2, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));
            wptList.AddNeighbor(index2, index1, new Neighbor("AIRWAY1", wptList.Distance(index1, index2)));

            return wptList;
        }

        [TestMethod]
        public void AddToWptListCase4()
        {
            var wptList = Case4WptList();
            var adder = new StarAdder("AXYZ",
                                      new StarCollection(
                                                    CreateList(new StarEntry("18",
                                                                             "STAR1",
                                                                             CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                                        new Waypoint("WPT102", 25.0150, 50.0800),
                                                                                        new Waypoint("WPT103", 25.0175, 50.1300),
                                                                                        new Waypoint("WPT104", 25.0225, 50.1800)),
                                                                             EntryType.RwySpecific))),
                                      wptList,
                                      wptList.GetEditor(),
                                      GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", CreateList("STAR1"));

            // Check the STAR1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesToCount(rwyIndex) > 0);
            Assert.IsTrue(starIsAdded(wptList.FindByWaypoint(new Waypoint("WPT101", 25.0125, 50.0300)),
                                      "STAR1",
                                      GetTotalDistance(CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                                                                  new Waypoint("WPT102", 25.0150, 50.0800),
                                                                  new Waypoint("WPT103", 25.0175, 50.1300),
                                                                  new Waypoint("WPT104", 25.0225, 50.1800),
                                                                  new Waypoint("AXYZ18", 25.0003, 50.0001))),
                                      wptList));

            // Check the edges of first wpt 
            int index = wptList.FindByWaypoint(new Waypoint("WPT101", 25.0125, 50.0300));

            Assert.IsTrue(index >= 0);
            Assert.AreEqual(2, wptList.EdgesToCount(index));

            foreach (var i in wptList.EdgesTo(index))
            {
                var edge = wptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.Value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(new LatLon(25.0125, 50.0300).Distance(wptList[edge.FromNodeIndex].LatLon),
                                                     edge.Value.Distance,
                                                     0.1));
            }
        }

        [TestMethod]
        public void AddToWptListCase3()
        {
            var wptList = Case4WptList();
            var adder = new StarAdder("AXYZ",
                                      new StarCollection(
                                                    CreateList(new StarEntry("18",
                                                                             "STAR1",
                                                                             CreateList(new Waypoint("26N050E", 26.0, 50.0),
                                                                                        new Waypoint("WPT01", 25.0, 50.0)),
                                                                             EntryType.RwySpecific))),
                                      wptList,
                                      wptList.GetEditor(),
                                      GetAirportManager());

            int rwyIndex = adder.AddStarsToWptList("18", CreateList("STAR1"));

            // Check the STAR1 has been added with correct total distance.
            Assert.IsTrue(wptList.EdgesToCount(rwyIndex) == 1);
            Assert.IsTrue(starIsAdded(wptList.FindByWaypoint(new Waypoint("26N050E", 26.0, 50.0)),
                                     "STAR1",
                                     GetTotalDistance(CreateList(new Waypoint("26N050E", 26.0, 50.0),
                                                                 new Waypoint("WPT01", 25.0, 50.0),
                                                                 new Waypoint("AXYZ18", 25.0003, 50.0001))),
                                     wptList));
        }

        private WaypointList Case3WptList()
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
                                      true,
                                      5000,
                                      8000,
                                      3500,
                                      CreateList(new RwyData("18", "180", 3500, 60, true, false, "0.000", "0", 25.0003, 50.0001, 15, 3.00, 50, 1, 0))));

            return new AirportManager(airportDB);
        }

        private bool starIsAdded(int fromIndex, string name, double dis, WaypointList wptList)
        {
            foreach (var i in wptList.EdgesFrom(fromIndex))
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
