using NUnit.Framework;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.TerminalProcedures.Sid;
using static CommonLibrary.LibraryExtension.Types;

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
            OnlyRwySpecificPart(true);
            OnlyRwySpecificPart(false);
        }

        private void OnlyRwySpecificPart(bool hasVector)
        {
            var sids = new SidCollection(List(RwySpecificPart(hasVector)));

            var info = sids.GetSidInfo("SID1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPT02", 11.0, 20.0)
                .Equals(info.LastWaypoint));

            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.AreEqual(DistanceRwySpecificPart(),
                info.TotalDistance, 1E-8);
        }

        private double DistanceRwySpecificPart()
        {
            return List(runway05,
                        new Waypoint("WPT01", 10.0, 20.0),
                        new Waypoint("WPT02", 11.0, 20.0))
                .TotalDistance();
        }

        private SidEntry RwySpecificPart(bool hasVector)
        {
            return new SidEntry(
                "05",
                "SID1",
                List(new Waypoint("WPT01", 10.0, 20.0),
                     new Waypoint("WPT02", 11.0, 20.0)),
                EntryType.RwySpecific,
                hasVector);
        }

        #endregion

        #region RwySpecificAndCommonPart

        [Test]
        public void RwySpecificAndCommonPart()
        {
            RwySpecificAndCommonPart(true);
            RwySpecificAndCommonPart(false);
        }

        private void RwySpecificAndCommonPart(bool hasVector)
        {
            var sids = new SidCollection(
                List(RwySpecificPart(!hasVector), CommonPart(hasVector)));

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
                List(new Waypoint("WPT02", 11.0, 20.0),
                     new Waypoint("WPTA", 12.0, 21.0)),
                EntryType.Common,
                hasVector);
        }

        private double DistanceRwySpecificAndCommonPart()
        {
            return List(runway05,
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
            RwySpecificAndCommonAndTransitionPart(true);
            RwySpecificAndCommonAndTransitionPart(false);
        }

        private void RwySpecificAndCommonAndTransitionPart(bool hasVector)
        {
            var sids = new SidCollection(List(RwySpecificPart(!hasVector),
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
                List(new Waypoint("WPTA", 12.0, 21.0),
                     new Waypoint("WPTBB", 12.0, 22.0),
                     new Waypoint("WPTCC", 13.0, 22.0)),
                EntryType.Transition,
                hasVector);
        }

        private double DistanceRwySpecificAndCommonAndTransitionPart()
        {
            return List(runway05,
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
