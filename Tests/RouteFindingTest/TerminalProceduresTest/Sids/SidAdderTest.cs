using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools;
using System.Collections.Generic;
using static QSP.RouteFinding.Utilities;
using static Tests.Common.Utilities;
using QSP.RouteFinding.TerminalProcedures.Sid;
using static Tests.RouteFindingTest.TestDataGenerators.TestDataProvider;
using Tests.RouteFindingTest.TestDataGenerators;

namespace Tests.RouteFindingTest.TerminalProceduresTest.Sids
{
    [TestClass]
    public class SidAdderTest
    {
        #region Test Setup

        private static SidAdder adder;

        public static SidAdder GetSidAdder()
        {
            if (adder == null)
            {
                adder = new SidAdder("AXYZ", GetSidCollection(), GetWptList(), AirportManagerGenerator.AirportList);
            }
            return adder;
        }

        #endregion

        [TestMethod]
        public void AddToWptList_NoSid_Case1()
        {
            var adder = GetSidAdder();
            int rwyIndex = adder.AddSidsToWptList("03", new List<string>());

            // Check the SID is added as an edge
            Assert.IsTrue(GetWptList().EdgesFromCount(rwyIndex) > 0);

            foreach (var j in GetWptList().EdgesFrom(rwyIndex))
            {
                var edge = GetWptList().GetEdge(j);

                // Name is DCT 
                Assert.AreEqual("DCT", edge.value.Airway);

                // Distance is correct
                Assert.IsTrue(WithinPrecisionPercent(GetWptList().Distance(rwyIndex, edge.ToNodeIndex), edge.value.Distance, 0.1));
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
            var adder = GetSidAdder();
            var sids = new List<string>();
            sids.Add("SID3");

            // distance           
            double dis = sid3_Dis();

            int rwyIndex = adder.AddSidsToWptList("18", sids);

            // Check the SID3 has been added with correct total distance.
            Assert.IsTrue(GetWptList().EdgesFromCount(rwyIndex) > 0);

            // Check the edges of last wpt 

            foreach (var i in GetWptList().EdgesFrom(rwyIndex))
            {
                var edge = GetWptList().GetEdge(i);
                Assert.AreEqual("SID3", edge.value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(dis + new LatLon(24.6, 50.0).Distance(GetWptList()[edge.ToNodeIndex].LatLon),
                                                     edge.value.Distance,
                                                     0.1));
            }
        }

        private double sid1_Dis()
        {
            var latLons = new List<LatLon>();
            latLons.Add(new LatLon(25.0003, 50.0001));  // Rwy 18 
            latLons.Add(new LatLon(25.0125, 50.0300));
            latLons.Add(new LatLon(25.0150, 50.0800));
            latLons.Add(new LatLon(25.0175, 50.1300));
            latLons.Add(new LatLon(25.0225, 50.1800));

            return GetTotalDistance(latLons);
        }

        [TestMethod]
        public void AddToWptList_Case3()
        {
            var adder = GetSidAdder();
            var sids = new List<string>();
            sids.Add("SID1");

            // distance           
            double dis = sid1_Dis();

            int rwyIndex = adder.AddSidsToWptList("18", sids);

            // Check the SID1 has been added with correct total distance.
            Assert.IsTrue(GetWptList().EdgesFromCount(rwyIndex) > 0);
            Assert.IsTrue(sidIsAdded(rwyIndex, "SID1", dis));

            // Check the edges of last wpt 
            int index = GetWptList().FindByWaypoint("WPT104", 25.0225, 50.1800);

            foreach (var i in GetWptList().EdgesFrom(index))
            {
                var edge = GetWptList().GetEdge(i);
                Assert.AreEqual("DCT", edge.value.Airway);
                Assert.IsTrue(WithinPrecisionPercent(new LatLon(25.0225, 50.1800).Distance(GetWptList()[edge.ToNodeIndex].LatLon),
                                                     edge.value.Distance,
                                                     0.1));
            }
        }

        private bool sidIsAdded(int rwyIndex, string name, double dis)
        {
            foreach (var i in GetWptList().EdgesFrom(rwyIndex))
            {
                var edge = GetWptList().GetEdge(i);

                if (edge.value.Airway == name && WithinPrecisionPercent(dis, edge.value.Distance, 0.1))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
