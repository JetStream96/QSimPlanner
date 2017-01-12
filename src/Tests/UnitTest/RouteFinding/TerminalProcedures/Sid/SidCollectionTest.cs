using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using static QSP.LibraryExtension.Lists;

namespace UnitTest.RouteFinding.TerminalProcedures.Sid
{
    [TestFixture]
    public class SidCollectionTest
    {
        private readonly Waypoint runway05 = new Waypoint("ABCD05", 10.0, 19.0);

        #region OnlyRwySpecificPart

        [Test]
        public void OnlyRwySpecificPart()
        {
            onlyRwySpecificPart(true);
            onlyRwySpecificPart(false);
        }

        private void onlyRwySpecificPart(bool hasVector)
        {
            var sids = new SidCollection(CreateList(RwySpecificPart(hasVector)));

            var info = sids.GetSidInfo("SID1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPT02", 11.0, 20.0)
                .Equals(info.LastWaypoint));

            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.AreEqual(DistanceRwySpecificPart(),
                info.TotalDistance, 1E-8);
        }

        private double DistanceRwySpecificPart()
        {
            return CreateList(
                runway05,
                new Waypoint("WPT01", 10.0, 20.0),
                new Waypoint("WPT02", 11.0, 20.0))
                .TotalDistance();
        }

        private SidEntry RwySpecificPart(bool hasVector)
        {
            return new SidEntry(
                "05",
                "SID1",
                CreateList(
                    new Waypoint("WPT01", 10.0, 20.0),
                    new Waypoint("WPT02", 11.0, 20.0)),
                EntryType.RwySpecific,
                hasVector);
        }

        #endregion

        #region RwySpecificAndCommonPart

        [Test]
        public void RwySpecificAndCommonPart()
        {
            rwySpecificAndCommonPart(true);
            rwySpecificAndCommonPart(false);
        }

        private void rwySpecificAndCommonPart(bool hasVector)
        {
            var sids = new SidCollection(
                CreateList(RwySpecificPart(!hasVector), CommonPart(hasVector)));

            var info = sids.GetSidInfo("SID1", "05", runway05);
            Assert.IsTrue(new Waypoint("WPTA", 12.0, 21.0).Equals(info.LastWaypoint));

            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.AreEqual(DistanceRwySpecificAndCommonPart(),
                info.TotalDistance, 1E-8);
        }

        private SidEntry CommonPart(bool hasVector)
        {
            return new SidEntry(
                "ALL",
                "SID1",
                CreateList(
                    new Waypoint("WPT02", 11.0, 20.0),
                    new Waypoint("WPTA", 12.0, 21.0)),
                EntryType.Common,
                hasVector);
        }

        private double DistanceRwySpecificAndCommonPart()
        {
            return CreateList(
                runway05,
                new Waypoint("WPT01", 10.0, 20.0),
                new Waypoint("WPT02", 11.0, 20.0),
                new Waypoint("WPTA", 12.0, 21.0))
                .TotalDistance();
        }

        #endregion

        #region RwySpecificAndCommonAndTransitionPart

        [Test]
        public void RwySpecificAndCommonAndTransitionPart()
        {
            rwySpecificAndCommonAndTransitionPart(true);
            rwySpecificAndCommonAndTransitionPart(false);
        }

        private void rwySpecificAndCommonAndTransitionPart(bool hasVector)
        {
            var sids =
                new SidCollection(
                    CreateList(
                        RwySpecificPart(!hasVector),
                        CommonPart(!hasVector),
                        TransitionPart(hasVector)));

            var info = sids.GetSidInfo("SID1.TRANS1", "05", runway05);
            Assert.IsTrue(new Waypoint("WPTCC", 13.0, 22.0).Equals(info.LastWaypoint));

            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.AreEqual(DistanceRwySpecificAndCommonAndTransitionPart(),
                info.TotalDistance, 1E-8);
        }

        private SidEntry TransitionPart(bool hasVector)
        {
            return new SidEntry(
                "TRANS1",
                "SID1",
                CreateList(
                    new Waypoint("WPTA", 12.0, 21.0),
                    new Waypoint("WPTBB", 12.0, 22.0),
                    new Waypoint("WPTCC", 13.0, 22.0)),
                EntryType.Transition,
                hasVector);
        }

        private double DistanceRwySpecificAndCommonAndTransitionPart()
        {
            return CreateList(
                runway05,
                new Waypoint("WPT01", 10.0, 20.0),
                new Waypoint("WPT02", 11.0, 20.0),
                new Waypoint("WPTA", 12.0, 21.0),
                new Waypoint("WPTBB", 12.0, 22.0),
                new Waypoint("WPTCC", 13.0, 22.0))
                .TotalDistance();
        }

        #endregion

    }
}
