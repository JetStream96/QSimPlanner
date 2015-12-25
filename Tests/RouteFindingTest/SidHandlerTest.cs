using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP;
using QSP.AviationTools;
using QSP.RouteFinding;
using QSP.RouteFinding.AirwayStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.RouteFindingTest.TestDataGenerators;
using static QSP.MathTools.MathTools;
using static QSP.RouteFinding.Utilities;
using static Tests.Common.Utilities;
using QSP.RouteFinding.TerminalProcedures.Sid;

namespace Tests.RouteFindingTest
{
    [TestClass]
    public class SidHandlerTest
    {
        #region Test Setup

        private static SidHandler handler;
        private static WaypointList WptList;

        public static void SetWptList()
        {
            if (WptList == null)
            {
                WptList = new WptListGenerator().Generate();
            }
        }

        public static SidHandler GetHandlerAXYZ()
        {
            SetWptList();

            if (handler == null)
            {
                handler = new SidHandler("AXYZ", "RouteFindingTest\\TestData", WptList, AirportManagerGenerator.AirportList);
            }
            return handler;
        }

        #endregion

        #region Testing GetSidList

        [TestMethod]
        public void GetSidListWithTransitionTest1()
        {
            var manager = GetHandlerAXYZ();
            var sids = manager.GetSidList("18");

            Assert.AreEqual(7, sids.Count);

            Assert.IsTrue(sids.Contains("SID1"));
            Assert.IsTrue(sids.Contains("SID2"));
            Assert.IsTrue(sids.Contains("SID3"));
            Assert.IsTrue(sids.Contains("SID4"));
            Assert.IsTrue(sids.Contains("SID5.TRANS1"));
            Assert.IsTrue(sids.Contains("SID5.TRANS2"));
            Assert.IsTrue(sids.Contains("SID6"));
        }

        [TestMethod]
        public void GetSidListWithTransitionTest2()
        {
            var manager = GetHandlerAXYZ();
            var sids = manager.GetSidList("36");

            Assert.AreEqual(2, sids.Count);

            Assert.IsTrue(sids.Contains("SID5.TRANS1"));
            Assert.IsTrue(sids.Contains("SID5.TRANS2"));
        }

        [TestMethod]
        public void GetSidListNoSidAvail()
        {
            var manager = GetHandlerAXYZ();
            var sids = manager.GetSidList("03");

            Assert.AreEqual(0, sids.Count);
        }

        [TestMethod]
        public void GetSidListWrongInputRwy()
        {
            var manager = GetHandlerAXYZ();
            var sids = manager.GetSidList("25");

            Assert.AreEqual(0, sids.Count);
        }

        #endregion

        #region Testing AnalysisInfo

        [TestMethod]
        public void AnalysisInfoNoTransition()
        {
            var manager = GetHandlerAXYZ();
            var info = manager.InfoForAnalysis("18", "SID3");

            var latLons = new List<LatLon>();
            latLons.Add(new LatLon(25.0003, 50.0001));  // Rwy 18 
            latLons.Add(new LatLon(24.9, 50.0));
            latLons.Add(new LatLon(24.8, 50.0));
            latLons.Add(new LatLon(24.7, 50.0));
            latLons.Add(new LatLon(24.6, 50.0));

            double dis = GetTotalDistance(latLons);

            Assert.IsTrue(WithinPrecisionPercent(info.Item1, dis, 0.1));
            Assert.IsTrue(info.Item2.Equals(new Waypoint("WPT304", 24.6, 50.0)));
        }

        [TestMethod]
        public void AnalysisInfoNoWptInSid()
        {
            var manager = GetHandlerAXYZ();
            var info = manager.InfoForAnalysis("18", "SID6");

            Assert.IsTrue(WithinPrecisionPercent(info.Item1, 0.0, 0.1));
            Assert.IsTrue(info.Item2.Equals(new Waypoint("AXYZ18", 25.0003, 50.0001)));  // Rwy 18 
        }

        [TestMethod]
        public void AnalysisInfoWithTransition()
        {
            var manager = GetHandlerAXYZ();
            var info = manager.InfoForAnalysis("18", "SID5.TRANS1");

            var latLons = new List<LatLon>();
            latLons.Add(new LatLon(25.0003, 50.0001));  // Rwy 18 
            latLons.Add(new LatLon(24.7, 49.7));
            latLons.Add(new LatLon(24.2, 49.2));
            latLons.Add(new LatLon(24.0, 49.0));
            latLons.Add(new LatLon(23.0, 49.0));
            latLons.Add(new LatLon(22.0, 49.0));

            double dis = GetTotalDistance(latLons);

            Assert.IsTrue(WithinPrecisionPercent(info.Item1, dis, 0.1));
            Assert.IsTrue(info.Item2.Equals(new Waypoint("N22E049", 22.0, 49.0)));
        }

