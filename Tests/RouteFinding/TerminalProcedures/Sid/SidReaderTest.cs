using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System.Collections.Generic;
using System.Linq;
using Tests.RouteFindingTest.TestDataGenerators;
using static QSP.LibraryExtension.Lists;
using QSP.RouteFinding.Containers;

namespace Tests.RouteFindingTest.TerminalProceduresTest.Sid
{
    [TestClass]
    public class SidReaderTest
    {
        [TestMethod]
        public void SidContainsOnlyVector()
        {
            Assert.IsTrue(containResult(TestDataProvider.GetSidCollection(), "SID6", "18", new List<Waypoint>(), EntryType.RwySpecific, true));
        }

        [TestMethod]
        public void SidIsRwySpecificNotEndingWithVector()
        {
            Assert.IsTrue(containResult(TestDataProvider.GetSidCollection(), "SID1", "18", sid1_Wpts(), EntryType.RwySpecific, false));
        }

        [TestMethod]
        public void SidRwySpecificEndsWithVector()
        {
            Assert.IsTrue(containResult(TestDataProvider.GetSidCollection(), "SID3", "18", sid3_Wpts(), EntryType.RwySpecific, true));
        }

        [TestMethod]
        public void SidCommonPart()
        {
            Assert.IsTrue(containResult_Common(TestDataProvider.GetSidCollection(), "SID5", sid5_Wpts(), false));
        }

        [TestMethod]
        public void SidTransitionPart()
        {
            Assert.IsTrue(containResult(TestDataProvider.GetSidCollection(), "SID5", "TRANS2", sid5_Trans2_Wpts(), EntryType.Transition, false));
        }

        private List<Waypoint> sid5_Trans2_Wpts()
        {
            return CreateList(new Waypoint("N23E049", 23.0, 49.0),
                              new Waypoint("N24E049", 24.0, 49.0));
        }

        private List<Waypoint> sid5_Wpts()
        {
            return CreateList(new Waypoint("N24E049", 24.0, 49.0),
                              new Waypoint("N23E049", 23.0, 49.0));
        }

        private List<Waypoint> sid3_Wpts()
        {
            return CreateList(new Waypoint("WPT301", 24.9, 50.0),
                              new Waypoint("WPT302", 24.8, 50.0),
                              new Waypoint("WPT303", 24.7, 50.0),
                              new Waypoint("WPT304", 24.6, 50.0));
        }

        private List<Waypoint> sid1_Wpts()
        {
            return CreateList(new Waypoint("WPT101", 25.0125, 50.0300),
                              new Waypoint("WPT102", 25.0150, 50.0800),
                              new Waypoint("WPT103", 25.0175, 50.1300),
                              new Waypoint("WPT104", 25.0225, 50.1800));
        }

        private bool containResult_Common(SidCollection collection, string sid, List<Waypoint> wpts, bool endWithVector)
        {
            foreach (var i in collection.SidList)
            {
                if (i.Name == sid && Enumerable.SequenceEqual(i.Waypoints, wpts) &&
                    i.Type == EntryType.Common && i.EndWithVector == endWithVector)
                {
                    return true;
                }
            }
            return false;
        }

        private bool containResult(SidCollection collection, string sid, string rwy, List<Waypoint> wpts, EntryType type, bool endWithVector)
        {
            foreach (var i in collection.SidList)
            {
                if (i.Name == sid && i.RunwayOrTransition == rwy && Enumerable.SequenceEqual(i.Waypoints, wpts) &&
                    i.Type == type && i.EndWithVector == endWithVector)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
