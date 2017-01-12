using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Star;
using static QSP.LibraryExtension.Lists;
using static UnitTest.Common.Constants;

namespace UnitTest.RouteFinding.TerminalProcedures.Star
{
    [TestFixture]
    public class StarCollectionTest
    {
        private Waypoint runway05 = new Waypoint("ABCD05", 10.0, 19.0);

        #region OnlyRwySpecificPart

        [Test]
        public void OnlyRwySpecificPart()
        {
            var stars = new StarCollection(CreateList(RwySpecificPart()));
            var info = stars.GetStarInfo("STAR1", "05", runway05);

            Assert.AreEqual(new Waypoint("WPT01", 11.0, 20.0),
                info.FirstWaypoint);

            Assert.AreEqual(DistanceRwySpecificPart(),
                info.TotalDistance,
                DistanceEpsilon);
        }

        private double DistanceRwySpecificPart()
        {
            return CreateList(
                new Waypoint("WPT01", 11.0, 20.0),
                new Waypoint("WPT02", 10.0, 20.0),
                runway05)
                .TotalDistance();
        }

        private StarEntry RwySpecificPart()
        {
            return new StarEntry(
                "05",
                "STAR1",
                CreateList(
                    new Waypoint("WPT01", 11.0, 20.0),
                    new Waypoint("WPT02", 10.0, 20.0),
                    runway05),
                EntryType.RwySpecific);
        }

        #endregion

        #region RwySpecificAndCommonPart

        [Test]
        public void RwySpecificAndCommonPart()
        {
            var info = new StarCollection(
                CreateList(RwySpecificPart(), CommonPart()))
                .GetStarInfo("STAR1", "05", runway05);

            Assert.AreEqual(new Waypoint("WPTA", 12.0, 21.0), info.FirstWaypoint);

            Assert.AreEqual(DistanceRwySpecificAndCommonPart(),
                info.TotalDistance,
                DistanceEpsilon);
        }

        private StarEntry CommonPart()
        {
            return new StarEntry(
                "ALL",
                "STAR1",
                CreateList(
                    new Waypoint("WPTA", 12.0, 21.0),
                    new Waypoint("WPT01", 11.0, 20.0)),
                EntryType.Common);
        }

        private double DistanceRwySpecificAndCommonPart()
        {
            return CreateList(
                new Waypoint("WPTA", 12.0, 21.0),
                new Waypoint("WPT01", 11.0, 20.0),
                new Waypoint("WPT02", 10.0, 20.0),
                runway05)
                .TotalDistance();
        }

        #endregion

        #region RwySpecificAndCommonAndTransitionPart

        [Test]
        public void RwySpecificAndCommonAndTransitionPart()
        {
            var stars = new StarCollection(
                CreateList(
                    RwySpecificPart(),
                    CommonPart(),
                    TransitionPart()));

            var info = stars.GetStarInfo("STAR1.TRANS1", "05", runway05);
            Assert.AreEqual(new Waypoint("WPTBB", 12.0, 22.0), info.FirstWaypoint);

            Assert.AreEqual(DistanceRwySpecificAndCommonAndTransitionPart(),
                info.TotalDistance,
                DistanceEpsilon);
        }

        private StarEntry TransitionPart()
        {
            return new StarEntry(
                "TRANS1",
                "STAR1",
                CreateList(
                    new Waypoint("WPTBB", 12.0, 22.0),
                    new Waypoint("WPTCC", 13.0, 22.0),
                    new Waypoint("WPTA", 12.0, 21.0)),
                EntryType.Transition);
        }

        private double DistanceRwySpecificAndCommonAndTransitionPart()
        {
            return CreateList(
                new Waypoint("WPTBB", 12.0, 22.0),
                new Waypoint("WPTCC", 13.0, 22.0),
                new Waypoint("WPTA", 12.0, 21.0),
                new Waypoint("WPT01", 11.0, 20.0),
                new Waypoint("WPT02", 10.0, 20.0),
                runway05)
                .TotalDistance();
        }

        #endregion
    }
}
