using NUnit.Framework;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Tracks.Common.TDM.Parser;
using System.Linq;
using static UnitTest.RouteFinding.Common;

namespace UnitTest.RouteFinding.Tracks.Common.TDM.Parser
{
    [TestFixture]
    public class ConnectionRouteInterpreterTest
    {
        [Test]
        public void RecongnizeToOrFromCorrectly()
        {
            var result = ConnectionRouteInterpreter.Convert(
                new[] { "P1", "P2", "P3" },
                new[] { "Q P1", "P3 R" },
                new AirportManager());

            Assert.AreEqual(1, result.RouteFrom.Count);
            Assert.AreEqual(1, result.RouteTo.Count);

            Assert.IsTrue(result.RouteFrom[0].SequenceEqual(new[] { "Q", "P1" }));
            Assert.IsTrue(result.RouteTo[0].SequenceEqual(new[] { "P3", "R" }));
        }

        [Test]
        public void RemovesAirportsCorrectly()
        {
            var airports = new AirportManager();
            airports.Add(GetAirport("ABCD"));
            airports.Add(GetAirport("EFGH"));

            var result = ConnectionRouteInterpreter.Convert(
                new[] { "P1", "P2", "P3" },
                new[] { "ABCD Q P1", "P3 R EFGH" },
                airports);

            Assert.AreEqual(1, result.RouteFrom.Count);
            Assert.AreEqual(1, result.RouteTo.Count);

            Assert.IsTrue(result.RouteFrom[0].SequenceEqual(new[] { "Q", "P1" }));
            Assert.IsTrue(result.RouteTo[0].SequenceEqual(new[] { "P3", "R" }));
        }
    }
}
