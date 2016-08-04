using NUnit.Framework;
using QSP.RouteFinding.Tracks.Pacots;

namespace UnitTest.RouteFinding.Tracks.Pacots
{
    [TestFixture]
    public class TrackValidPeriodTest
    {
        [Test]
        public void GetValidPeriodTest()
        {
            var input = "BETWEEN 02161200UTC AND 02161600UTC,";
            var result = TrackValidPeriod.GetValidPeriod(input);

            Assert.IsTrue(result.Start == "02161200UTC");
            Assert.IsTrue(result.End == "02161600UTC");
        }

        [Test]
        public void GetValidPeriodInvalidInput()
        {
            var input = "BETWEEN 02161200UTC AND ";
            var result = TrackValidPeriod.GetValidPeriod(input);

            Assert.IsTrue(result.Start == "");
            Assert.IsTrue(result.End == "");
        }
    }
}
