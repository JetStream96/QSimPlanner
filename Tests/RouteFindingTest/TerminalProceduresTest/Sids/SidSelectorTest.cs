using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP;
using QSP.RouteFinding.TerminalProcedures;
using static QSP.RouteFinding.Utilities;
using static Tests.Common.Utilities;

namespace Tests.RouteFindingTest.TerminalProceduresTest.Sids
{
    [TestClass]
    public class SidSelectorTest
    {
        [TestMethod]
        public void WhenThereIsNoSidThenReturnEmptyList()
        {
            var avaliableSids = new SidSelector(new List<SidEntry>(), "14").GetSidList();
            Assert.AreEqual(0, avaliableSids.Count);
        }

        [TestMethod]
        public void WhenSidIsRwySpecific()
        {
            var sids = new List<SidEntry>();
            sids.Add(new SidEntry("14", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));
            sids.Add(new SidEntry("25", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));

            var avaliableSids = new SidSelector(sids, "14").GetSidList();
            Assert.AreEqual(1, avaliableSids.Count);
            Assert.IsTrue(avaliableSids[0] == "SID1");
        }

        [TestMethod]
        public void WhenCommonPartExists()
        {
            var sids = new List<SidEntry>();
            sids.Add(new SidEntry("14", "SID1", new List<Waypoint>(), EntryType.Common, false));

            var avaliableSids = new SidSelector(sids, "14").GetSidList();
            Assert.AreEqual(1, avaliableSids.Count);
            Assert.IsTrue(avaliableSids[0] == "SID1");
        }

        [TestMethod]
        public void WhenCommonPartIsNotForTheInterestedRwy()
        {
            var sids = new List<SidEntry>();
            sids.Add(new SidEntry("ALL", "SID1", new List<Waypoint>(), EntryType.Common, false));
            sids.Add(new SidEntry("25", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));
            sids.Add(new SidEntry("14", "SID2", new List<Waypoint>(), EntryType.RwySpecific, false));

            var avaliableSids = new SidSelector(sids, "14").GetSidList();
            Assert.AreEqual(1, avaliableSids.Count);
            Assert.IsTrue(avaliableSids[0] == "SID2");
        }

        [TestMethod]
        public void WhenTransitionIsAvailableThenNoTransitionPartIsIgnored()
        {
            var sids = new List<SidEntry>();
            sids.Add(new SidEntry("14", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));
            sids.Add(new SidEntry("TRANS1", "SID1", new List<Waypoint>(), EntryType.Transition, false));
            sids.Add(new SidEntry("TRANS2", "SID1", new List<Waypoint>(), EntryType.Transition, false));

            var avaliableSids = new SidSelector(sids, "14").GetSidList();
            Assert.AreEqual(2, avaliableSids.Count);
            Assert.IsTrue(avaliableSids.Contains("SID1.TRANS1"));
            Assert.IsTrue(avaliableSids.Contains("SID1.TRANS2"));
        }

        [TestMethod]
        public void WhenTransitionIsForWrongRwyThenDoesNotAddToResult()
        {
            var sids = new List<SidEntry>();
            sids.Add(new SidEntry("25", "SID2", new List<Waypoint>(), EntryType.RwySpecific, false));
            sids.Add(new SidEntry("TRANS2", "SID2", new List<Waypoint>(), EntryType.Transition, false));

            var avaliableSids = new SidSelector(sids, "14").GetSidList();
            Assert.AreEqual(0, avaliableSids.Count);
        }

    }
}
