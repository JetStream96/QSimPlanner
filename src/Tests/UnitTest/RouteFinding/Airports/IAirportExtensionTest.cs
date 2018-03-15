using FakeItEasy;
using NUnit.Framework;
using QSP.RouteFinding.Airports;
using static QSP.LibraryExtension.Types;
using static QSP.RouteFinding.Airports.IAirportExtensions;

namespace UnitTest.RouteFinding.Airports
{
    [TestFixture]
    public class IAirportExtensionTest
    {
        [Test]
        public void GetSlopePercentShouldComputeSlope()
        {
            var rwy0 = A.Fake<IRwyData>();
            A.CallTo(() => rwy0.RwyIdent).Returns("12");
            A.CallTo(() => rwy0.ElevationFt).Returns(-5);
            A.CallTo(() => rwy0.LengthFt).Returns(10000);

            var rwy1 = A.Fake<IRwyData>();
            A.CallTo(() => rwy1.RwyIdent).Returns("30");
            A.CallTo(() => rwy1.ElevationFt).Returns(-10);
            A.CallTo(() => rwy1.LengthFt).Returns(10000);

            var airport = A.Fake<IAirport>();
            A.CallTo(() => airport.Rwys).Returns(List(rwy0, rwy1));

            Assert.AreEqual(airport.GetSlopePercent("30").Value, 5.0 / 10000.0 * 100, 0.0001);

            var rwy2 = A.Fake<IRwyData>();
            A.CallTo(() => rwy2.RwyIdent).Returns("33L");
            A.CallTo(() => rwy2.ElevationFt).Returns(10);
            A.CallTo(() => rwy2.LengthFt).Returns(10000);
        }

        [Test]
        public void GetSlopePercentCannotFindRunway()
        {
            var airport = A.Fake<IAirport>();
            A.CallTo(() => airport.Rwys).Returns(new IRwyData[0]);

            Assert.IsNull(airport.GetSlopePercent("30"));
        }

        [Test]
        public void GetSlopePercentCannotOppositeRunway()
        {
            var rwy0 = A.Fake<IRwyData>();
            A.CallTo(() => rwy0.RwyIdent).Returns("12");
            A.CallTo(() => rwy0.ElevationFt).Returns(-5);
            A.CallTo(() => rwy0.LengthFt).Returns(10000);

            var airport = A.Fake<IAirport>();
            A.CallTo(() => airport.Rwys).Returns(List(rwy0));

            Assert.IsNull(airport.GetSlopePercent("12"));
        }
    }
}
