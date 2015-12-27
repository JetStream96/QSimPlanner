using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System.IO;
using Tests.RouteFindingTest.TestDataGenerators;
using QSP.RouteFinding.TerminalProcedures;
using QSP;
using System.Collections.Generic;
using System.Linq;

namespace Tests.RouteFindingTest.TerminalProceduresTest.Sids
{
    [TestClass]
    public class SidReaderTest
    {
        [TestMethod]
        public void ParseTest()
        {
            var allTxt = CommentLineRemover.RemoveComments(File.ReadAllText("RouteFindingTest\\TestData\\PROC\\AXYZ_WithComments.txt"));
            var reader = new SidReader(allTxt);

            var sids = reader.Parse();

            Assert.IsTrue(containResult(sids, "SID6", "18", new List<Waypoint>(), EntryType.RwySpecific, true));
            Assert.IsTrue(containResult(sids, "SID1", "18", sid1_Wpts(), EntryType.RwySpecific, false));
            Assert.IsTrue(containResult(sids, "SID3", "18", sid3_Wpts(), EntryType.RwySpecific, true));
            Assert.IsTrue(containResult_Common(sids, "SID5", sid5_Wpts(), false));
            Assert.IsTrue(containResult(sids, "SID5", "TRANS2", sid5_Trans2_Wpts(), EntryType.Transition, false));
        }

        private List<Waypoint> sid5_Trans2_Wpts()
        {
            var result = new List<Waypoint>();

            result.Add(new Waypoint("N23E049", 23.0, 49.0));
            result.Add(new Waypoint("N24E049", 24.0, 49.0));

            return result;
        }

        private List<Waypoint> sid5_Wpts()
        {
            var result = new List<Waypoint>();

            result.Add(new Waypoint("N24E049", 24.0, 49.0));
            result.Add(new Waypoint("N23E049", 23.0, 49.0));

            return result;
        }

        private List<Waypoint> sid3_Wpts()
        {
            var result = new List<Waypoint>();

            result.Add(new Waypoint("WPT301", 24.9, 50.0));
            result.Add(new Waypoint("WPT302", 24.8, 50.0));
            result.Add(new Waypoint("WPT303", 24.7, 50.0));
            result.Add(new Waypoint("WPT304", 24.6, 50.0));

            return result;
        }

        private List<Waypoint> sid1_Wpts()
        {
            var result = new List<Waypoint>();

            result.Add(new Waypoint("WPT101", 25.0125, 50.0300));
            result.Add(new Waypoint("WPT102", 25.0150, 50.0800));
            result.Add(new Waypoint("WPT103", 25.0175, 50.1300));
            result.Add(new Waypoint("WPT104", 25.0225, 50.1800));

            return result;
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
