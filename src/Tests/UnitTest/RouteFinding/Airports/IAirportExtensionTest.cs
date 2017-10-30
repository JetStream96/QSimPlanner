using Moq;
using NUnit.Framework;
using QSP.RouteFinding.Airports;
using static QSP.RouteFinding.Airports.IAirportExtensions;

namespace UnitTest.RouteFinding.Airports
{
    [TestFixture]
    public class IAirportExtensionTest
    {
        [Test]
        public void GetSlopePercentShouldComputeSlope()
        {
            var rwy0 = new Mock<IRwyData>();
            rwy0.Setup(r => r.RwyIdent).Returns("12");
            rwy0.Setup(r => r.ElevationFt).Returns(-5);
            rwy0.Setup(r => r.LengthFt).Returns(10000);

            var rwy1 = new Mock<IRwyData>();
            rwy1.Setup(r => r.RwyIdent).Returns("30");
            rwy1.Setup(r => r.ElevationFt).Returns(-10);
            rwy1.Setup(r => r.LengthFt).Returns(10000);

            var airport = new Mock<IAirport>();
            airport.Setup(a => a.Rwys).Returns(new[] { rwy0.Object, rwy1.Object });

            Assert.AreEqual(airport.Object.GetSlopePercent("30").Value, 5.0 / 10000.0 * 100, 0.0001);

            var rwy2 = new Mock<IRwyData>();
            rwy2.Setup(r => r.RwyIdent).Returns("33L");
            rwy2.Setup(r => r.ElevationFt).Returns(10);
            rwy2.Setup(r => r.LengthFt).Returns(10000);
        }

        [Test]
        public void GetSlopePercentCannotFindRunway()
        {
            var airport = new Mock<IAirport>();
            airport.Setup(a => a.Rwys).Returns(new IRwyData[0]);

            Assert.IsNull(airport.Object.GetSlopePercent("30"));
        }

        [Test]
        public void GetSlopePercentCannotOppositeRunway()
        {
            var rwy0 = new Mock<IRwyData>();
            rwy0.Setup(r => r.RwyIdent).Returns("12");
            rwy0.Setup(r => r.ElevationFt).Returns(-5);
            rwy0.Setup(r => r.LengthFt).Returns(10000);

            var airport = new Mock<IAirport>();
            airport.Setup(a => a.Rwys).Returns(new[] { rwy0.Object });

            Assert.IsNull(airport.Object.GetSlopePercent("12"));
        }
    }
}
