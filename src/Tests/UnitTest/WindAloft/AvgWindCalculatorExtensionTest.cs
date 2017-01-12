using NUnit.Framework;
using QSP.WindAloft;
using QSP.AviationTools.Coordinates;

namespace UnitTest.WindAloft
{
    [TestFixture]
    public class AvgWindCalculatorExtensionTest
    {
        [Test]
        public void GetAirDistanceLessThanTwoElementShouldReturnZero()
        {
            var calc = new AvgWindCalculator(new DefaultWindTableCollection(), 100.0, 10000.0);

            var zeroElem = new LatLon[] { };
            var oneElem = new[] { new LatLon(0.0, 1.0) };

            Assert.AreEqual(0.0, calc.GetAirDistance(zeroElem));
            Assert.AreEqual(0.0, calc.GetAirDistance(oneElem));
        }

        [Test]
        public void GetAirDistanceTest()
        {
            LatLon[] pts =
            {
                new LatLon(0.0, 1.0),
                new LatLon(0.0, 2.0),
                new LatLon(0.0, 3.0)
            };

            Assert.AreEqual(pts[0].Distance(pts[2]),
                GetCalc().GetAirDistance(pts),
                1E-4);
        }

        private static AvgWindCalculator GetCalc()
        {
            return new AvgWindCalculator(new DefaultWindTableCollection(), 100.0, 10000.0);
        }
    }
}
