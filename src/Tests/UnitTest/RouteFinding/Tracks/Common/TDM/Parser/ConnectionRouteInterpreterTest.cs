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
                new string[] { "P1", "P2", "P3" },
                new string[] { "Q P1", "P3 R" },
                new AirportManager());

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
            var airports = new AirportManager();
            airports.Add(GetAirport("ABCD", new RwyData[0]));
            airports.Add(GetAirport("EFGH", new RwyData[0]));
            
            var result = ConnectionRouteInterpreter.Convert(
                new string[] { "P1", "P2", "P3" },
                new string[] { "ABCD Q P1", "P3 R EFGH" },
                airports);

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
