using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using QSP.RouteFinding.Airports;
using System.Linq;

namespace UnitTest.RouteFinding.Tracks.Common.TDM.Parser
{
    [TestClass]
    public class ConnectionRouteInterpreterTest
    {
        [TestMethod]
        public void RecongnizeToOrFromCorrectly()
        {
            var interpreter = new ConnectionRouteInterpreter(
                new string[] { "P1", "P2", "P3" },
                Array.AsReadOnly(new string[] { "Q P1", "P3 R" }),
                new AirportManager(new AirportCollection()));

            var result = interpreter.Convert();

            Assert.AreEqual(1, result.RouteFrom.Count);
            Assert.AreEqual(1, result.RouteTo.Count);

            Assert.IsTrue(Enumerable.SequenceEqual(
                result.RouteFrom[0],
                new string[] { "Q", "P1" }));

            Assert.IsTrue(Enumerable.SequenceEqual(
                result.RouteTo[0],
                new string[] { "P3", "R" }));
        }

        [TestMethod]
        public void RemovesAirportsCorrectly()
        {
            var airports = new AirportCollection();
            airports.Add(new Airport("ABCD", "", 0.0, 0.0, 0, 0, 0, 0, null));
            airports.Add(new Airport("EFGH", "", 0.0, 0.0, 0, 0, 0, 0, null));

            var interpreter = new ConnectionRouteInterpreter(
                new string[] { "P1", "P2", "P3" },
                Array.AsReadOnly(new string[] { "ABCD Q P1", "P3 R EFGH" }),
                new AirportManager(airports));

            var result = interpreter.Convert();

            Assert.AreEqual(1, result.RouteFrom.Count);
            Assert.AreEqual(1, result.RouteTo.Count);

            Assert.IsTrue(Enumerable.SequenceEqual(
                result.RouteFrom[0],
                new string[] { "Q", "P1" }));

            Assert.IsTrue(Enumerable.SequenceEqual(
                result.RouteTo[0],
                new string[] { "P3", "R" }));
        }
    }
}
