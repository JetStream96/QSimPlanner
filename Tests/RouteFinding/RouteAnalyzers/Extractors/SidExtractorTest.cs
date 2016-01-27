using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.RouteAnalyzers.Extractors;
using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures;
using static Tests.Common.Utilities;
using static QSP.MathTools.Utilities;

namespace Tests.RouteFinding.RouteAnalyzers.Extractors
{
    [TestClass]
    public class SidExtractorTest
    {
        [TestMethod]
        public void WhenInputIsEmptyLinkedListShouldReturnRwy()
        {
            var route = new LinkedList<string>();
            var rwyWpt = new Waypoint("RCTP05L", 20.0, 120.0);
            var extractor = new SidExtractor(route, "", "", rwyWpt, null, null);

            var origRoute = extractor.Extract();

            Assert.AreEqual(1, origRoute.Count);
            Assert.IsTrue(origRoute.First.Waypoint.Equals(rwyWpt));
        }

        [TestMethod]
        public void WhenExistsShouldRemoveIcao()
        {
            var route = new LinkedList<string>(new string[] { "RCTP", "HLG", "A1", "MKG" });
            var extractor = new SidExtractor(route, "RCTP", "", new Waypoint("RCTP05L", 20.0, 120.0), null, null);

            var origRoute = extractor.Extract();

            Assert.AreEqual(3, route.Count);
            Assert.IsFalse(origRoute.First.Waypoint.ID == "RCTP");
        }

        [TestMethod]
        public void WhenSidExistsShouldRemoveAndAddToOrigRoute()
        {
            // Setup
            var route = new LinkedList<string>(new string[] { "SID1", "HLG", "A1", "MKG" });
            var wptList = new WaypointList();
            wptList.AddWaypoint( new Waypoint("HLG", 22.0, 122.0));

            var extractor = new SidExtractor(route,
                                             "RCTP",
                                             "05L",
                                             new Waypoint("RCTP05L", 20.0, 120.0),
                                             wptList,
                                             new SidCollection(new List<SidEntry>() {
                                                 new SidEntry("05L",
                                                              "SID1",
                                                              new List<Waypoint>(){new Waypoint("HLG", 22.0, 122.0) },
                                                              EntryType.RwySpecific,
                                                              false) }));
            // Invoke
            var origRoute = extractor.Extract();

            // Assert
            Assert.AreEqual(3, route.Count);
            Assert.IsTrue(route.First.Value == "HLG");

            Assert.AreEqual(2, origRoute.Count);

            var node = origRoute.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 20.0, 120.0)) &&
                          node.Value.AirwayToNext == "SID1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(20.0, 120.0, 22.0, 122.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("HLG", 22.0, 122.0)) &&
                          node == origRoute.LastNode);

        }

        [TestMethod]
        public void WhenSidLastWptNotInWptListShouldRemoveFromRoute()
        {
            // Setup
            var route = new LinkedList<string>(new string[] { "SID1", "P1", "HLG", "A1", "MKG" });
            var wptList = new WaypointList();
            wptList.AddWaypoint(new Waypoint("HLG", 22.0, 122.0));

            var extractor = new SidExtractor(route,
                                             "RCTP",
                                             "05L",
                                             new Waypoint("RCTP05L", 20.0, 120.0),
                                             wptList,
                                             new SidCollection(new List<SidEntry>() {
                                                 new SidEntry("05L",
                                                              "SID1",
                                                              new List<Waypoint>(){new Waypoint("P1", 21.0, 121.0) },
                                                              EntryType.RwySpecific,
                                                              false) }));
            // Invoke
            var origRoute = extractor.Extract();

            // Assert
            Assert.AreEqual(3, route.Count);
            Assert.IsTrue(route.First.Value == "HLG");

            Assert.AreEqual(2, origRoute.Count);

            var node = origRoute.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 20.0, 120.0)) &&
                          node.Value.AirwayToNext == "SID1" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(20.0, 120.0, 21.0, 121.0), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("P1", 21.0, 121.0)) &&
                          node == origRoute.LastNode);
        }
    }
}
