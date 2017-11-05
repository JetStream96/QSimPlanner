using System;
using NUnit.Framework;
using QSP.Utilities.Units;
using static QSP.AviationTools.ConversionTools;
using static UnitTest.Common.Utilities;

namespace UnitTest.AviationTools
{
    [TestFixture]
    public class CoversionToolsTest
    {
        [Test]
        public void IsaTempTest()
        {
            Assert.AreEqual(IsaTemp(20000.0), -24.624, 1.0);
            Assert.AreEqual(IsaTemp(41000.0), -56.5, 1.0);
        }

        [Test]
        public void IsaTempTestException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            IsaTemp(80000.0));
        }

        [Test]
        public void AltToPressureMbTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(218.721, PressureMb(36800.0), 1.0));
        }

        [Test]
        public void PressureToAltFtTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(29272.26, PressureMbToAltFt(311.0), 1.0));
        }

        [Test]
        public void AirDensityTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(0.3723, AirDensity(35500.0), 1.0));
        }
        
        [Test]
        public void PressureAltitudeFtTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(5000.0 + 419.0, PressureAltitudeFt(5000.0, 998.0), 1.0));
        }

        [Test]
        public void ToCelsiusTest()
        {
            Assert.AreEqual(ToCelsius(50.0), 10.0, 0.1);
        }

        [Test]
        public void ToFahrenheitTest()
        {
            Assert.AreEqual(ToFahrenheit(-50.0), -58.0, 0.1);
        }

        [Test]
        public void ParseHeadingTest()
        {
            Assert.AreEqual(0.0, ParseHeading("0.0"), 1E-8);
            Assert.AreEqual(350.0, ParseHeading("-10"), 1E-8);
            Assert.AreEqual(160.0, ParseHeading("160"), 1E-8);
            Assert.IsNull(ParseHeading("X"));
        }

        [Test]
        public void HeadwindComponentTest()
        {
            Assert.AreEqual(17.6776695297, HeadwindComponent(10, 55, 25), 1E-6);
        }

        [Test]
        public void GetLengthMeterTest()
        {
            Assert.AreEqual(3.048, GetLengthMeter(10, LengthUnit.Feet), 0.01);
            Assert.AreEqual(8, GetLengthMeter(8, LengthUnit.Meter), 1e-6);
        }

        [Test]
        public void GetLengthFtTest()
        {
            Assert.AreEqual(10, GetLengthFt(3.048, LengthUnit.Meter), 0.1);
            Assert.AreEqual(8, GetLengthFt(8, LengthUnit.Feet), 1e-6);
        }
    }
}
