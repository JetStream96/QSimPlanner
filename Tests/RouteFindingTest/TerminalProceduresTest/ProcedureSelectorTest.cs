using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP;
using QSP.RouteFinding.TerminalProcedures;

namespace Tests.RouteFindingTest.TerminalProceduresTest
{
    [TestClass]
    public class ProcedureSelectorTest
    {

        [TestMethod]
        public void WhenThereIsNoSidThenReturnEmptyList()
        {
            var avaliableSids = new ProcedureSelector<SidEntry>(new List<SidEntry>(), "14").GetProcedureList();
            Assert.AreEqual(0, avaliableSids.Count);
        }

        [TestMethod]
        public void WhenSidIsRwySpecific()
        {
            var sids = sidEntryCreateHelper(new sidData("14", "SID1", EntryType.RwySpecific),
                                            new sidData("25", "SID1", EntryType.RwySpecific));

            var avaliableSids = new ProcedureSelector<SidEntry>(sids, "14").GetProcedureList();
            Assert.AreEqual(1, avaliableSids.Count);
            Assert.IsTrue(avaliableSids[0] == "SID1");
        }

        [TestMethod]
        public void WhenCommonPartExists()
        {
            var sids = sidEntryCreateHelper(new sidData("14", "SID1", EntryType.Common));

            var avaliableSids = new ProcedureSelector<SidEntry>(sids, "14").GetProcedureList();
            Assert.AreEqual(1, avaliableSids.Count);
            Assert.IsTrue(avaliableSids[0] == "SID1");
        }

        [TestMethod]
        public void WhenCommonPartIsNotForTheInterestedRwy()
        {
            var sids = sidEntryCreateHelper(new sidData("ALL", "SID1", EntryType.Common),
                                            new sidData("25", "SID1", EntryType.RwySpecific),
                                            new sidData("14", "SID2", EntryType.RwySpecific));

            var avaliableSids = new ProcedureSelector<SidEntry>(sids, "14").GetProcedureList();
            Assert.AreEqual(1, avaliableSids.Count);
            Assert.IsTrue(avaliableSids[0] == "SID2");
        }

        [TestMethod]
        public void WhenTransitionIsAvailableThenNoTransitionPartIsIgnored()
        {
            var sids = sidEntryCreateHelper(new sidData("14", "SID1", EntryType.RwySpecific),
                                            new sidData("TRANS1", "SID1", EntryType.Transition),
                                            new sidData("TRANS2", "SID1", EntryType.Transition));

            var avaliableSids = new ProcedureSelector<SidEntry>(sids, "14").GetProcedureList();
            Assert.AreEqual(2, avaliableSids.Count);
            Assert.IsTrue(avaliableSids.Contains("SID1.TRANS1"));
            Assert.IsTrue(avaliableSids.Contains("SID1.TRANS2"));
        }

        [TestMethod]
        public void WhenTransitionIsForWrongRwyThenDoesNotAddToResult()
        {
            var sids = sidEntryCreateHelper(new sidData("25", "SID2", EntryType.RwySpecific),
                                            new sidData("TRANS2", "SID2", EntryType.Transition));

            var avaliableSids = new ProcedureSelector<SidEntry>(sids, "14").GetProcedureList();
            Assert.AreEqual(0, avaliableSids.Count);
        }

        private static List<SidEntry> sidEntryCreateHelper(params sidData[] data)
        {
            var emptyList = new List<Waypoint>();
            var sids = new List<SidEntry>();

            foreach (var i in data)
            {
                sids.Add(new SidEntry(i.rwy, i.sidName, emptyList, i.type, false));
            }
            return sids;
        }

        private class sidData
        {
            public string rwy;
            public string sidName;
            public EntryType type;

            public sidData(string rwy, string sidName, EntryType type)
            {
                this.rwy = rwy;
                this.sidName = sidName;
                this.type = type;
            }
        }

    }
}
