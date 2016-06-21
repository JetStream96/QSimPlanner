using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Lists;

namespace UnitTest.RouteFindingTest.TerminalProceduresTest.Star
{
    [TestFixture]
    public class StarReaderTest
    {
        private static string TxtData = @"STAR,STAR1,ALL,2
IF,WPT01,-50.00,-121.000,0.0,0.0,0,0,0,0,0,0,0,0, 
TF,WPT02,-50.0145,-121.015656,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0, 

STAR,STAR1,05L,2
TF,WPT02,-50.0145,-121.015656,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
IF,WPT03,-50.0872,-121.0876,0.0,0.0,0,0,0,0,0,0,0,0, 

STAR,STAR1,TRANS1,2
TF,WPT00,-49.8486,-120.516,0, ,0.0,0.0,0.0,0.0,0,0,0,0,0,0,0,0,
IF,WPT01,-50.00,-121.000,0.0,0.0,0,0,0,0,0,0,0,0, 
";

        [Test]
        public void StarRwySpecific()
        {
            Assert.IsTrue(containResult(
                GetStarCollection(),
                    "STAR1",
                    "05L",
                    CreateList(
                        new Waypoint("WPT02", -50.0145, -121.015656),
                        new Waypoint("WPT03", -50.0872, -121.0876)),
                    EntryType.RwySpecific));
        }

        [Test]
        public void StarCommonPart()
        {
            Assert.IsTrue(containResultCommonPart(
                GetStarCollection(),
                "STAR1",
                CreateList(
                    new Waypoint("WPT01", -50.00, -121.000),
                    new Waypoint("WPT02", -50.0145, -121.015656))));
        }

        [Test]
        public void StarTransitionPart()
        {
            Assert.IsTrue(containResult(
                GetStarCollection(),
                "STAR1",
                "TRANS1",
                CreateList(
                    new Waypoint("WPT00", -49.8486, -120.516),
                    new Waypoint("WPT01", -50.00, -121.000)),
                EntryType.Transition));
        }

        private static StarCollection GetStarCollection()
        {
            return new StarReader(TxtData).Parse();
        }

        private static bool containResultCommonPart(
            StarCollection collection, string sid, List<Waypoint> wpts)
        {
            return collection.StarList.Any(i =>
            i.Name == sid && Enumerable.SequenceEqual(i.Waypoints, wpts));
        }

        private static bool containResult(
            StarCollection collection,
            string sid,
            string rwy,
            List<Waypoint> wpts,
            EntryType type)
        {
            return collection.StarList.Any(i =>
            i.Name == sid &&
            i.RunwayOrTransition == rwy &&
            Enumerable.SequenceEqual(i.Waypoints, wpts) &&
            i.Type == type);
        }
    }
}
