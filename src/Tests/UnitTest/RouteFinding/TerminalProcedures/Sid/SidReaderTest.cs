using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System.Collections.Generic;
using System.Linq;
using QSP.LibraryExtension;
using static QSP.LibraryExtension.Lists;

namespace UnitTest.RouteFindingTest.TerminalProceduresTest.Sid
{
    [TestFixture]
    public class SidReaderTest
    {
        private static string sidAllTxt =
@"A,AXYZ,Test Airport 01,25.0,50.0,15,5000,8000,3500,0
R,18,180,3500,60,0,0.000,0,25.0003, 50.0001,15,3.00,50,1,0
R,36,360,3500,60,0,0.000,0,25.0001, 50.0005,15,3.00,50,1,0
R,03,033,2100,50,0,0.000,0,25.0004,50.0004,15,3.0,50,1,0
R,21,213,2100,50,0,0.000,0,25.0000,50.0001,15,3.0,50,1,0

SID,SID1,18,4
VA,0,53.0,2,600,0,1,210,0,0,0, 
DF,WPT101,25.0125,50.0300,0, ,0.0,0.0,0,0,0,1,210,0,0,1, 
DF,WPT102,25.0150,50.0800,1, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT103,25.0175,50.1300,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT104,25.0225,50.1800,0, ,0.0,0.0,0.0,0.0,2,4000,0,0,0,0,0,0,

SID,SID2,18,5
VA,0,53.0,2,600,0,1,210,0,0,0, 
TF,WPT201,25.1,50.1,0, ,0.0,0.0,0,0,0,1,210,0,0,1, 
TF,WPT202,25.2,50.2,0, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT203,25.3,50.3,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT204,25.4,50.4,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
TF,N26E051,26.0,51.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,

SID,SID3,18,5
VA,0,53.0,2,600,0,1,210,0,0,0, 
DF,WPT301,24.9,50.0,0, ,0.0,0.0,0,0,0,1,210,0,0,1, 
DF,WPT302,24.8,50.0,1, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT303,24.7,50.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT304,24.6,50.0,0, ,0.0,0.0,0.0,0.0,2,4000,0,0,0,0,0,0,
VA,0,53.0,2,600,0,1,210,0,0,0, 

SID,SID4,18,6
VA,0,53.0,2,600,0,1,210,0,0,0, 
TF,WPT401,25.0,49.9,0, ,0.0,0.0,0,0,0,1,210,0,0,1, 
TF,WPT402,25.0,49.8,0, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT403,25.0,49.7,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT404,25.0,49.6,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
TF,N25E049,25.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
VA,0,53.0,2,600,0,1,210,0,0,0, 

SID,SID6,18,0
VA,0,53.0,2,600,0,1,210,0,0,0, 

SID,SID5,18,3
VA,0,53.0,2,600,0,1,210,0,0,0, 
TF,WPT5001,24.7,49.7,0, ,0.0,0.0,0,0,0,1,210,0,0,1, 
TF,WPT5002,24.2,49.2,0, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,N24E049,24.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 

SID,SID5,36,3
VA,0,53.0,2,600,0,1,210,0,0,0, 
TF,WPT5101,24.8,49.3,0, ,0.0,0.0,0,0,0,1,210,0,0,1, 
TF,WPT5102,24.5,49.1,0, ,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,N24E049,24.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,

SID,SID5,ALL,2
TF,N24E049,24.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
TF,N23E049,23.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,

SID,SID5,TRANS1,2
TF,N23E049,23.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
TF,N22E049,22.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,

SID,SID5,TRANS2,2
TF,N23E049,23.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
TF,N24E049,24.0,49.0,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,";

        private static SidCollection SidCollection;

        public static SidCollection GetSidCollection()
        {
            if (SidCollection == null)
            {
                SidCollection = SidReader.Parse(sidAllTxt.Lines());
            }

            return SidCollection;
        }

        [Test]
        public void SidContainsOnlyVector()
        {
            Assert.IsTrue(
                ContainResult(
                    GetSidCollection(), "SID6", "18", new List<Waypoint>(),
                    EntryType.RwySpecific, true));
        }

        [Test]
        public void SidIsRwySpecificNotEndingWithVector()
        {
            Assert.IsTrue(
                ContainResult(GetSidCollection(), "SID1", "18", sid1_Wpts(),
                EntryType.RwySpecific, false));
        }

        [Test]
        public void SidRwySpecificEndsWithVector()
        {
            Assert.IsTrue(
                ContainResult(GetSidCollection(), "SID3", "18", sid3_Wpts(),
                EntryType.RwySpecific, true));
        }

        [Test]
        public void SidCommonPart()
        {
            Assert.IsTrue(
                containResult_Common(GetSidCollection(), "SID5", sid5_Wpts(),
                false));
        }

        [Test]
        public void SidTransitionPart()
        {
            Assert.IsTrue(
                ContainResult(GetSidCollection(), "SID5", "TRANS2",
                sid5_Trans2_Wpts(), EntryType.Transition, false));
        }

        private List<Waypoint> sid5_Trans2_Wpts()
        {
            return CreateList(
                new Waypoint("N23E049", 23.0, 49.0),
                new Waypoint("N24E049", 24.0, 49.0));
        }

        private List<Waypoint> sid5_Wpts()
        {
            return CreateList(
                new Waypoint("N24E049", 24.0, 49.0),
                new Waypoint("N23E049", 23.0, 49.0));
        }

        private List<Waypoint> sid3_Wpts()
        {
            return CreateList(
                new Waypoint("WPT301", 24.9, 50.0),
                new Waypoint("WPT302", 24.8, 50.0),
                new Waypoint("WPT303", 24.7, 50.0),
                new Waypoint("WPT304", 24.6, 50.0));
        }

        private List<Waypoint> sid1_Wpts()
        {
            return CreateList(
                new Waypoint("WPT101", 25.0125, 50.0300),
                new Waypoint("WPT102", 25.0150, 50.0800),
                new Waypoint("WPT103", 25.0175, 50.1300),
                new Waypoint("WPT104", 25.0225, 50.1800));
        }

        private bool containResult_Common(
            SidCollection collection,
            string sid,
            List<Waypoint> wpts,
            bool endWithVector)
        {
            return collection.SidList.Any(i =>
                i.Name == sid &&
                i.Waypoints.SequenceEqual(wpts) &&
                i.Type == EntryType.Common &&
                i.EndWithVector == endWithVector);
        }

        private bool ContainResult(
            SidCollection collection, string sid, string rwy,
            List<Waypoint> wpts, EntryType type, bool endWithVector)
        {
            return collection.SidList.Any(i =>
                i.Name == sid &&
                i.RunwayOrTransition == rwy &&
                i.Waypoints.SequenceEqual(wpts) &&
                i.Type == type &&
                i.EndWithVector == endWithVector);
        }
    }
}