        #endregion

        #region Testing AddSidsToWptList

        [TestMethod]
        public void AddToWptList_NoSid_Case1()
        {
            // Case 1

            var manager = GetHandlerAXYZ();
            int rwyIndex = manager.AddSidsToWptList("03", new List<string>());

            // Check the SID is added as an edge
            Assert.IsTrue(WptList.EdgesFromCount(rwyIndex) > 0);

            foreach (var j in WptList.EdgesFrom(rwyIndex))
            {
                var edge = WptList.GetEdge(j);

                // Name is DCT 
                Assert.AreEqual("DCT", edge.value.Airway);

                // Distance is correct
                Assert.IsTrue(WithinPrecisionPercent(edge.value.Distance, WptList.Distance(rwyIndex, edge.ToNodeIndex), 0.1));
            }
        }

        private double sid3_Dis()
        {
            var latLons = new List<LatLon>();
            latLons.Add(new LatLon(25.0003, 50.0001));  // Rwy 18 
            latLons.Add(new LatLon(24.9, 50.0));
            latLons.Add(new LatLon(24.8, 50.0));
            latLons.Add(new LatLon(24.7, 50.0));
            latLons.Add(new LatLon(24.6, 50.0));

            return GetTotalDistance(latLons);
        }

        [TestMethod]
        public void AddToWptList_Case2()
        {
            // Case 2

            var manager = GetHandlerAXYZ();
            var sids = new List<string>();
            sids.Add("SID3");

            // distance           
            double dis = sid3_Dis();

            int rwyIndex = manager.AddSidsToWptList("18", sids);

            // Check the SID3 has been added with correct total distance.
            Assert.IsTrue(WptList.EdgesFromCount(rwyIndex) > 0);

            // Check the edges of last wpt 

            foreach (var i in WptList.EdgesFrom(rwyIndex))
            {
                var edge = WptList.GetEdge(i);
                Assert.AreEqual("SID3", edge.value.Airway);
                Assert.IsTrue(
                    WithinPrecisionPercent(edge.value.Distance, 
                                           dis + new LatLon(24.6, 50.0).Distance(WptList[edge.ToNodeIndex].LatLon), 
                                           0.1));
            }
        }

        private double sid1_Dis()
        {
            var latLons = new List<LatLon>();
            latLons.Add(new LatLon(25.0003, 50.0001));  // Rwy 18 
            latLons.Add(new LatLon(25.0125,50.0300));
            latLons.Add(new LatLon(25.0150,50.0800));
            latLons.Add(new LatLon(25.0175,50.1300));
            latLons.Add(new LatLon(25.0225,50.1800));

            return GetTotalDistance(latLons);
        }

        [TestMethod]
        public void AddToWptList_Case3()
        {
            // Case 3


            var manager = GetHandlerAXYZ();
            var sids = new List<string>();
            sids.Add("SID1");

            // distance           
            double dis = sid1_Dis();

            int rwyIndex = manager.AddSidsToWptList("18", sids);

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(WptList.EdgesFromCount(rwyIndex) > 0);
            Assert.IsTrue(sidIsAdded(rwyIndex, "SID1", dis));

            // Check the edges of last wpt 
            int index = WptList.FindByWaypoint("WPT104", 25.0225, 50.1800);

            foreach (var i in WptList.EdgesFrom(index))
            {
                var edge = WptList.GetEdge(i);
                Assert.AreEqual("DCT", edge.value.Airway);
                Assert.IsTrue(
                    WithinPrecisionPercent(edge.value.Distance,
                                           new LatLon(25.0225, 50.1800).Distance(WptList[edge.ToNodeIndex].LatLon),
                                           0.1));
            }

        }

        private bool sidIsAdded(int rwyIndex, string name, double dis)
        {
            foreach (var i in WptList.EdgesFrom(rwyIndex))
            {
                var edge = WptList.GetEdge(i);

                if (edge.value.Airway == name && WithinPrecisionPercent(edge.value.Distance, dis, 0.1))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
    }
