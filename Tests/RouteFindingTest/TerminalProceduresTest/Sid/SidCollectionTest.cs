using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP;
using QSP.RouteFinding.TerminalProcedures;
using static QSP.RouteFinding.Utilities;
using static Tests.Common.Utilities;
using static QSP.LibraryExtension.Lists;
using QSP.RouteFinding.Containers;

namespace Tests.RouteFindingTest.TerminalProceduresTest.Sid
{
    [TestClass]
    public class SidCollectionTest
    {
        private Waypoint runway05 = new Waypoint("ABCD05", 10.0, 19.0);

        #region OnlyRwySpecificPart

        [TestMethod]
        public void GetSidInfoTest_OnlyRwySpecificPartWithOrWithoutVector()
        {
            GetSidInfoTest_OnlyRwySpecificPart(true);
            GetSidInfoTest_OnlyRwySpecificPart(false);
        }

        private void GetSidInfoTest_OnlyRwySpecificPart(bool hasVector)
        {
            var info = new SidCollection(CreateList(rwySpecificPart(hasVector))).GetSidInfo("SID1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPT02", 11.0, 20.0).Equals(info.LastWaypoint));
            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.IsTrue(WithinPrecisionPercent(distanceRwySpecificPart(), info.TotalDistance, 0.001));
        }

        private double distanceRwySpecificPart()
        {
            return GetTotalDistance(CreateList(runway05,
                                               new Waypoint("WPT01", 10.0, 20.0),
                                               new Waypoint("WPT02", 11.0, 20.0)));
        }

        private SidEntry rwySpecificPart(bool hasVector)
        {
            return new SidEntry("05",
                                "SID1",
                                CreateList(runway05,
                                           new Waypoint("WPT01", 10.0, 20.0),
                                           new Waypoint("WPT02", 11.0, 20.0)),
                                EntryType.RwySpecific,
                                hasVector);
        }

        #endregion

        #region RwySpecificAndCommonPart

        [TestMethod]
        public void GetSidInfoTest_RwySpecificAndCommonPartWithOrWithoutVector()
        {
            GetSidInfoTest_RwySpecificAndCommonPart(true);
            GetSidInfoTest_RwySpecificAndCommonPart(false);
        }

        private void GetSidInfoTest_RwySpecificAndCommonPart(bool hasVector)
        {
            var info = new SidCollection(CreateList(rwySpecificPart(!hasVector),
                                                    commonPart(hasVector)))
                       .GetSidInfo("SID1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPTA", 12.0, 21.0).Equals(info.LastWaypoint));
            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.IsTrue(WithinPrecisionPercent(distanceRwySpecificAndCommonPart(), info.TotalDistance, 0.001));
        }

        private SidEntry commonPart(bool hasVector)
        {
            return new SidEntry("ALL",
                                "SID1",
                                CreateList(new Waypoint("WPT02", 11.0, 20.0),
                                           new Waypoint("WPTA", 12.0, 21.0)),
                                EntryType.Common,
                                hasVector);
        }

        private double distanceRwySpecificAndCommonPart()
        {
            var wpts = new List<Waypoint>();
            wpts.Add(runway05);
            wpts.Add(new Waypoint("WPT01", 10.0, 20.0));
            wpts.Add(new Waypoint("WPT02", 11.0, 20.0));
            wpts.Add(new Waypoint("WPTA", 12.0, 21.0));

            return GetTotalDistance(CreateList(runway05,
                                               new Waypoint("WPT01", 10.0, 20.0),
                                               new Waypoint("WPT02", 11.0, 20.0),
                                               new Waypoint("WPTA", 12.0, 21.0)));
        }

        #endregion

        #region RwySpecificAndCommonAndTransitionPart

        [TestMethod]
        public void GetSidInfoTest_RwySpecificAndCommonAndTransitionPartWithOrWithoutVector()
        {
            GetSidInfoTest_RwySpecificAndCommonAndTransitionPart(true);
            GetSidInfoTest_RwySpecificAndCommonAndTransitionPart(false);
        }

        private void GetSidInfoTest_RwySpecificAndCommonAndTransitionPart(bool hasVector)
        {
            var info = new SidCollection(CreateList(rwySpecificPart(!hasVector),
                                                    commonPart(!hasVector),
                                                    transitionPart(hasVector)))
                       .GetSidInfo("SID1.TRANS1", "05", runway05);

            Assert.IsTrue(new Waypoint("WPTCC", 13.0, 22.0).Equals(info.LastWaypoint));
            Assert.AreEqual(hasVector, info.EndsWithVector);
            Assert.IsTrue(WithinPrecisionPercent(distanceRwySpecificAndCommonAndTransitionPart(), info.TotalDistance, 0.001));
        }

        private SidEntry transitionPart(bool hasVector)
        {
            return new SidEntry("TRANS1",
                                "SID1",
                                CreateList(new Waypoint("WPTA", 12.0, 21.0),
                                           new Waypoint("WPTBB", 12.0, 22.0),
                                           new Waypoint("WPTCC", 13.0, 22.0)),
                                EntryType.Transition,
                                hasVector);
        }

        private double distanceRwySpecificAndCommonAndTransitionPart()
        {
            return GetTotalDistance(CreateList(runway05,
                                               new Waypoint("WPT01", 10.0, 20.0),
                                               new Waypoint("WPT02", 11.0, 20.0),
                                               new Waypoint("WPTA", 12.0, 21.0),
                                               new Waypoint("WPTBB", 12.0, 22.0),
                                               new Waypoint("WPTCC", 13.0, 22.0)));
        }

        #endregion

    }
}
