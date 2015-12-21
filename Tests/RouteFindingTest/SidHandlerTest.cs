using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools;
using QSP.RouteFinding;
using QSP.RouteFinding.AirwayStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.RouteFindingTest.TestDataGenerators;
using static QSP.RouteFinding.Utilities;
using static Tests.Common.Utilities;
using QSP;
using static QSP.MathTools.MathTools;

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
            Assert.IsTrue(info.Item2.Equals(new Waypoint("WPT04", 24.6, 50.0)));
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

        private List<Waypoint> addedWpts()
        {
            // for 25,50
            var result = new List<Waypoint>();

            for (int lat = 20; lat < 30; lat++)
            {
                for (int lon = 45; lon < 55; lon++)
                {
                    if (GreatCircleDistance(lat, lon, 25, 50) < Constants.MAX_LEG_DIS)
                    {
                        result.Add(new Waypoint(LatLonConversion.To7DigitFormat(lat, lon), lat, lon));
                    }
                }
            }
            return result;
        }

        [TestMethod]
        public void AddToWptList_NoSid_Case1()
        {
            // Case 1

            var manager = GetHandlerAXYZ();
            int rwyIndex = manager.AddSidsToWptList("03", new List<string>());

            var neighborList = addedWpts();

            // Check the nearby waypoints are added
            foreach (var i in neighborList)
            {
                Assert.IsTrue(WptList.FindByWaypoint(i) >= 0);
            }

            // Check the SID is added as an edge
            Assert.AreEqual(neighborList.Count, WptList.EdgesFromCount(rwyIndex));

            foreach (var j in WptList.EdgesFrom(rwyIndex))
            {
                var edge = WptList.GetEdge(j);

                // Name is DCT 
                Assert.AreEqual("DCT", edge.value.Airway);

                // Distance is correct
                Assert.IsTrue(WithinPrecisionPercent(edge.value.Distance, WptList.Distance(rwyIndex, edge.ToNodeIndex), 0.1));
            }
        }

        [TestMethod]
        public void AddToWptList_Case2()
        {
            // Case 2

            var manager = GetHandlerAXYZ();
            var sids = new List<string>();
            sids.Add("SID3");

            manager.AddSidsToWptList("18", sids);

            // Check the nearby waypoints are added
            foreach (var i in addedWpts())
            {
                Assert.IsTrue(WptList.FindByWaypoint(i) >= 0);
            }
        }

        #endregion

    }
}
