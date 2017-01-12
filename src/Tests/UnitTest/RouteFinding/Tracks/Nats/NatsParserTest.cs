using NUnit.Framework;
using QSP.RouteFinding.Tracks.Nats;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.RouteFinding.Tracks.Nats
{
    [TestFixture]
    public class NatsParserTest
    {
        [Test]
        public void CategorizeWestEastTest()
        {
            // Arrange
            var west = new IndividualNatsMessage(
                    "",
                    "",
                    NatsDirection.West,
                    @"042049 EGGXZOZX
A [WPTS]
...
B [WPTS]
...
C [WPTS]
...");

            var east = new IndividualNatsMessage(
                    "",
                    "",
                    NatsDirection.East,
                    @"042049 CZQXZQZX
U [WPTS]
...
V [WPTS]
...
W [WPTS]
...");

            var msg = new NatsMessage(west, east);

            // Act
            var parser = new NatsParser(msg, null);
            var result = parser.Parse();

            // Assert
            Assert.AreEqual(6, result.Count);
            Assert.IsTrue(ContainTrack(result, "A", NatsDirection.West));
            Assert.IsTrue(ContainTrack(result, "B", NatsDirection.West));
            Assert.IsTrue(ContainTrack(result, "C", NatsDirection.West));
            Assert.IsTrue(ContainTrack(result, "U", NatsDirection.East));
            Assert.IsTrue(ContainTrack(result, "V", NatsDirection.East));
            Assert.IsTrue(ContainTrack(result, "W", NatsDirection.East));
        }

        private bool ContainTrack(
            IEnumerable<NorthAtlanticTrack> trks,
            string ident,
            NatsDirection dir)
        {
            return trks.Any(i => i.Ident == ident && i.Direction == dir);
        }

        [Test]
        public void SplitWptsTest()
        {
            // Arrange
            var msg = new NatsMessage(
                new IndividualNatsMessage(
                    "",
                    "",
                    NatsDirection.West,
                    @"042049 EGGXZOZX
A [WPTS]
..."),
                new IndividualNatsMessage(
                    "",
                    "",
                    NatsDirection.East,
                    @"042049 CZQXZQZX
U TUDEP 52/50 53/40 54/30 54/20 DOGAL BEXET
..."));

            // Act
            var parser = new NatsParser(msg, null);
            var result = parser.Parse();

            // Assert
            var trackU = (from trk in result
                          where trk.Ident == "U"
                          select trk).First();

            Assert.IsTrue(trackU.MainRoute.SequenceEqual(
                new[] { "TUDEP", "5250N", "5340N", "5430N", "5420N", "DOGAL", "BEXET" }));
        }
    }
}
