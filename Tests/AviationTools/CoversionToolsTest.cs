using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.AviationTools.CoversionTools;
using static Tests.Common.Utilities;

namespace Tests.AviationToolsTest
{
    [TestClass]
    public class CoversionToolsTest
    {
        [TestMethod]
        public void MachToTasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(491.45, MachToTas(0.8, 20000.0), 0.1));
            Assert.IsTrue(WithinPrecisionPercent(413.138, MachToTas(0.72, 36000.0), 0.1));
        }

        [TestMethod]
        public void IsaTempTest()
        {
            Assert.IsTrue(WithinPrecision(IsaTemp(20000.0), -24.624, 1.0));
            Assert.IsTrue(WithinPrecision(IsaTemp(41000.0), -56.5, 1.0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IsaTempTestException()
        {
            IsaTemp(80000.0);
        }

        [TestMethod]
        public void AltToPressureMbTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(218.721, AltToPressureMb(36800.0), 1.0));
        }

        [TestMethod]
        public void PressureToAltFtTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(29272.26, PressureMbToAltFT(311.0), 1.0));
        }

        [TestMethod]
        public void AirDensityTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(0.3723, AirDensity(35500.0), 1.0));
        }

        [TestMethod]
        public void KtasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(498.0, Ktas(280.0, 34500.0), 1.0));
        }

        [TestMethod()]
        public void RwyIdentOppositeDirTest()
        {
            Assert.AreEqual("18", RwyIdentOppositeDir("36"));
            Assert.AreEqual("15", RwyIdentOppositeDir("33"));
            Assert.AreEqual("19", RwyIdentOppositeDir("01"));
        }

        [TestMethod]
        public void PressureAltitudeFtTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(5000.0 + 419.0, PressureAltitudeFt(5000.0, 998.0), 1.0));
        }

        [TestMethod]
        public void ToCelsiusTest()
        {
            Assert.IsTrue(WithinPrecision(ToCelsius(50.0), 10.0, 0.1));
        }

        [TestMethod]
        public void ToFahrenheitTest()
        {
            Assert.IsTrue(WithinPrecision(ToFahrenheit(-50.0), -58.0, 0.1));
        }

    }
}
