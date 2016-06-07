using System;
using NUnit.Framework;
using static QSP.AviationTools.CoversionTools;
using static UnitTest.Common.Utilities;

namespace UnitTest.AviationToolsTest
{
    [TestFixture]
    public class CoversionToolsTest
    {
        [Test]
        public void MachToTasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(491.45, MachToTas(0.8, 20000.0), 0.1));
            Assert.IsTrue(WithinPrecisionPercent(413.138, MachToTas(0.72, 36000.0), 0.1));
        }

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
            Assert.IsTrue(WithinPrecisionPercent(218.721, AltToPressureMb(36800.0), 1.0));
        }

        [Test]
        public void PressureToAltFtTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(29272.26, PressureMbToAltFT(311.0), 1.0));
        }

        [Test]
        public void AirDensityTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(0.3723, AirDensity(35500.0), 1.0));
        }

        [Test]
        public void KtasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(498.0, Ktas(280.0, 34500.0), 1.0));
        }

        [Test]
        public void RwyIdentOppositeDirTest()
        {
            Assert.AreEqual("18", RwyIdentOppositeDir("36"));
            Assert.AreEqual("15", RwyIdentOppositeDir("33"));
            Assert.AreEqual("19", RwyIdentOppositeDir("01"));
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

    }
}
