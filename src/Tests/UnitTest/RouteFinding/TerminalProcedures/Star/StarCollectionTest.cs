using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using static QSP.LibraryExtension.Lists;
using static UnitTest.Common.Utilities;
using QSP.RouteFinding.Data.Interfaces;

namespace UnitTest.RouteFindingTest.TerminalProceduresTest.Star
{
    [TestClass]
    public class StarCollectionTest
    {
        private Waypoint runway05 = new Waypoint("ABCD05", 10.0, 19.0);

        #region OnlyRwySpecificPart

        [TestMethod]
        public void GetSidInfoTest_OnlyRwySpecificPart()
        {
            var info = new StarCollection(CreateList(rwySpecificPart())).GetStarInfo("STAR1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPT01", 11.0, 20.0).Equals(info.FirstWaypoint));
            Assert.IsTrue(WithinPrecisionPercent(distanceRwySpecificPart(), info.TotalDistance, 0.001));
        }

        private double distanceRwySpecificPart()
        {
            return CreateList(new Waypoint("WPT01", 11.0, 20.0),
                              new Waypoint("WPT02", 10.0, 20.0),
                              runway05).TotalDistance();
        }

        private StarEntry rwySpecificPart()
        {
            return new StarEntry("05",
                                 "STAR1",
                                 CreateList(new Waypoint("WPT01", 11.0, 20.0),
                                            new Waypoint("WPT02", 10.0, 20.0),
                                            runway05),
                                 EntryType.RwySpecific);
        }

        #endregion

        #region RwySpecificAndCommonPart

        [TestMethod]
        public void GetSidInfoTest_RwySpecificAndCommonPart()
        {
            var info = new StarCollection(CreateList(rwySpecificPart(),
                                                     commonPart()))
                       .GetStarInfo("STAR1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPTA", 12.0, 21.0).Equals(info.FirstWaypoint));
            Assert.IsTrue(WithinPrecisionPercent(distanceRwySpecificAndCommonPart(), info.TotalDistance, 0.001));
        }

        private StarEntry commonPart()
        {
            return new StarEntry("ALL",
                                 "STAR1",
                                 CreateList(new Waypoint("WPTA", 12.0, 21.0),
                                            new Waypoint("WPT01", 11.0, 20.0)),
                                 EntryType.Common);
        }

        private double distanceRwySpecificAndCommonPart()
        {
            return CreateList(new Waypoint("WPTA", 12.0, 21.0),
                            new Waypoint("WPT01", 11.0, 20.0),
                            new Waypoint("WPT02", 10.0, 20.0),
                            runway05).TotalDistance();
        }

        #endregion

        #region RwySpecificAndCommonAndTransitionPart

        [TestMethod]
        public void GetSidInfoTest_RwySpecificAndCommonAndTransitionPart()
        {
            var info = new StarCollection(CreateList(rwySpecificPart(),
                                                     commonPart(),
                                                     transitionPart()))
                       .GetStarInfo("STAR1.TRANS1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPTBB", 12.0, 22.0).Equals(info.FirstWaypoint));
            Assert.IsTrue(WithinPrecisionPercent(distanceRwySpecificAndCommonAndTransitionPart(), info.TotalDistance, 0.001));
        }

        private StarEntry transitionPart()
        {
            return new StarEntry("TRANS1",
                                 "STAR1",
                                 CreateList(new Waypoint("WPTBB", 12.0, 22.0),
                                            new Waypoint("WPTCC", 13.0, 22.0),
                                            new Waypoint("WPTA", 12.0, 21.0)),
                                 EntryType.Transition);
        }

        private double distanceRwySpecificAndCommonAndTransitionPart()
        {
            return CreateList(new Waypoint("WPTBB", 12.0, 22.0),
                            new Waypoint("WPTCC", 13.0, 22.0),
                            new Waypoint("WPTA", 12.0, 21.0),
                            new Waypoint("WPT01", 11.0, 20.0),
                            new Waypoint("WPT02", 10.0, 20.0),
                            runway05).TotalDistance();
        }

        #endregion
    }
}
