using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Airports;
using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.RouteFinding.Containers;
using static QSP.MathTools.Utilities;
using static Tests.Common.Utilities;

namespace Tests.RouteFinding.RouteAnalyzers
{
    [TestClass]
    public class StandardRouteAnalyzerTest
    {
        [TestMethod]
        public void EmptyRouteShouldReturnDirect()
        {
            // Setup
            var db = new AirportDatabase();

            db.Add(new Airport("RCTP", "", 0.0, 0.0, 0, 0, 0, 0,
                               new List<RwyData>() { new RwyData("05L", "", 0, 0, true, "", "", 25.072894, 121.215986, 0, 0.0, 0, 0, 0) }));
            db.Add(new Airport("VHHH", "", 0.0, 0.0, 0, 0, 0, 0,
                               new List<RwyData>() { new RwyData("07L", "", 0, 0, true, "", "", 22.310917, 113.897964, 0, 0.0, 0, 0, 0) }));

            var analyzer = new StandardRouteAnalyzer(new string[] {},
                                                   "RCTP",
                                                   "05L",
                                                   "VHHH",
                                                   "07L",
                                                   new AirportManager(db),
                                                   null,
                                                   new SidCollection(new List<SidEntry>()),
                                                   new StarCollection(new List<StarEntry>()));
            // Invoke
            var route = analyzer.Analyze();

            // Assert
            var node = route.FirstNode;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("RCTP05L", 25.072894, 121.215986)) &&
                          node.Value.AirwayToNext == "DCT" &&
                          WithinPrecision(node.Value.DistanceToNext, GreatCircleDistance(25.072894, 121.215986, 22.310917, 113.897964), 1E-8));

            node = node.Next;
            Assert.IsTrue(node.Value.Waypoint.Equals(new Waypoint("VHHH07L", 22.310917, 113.897964)) &&
                          node == route.LastNode);
        }



    }
}
