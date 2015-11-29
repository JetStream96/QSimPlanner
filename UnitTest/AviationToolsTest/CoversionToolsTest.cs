using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.AviationTools.CoversionTools;
using static UnitTest.Common.Utilities;

namespace UnitTest.AviationToolsTest
{
    [TestClass]
    public class CoversionToolsTest
    {
        [TestMethod]
        public void MachToTasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(MachToTas(0.8, 20000.0), 491.45, 0.1));
            Assert.IsTrue(WithinPrecisionPercent(MachToTas(0.72, 36000.0), 413.138, 0.1));
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
            Assert.IsTrue(WithinPrecisionPercent(AltToPressureMb(36800.0), 218.721, 1.0));
        }

        [TestMethod]
        public void PressureToAltFtTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(PressureMbToAltFT(311.0), 29272.26, 1.0));
        }

        [TestMethod]
        public void AirDensityTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(AirDensity(35500.0), 0.3723, 1.0));
        }

        [TestMethod]
        public void KtasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(Ktas(280.0, 34500.0), 498.0, 1.0));
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
            Assert.IsTrue(WithinPrecisionPercent(PressureAltitudeFt(5000.0, 998.0), 5000.0 + 419.0, 1.0));
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
