using NUnit.Framework;
using QSP.RouteFinding.Tracks.Nats;
using System;

namespace UnitTest.RouteFinding.Tracks.Nats
{
    [TestFixture]
    public class NatsMessageTest
    {
        [Test]
        public void ParseDateFailTest()
        {
            var (success, _) = NatsMessage.ParseDate("123");
            Assert.IsFalse(success);

            (success, _) = NatsMessage.ParseDate("2017/06/07 xx:38 GMT");
            Assert.IsFalse(success);
        }

        [Test]
        public void ParseDateTest()
        {
            var (success, date) = NatsMessage.ParseDate("2017/06/07 15:38 GMT");
            Assert.IsTrue(success);
            Assert.IsTrue(new DateTime(2017, 6, 7, 15, 38, 0, 0, DateTimeKind.Utc).Equals(date));
        }
    }
}
