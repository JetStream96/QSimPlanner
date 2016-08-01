using NUnit.Framework;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using System.Linq;

namespace UnitTest.RouteFinding.Tracks.Common.TDM.Parser
{
    [TestFixture]
    public class ConnectionRouteInterpreterTest
    {
        [Test]
        public void RecongnizeToOrFromCorrectly()
        {
            var result = ConnectionRouteInterpreter.Convert(
                new string[] { "P1", "P2", "P3" },
                new string[] { "Q P1", "P3 R" },
                new AirportManager(new AirportCollection()));

            Assert.AreEqual(1, result.RouteFrom.Count);
            Assert.AreEqual(1, result.RouteTo.Count);

            Assert.IsTrue(Enumerable.SequenceEqual(
                result.RouteFrom[0],
                new string[] { "Q", "P1" }));

            Assert.IsTrue(Enumerable.SequenceEqual(
                result.RouteTo[0],
                new string[] { "P3", "R" }));
        }

        [Test]
        public void RemovesAirportsCorrectly()
        {
            var airports = new AirportCollection();
            airports.Add(new Airport("ABCD", "", 0.0, 0.0, 0, true, 0, 0, 0, null));
            airports.Add(new Airport("EFGH", "", 0.0, 0.0, 0, true, 0, 0, 0, null));
            
            var result = ConnectionRouteInterpreter.Convert(
                new string[] { "P1", "P2", "P3" },
                new string[] { "ABCD Q P1", "P3 R EFGH" },
                new AirportManager(airports));

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
