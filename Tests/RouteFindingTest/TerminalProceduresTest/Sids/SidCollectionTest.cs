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
    public class SidCollectionTest
    {
        private Waypoint runway05 = new Waypoint("ABCD05", 10.0, 19.0);

        #region OnlyRwySpecificPart

        [TestMethod]
        public void GetSidInfoTest_OnlyRwySpecificPart()
        {
            var sids = new List<SidEntry>();
            sids.Add(onlyRwySpecificPart());
            var col = new SidCollection(sids);

            var info = col.GetSidInfo("SID1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPT02", 11.0, 20.0).Equals(info.LastWaypoint));
            Assert.IsTrue(WithinPrecisionPercent(dis_OnlyRwySpecificPart(), info.TotalDistance, 0.1));
        }

        private double dis_OnlyRwySpecificPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(runway05);
            wpts.Add(new Waypoint("WPT01", 10.0, 20.0));
            wpts.Add(new Waypoint("WPT02", 11.0, 20.0));

            return GetTotalDistance(wpts);
        }

        private SidEntry onlyRwySpecificPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(new Waypoint("WPT01", 10.0, 20.0));
            wpts.Add(new Waypoint("WPT02", 11.0, 20.0));

            return new SidEntry("05", "SID1", wpts, EntryType.RwySpecific, false);
        }

        #endregion

        #region RwySpecificAndCommonPart

        [TestMethod]
        public void GetSidInfoTest_RwySpecificAndCommonPart()
        {
            var col = new SidCollection(rwySpecificAndCommonPart());

            var info = col.GetSidInfo("SID1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPTA", 12.0, 21.0).Equals(info.LastWaypoint));
            Assert.IsTrue(WithinPrecisionPercent(dis_RwySpecificAndCommonPart(), info.TotalDistance, 0.1));
        }

        private List<SidEntry> rwySpecificAndCommonPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(new Waypoint("WPT02", 11.0, 20.0));
            wpts.Add(new Waypoint("WPTA", 12.0, 21.0));

            var result = new List<SidEntry>();
            result.Add(onlyRwySpecificPart());
            result.Add(new SidEntry("", "SID1", wpts, EntryType.Common, false));
            return result;
        }

        private double dis_RwySpecificAndCommonPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(runway05);
            wpts.Add(new Waypoint("WPT01", 10.0, 20.0));
            wpts.Add(new Waypoint("WPT02", 11.0, 20.0));
            wpts.Add(new Waypoint("WPTA", 12.0, 21.0));

            return GetTotalDistance(wpts);
        }

        #endregion

        #region RwySpecificAndCommonAndTransitionPart

        [TestMethod]
        public void GetSidInfoTest_RwySpecificAndCommonAndTransitionPart()
        {
            var col = new SidCollection(rwySpecificAndCommonAndTransitionPart());

            var info = col.GetSidInfo("SID1.TRANS1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPTCC", 13.0, 22.0).Equals(info.LastWaypoint));
            Assert.IsTrue(WithinPrecisionPercent(dis_RwySpecificAndCommonAndTransitionPart(), info.TotalDistance, 0.1));
        }

        private List<SidEntry> rwySpecificAndCommonAndTransitionPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(new Waypoint("WPTA", 12.0, 21.0));
            wpts.Add(new Waypoint("WPTBB", 12.0, 22.0));
            wpts.Add(new Waypoint("WPTCC", 13.0, 22.0));

            var result = rwySpecificAndCommonPart();
            result.Add(new SidEntry("TRANS1", "SID1", wpts, EntryType.Transition, false));
            return result;
        }

        private double dis_RwySpecificAndCommonAndTransitionPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(runway05);
            wpts.Add(new Waypoint("WPT01", 10.0, 20.0));
            wpts.Add(new Waypoint("WPT02", 11.0, 20.0));
            wpts.Add(new Waypoint("WPTA", 12.0, 21.0));
            wpts.Add(new Waypoint("WPTBB", 12.0, 22.0));
            wpts.Add(new Waypoint("WPTCC", 13.0, 22.0));

            return GetTotalDistance(wpts);
        }

        #endregion

        #region Testing GetSidList

        [TestMethod]
        public void GetSidListNoTransition()
        {
            var entries = new List<SidEntry>();

            // Waypoints are not important hence an empty list is passed.
            entries.Add(new SidEntry("05", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));

            entries.Add(new SidEntry("05", "SID2", new List<Waypoint>(), EntryType.RwySpecific, false));
            entries.Add(new SidEntry("ALL", "SID2", new List<Waypoint>(), EntryType.Common, false));

            entries.Add(new SidEntry("23", "SID3", new List<Waypoint>(), EntryType.RwySpecific, false));
            entries.Add(new SidEntry("ALL", "SID3", new List<Waypoint>(), EntryType.Common, false));

            entries.Add(new SidEntry("ALL", "SID4", new List<Waypoint>(), EntryType.Common, false));

            var collection = new SidCollection(entries);

            var sids = collection.GetSidList("05");

            Assert.IsTrue(sids.Contains("SID1"));
            Assert.IsTrue(sids.Contains("SID2"));
            Assert.IsFalse(sids.Contains("SID3"));
            Assert.IsTrue(sids.Contains("SID4"));
        }

        [TestMethod]
        public void GetSidListWithTransitionTest1()
        {
            var entries = new List<SidEntry>();

            // Waypoints are not important hence an empty list is passed.
            entries.Add(new SidEntry("05", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));

            entries.Add(new SidEntry("05", "SID2", new List<Waypoint>(), EntryType.RwySpecific, false));
            entries.Add(new SidEntry("ALL", "SID2", new List<Waypoint>(), EntryType.Common, false));

            entries.Add(new SidEntry("23", "SID3", new List<Waypoint>(), EntryType.RwySpecific, false));
            entries.Add(new SidEntry("ALL", "SID3", new List<Waypoint>(), EntryType.Common, false));

            entries.Add(new SidEntry("ALL", "SID4", new List<Waypoint>(), EntryType.Common, false));

            var collection = new SidCollection(entries);

            var sids = collection.GetSidList("05");

            Assert.IsTrue(sids.Contains("SID1"));
            Assert.IsTrue(sids.Contains("SID2"));
            Assert.IsFalse(sids.Contains("SID3"));
            Assert.IsTrue(sids.Contains("SID4"));


            //var manager = GetHandlerAXYZ();
            //var sids = manager.GetSidList("18");

            //Assert.AreEqual(7, sids.Count);

            //Assert.IsTrue(sids.Contains("SID1"));
            //Assert.IsTrue(sids.Contains("SID2"));
            //Assert.IsTrue(sids.Contains("SID3"));
            //Assert.IsTrue(sids.Contains("SID4"));
            //Assert.IsTrue(sids.Contains("SID5.TRANS1"));
            //Assert.IsTrue(sids.Contains("SID5.TRANS2"));
            //Assert.IsTrue(sids.Contains("SID6"));
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
            // Empty collection
            var collection = new SidCollection(new List<SidEntry>());
            var sids = collection.GetSidList("03");

            Assert.AreEqual(0, sids.Count);
        }

        [TestMethod]
        public void GetSidListWrongInputRwy()
        {
            var entries = new List<SidEntry>();

            // Waypoints are not important hence an empty list is passed.
            entries.Add(new SidEntry("05", "SID1", new List<Waypoint>(), EntryType.RwySpecific, false));
            entries.Add(new SidEntry("05", "SID2", new List<Waypoint>(), EntryType.RwySpecific, false));

            var collection = new SidCollection(entries);
            var sids = collection.GetSidList("23");

            Assert.AreEqual(0, sids.Count);
        }

        #endregion
    }
}
