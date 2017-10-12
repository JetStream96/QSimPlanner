using CommonLibrary.LibraryExtension;
using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.RouteFinding.TerminalProcedures.Star
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
            Waypoint[] wpts =
            {
                new Waypoint("WPT02", -50.0145, -121.015656),
                new Waypoint("WPT03", -50.0872, -121.0876)
            };

            Assert.IsTrue(ContainResult(
                GetStarCollection(),
                "STAR1",
                "05L",
                wpts,
                EntryType.RwySpecific));
        }

        [Test]
        public void StarCommonPart()
        {
            Waypoint[] wpts =
            {
                new Waypoint("WPT01", -50.00, -121.000),
                new Waypoint("WPT02", -50.0145, -121.015656)
            };

            Assert.IsTrue(ContainResultCommonPart(
                GetStarCollection(),
                "STAR1",
                wpts));
        }

        [Test]
        public void StarTransitionPart()
        {
            Waypoint[] wpts =
            {
                new Waypoint("WPT00", -49.8486, -120.516),
                new Waypoint("WPT01", -50.00, -121.000)
            };

            Assert.IsTrue(ContainResult(
                GetStarCollection(),
                "STAR1",
                "TRANS1",
                wpts,
                EntryType.Transition));
        }

        private static StarCollection GetStarCollection()
        {
            return StarReader.Parse(TxtData.Lines());
        }

        private static bool ContainResultCommonPart(StarCollection collection, string sid,
            IReadOnlyList<Waypoint> wpts)
        {
            return collection.StarList.Any(i =>
                i.Name == sid && i.Waypoints.SequenceEqual(wpts));
        }

        private static bool ContainResult(
            StarCollection collection,
            string sid,
            string rwy,
            IReadOnlyList<Waypoint> wpts,
            EntryType type)
        {
            return collection.StarList.Any(i =>
                i.Name == sid &&
                i.RunwayOrTransition == rwy &&
                i.Waypoints.SequenceEqual(wpts) &&
                i.Type == type);
        }
    }
}
