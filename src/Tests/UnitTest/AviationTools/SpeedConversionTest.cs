using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static UnitTest.Common.Utilities;
using static QSP.AviationTools.SpeedConversion;

namespace UnitTest.AviationTools
{
    [TestFixture]
    public class SpeedConversionTest
    {
        // The values in this test come from online speed calculator and
        // aircraft manuals.

        [Test]
        public void MachToTasTest()
        {
            const double delta = 0.5;

            Assert.AreEqual(491.45, TasKnots(0.8, 20000.0), delta);
            Assert.AreEqual(413.138, TasKnots(0.72, 36000.0), delta);
        }

        [Test]
        public void CasToTasTest()
        {
            const double delta = 0.2;
            Assert.AreEqual(393.73, Ktas(250.0, 30000.0), delta);
        }

        [Test]
        public void KcasToKeasTest()
        {
            const double delta = 0.1;
            Assert.AreEqual(284.999, KcasToKeas(300.0, 30000.0), delta);
        }

        [Test]
        public void MachToKcasTest()
        {
            const double delta = 0.5;
            Assert.AreEqual(259.299, Kcas(0.85, 40000.0), delta);
        }

        [Test]
        public void MachToKeasTest()
        {
            const double d = 1.0;
            Assert.AreEqual(241.893, MachToKeas(0.85, Delta(40000.0)), d);
        }

        [Test]
        public void KeasToMachTest()
        {
            const double d = 0.01;
            Assert.AreEqual(0.85, MachNumber(241.893, Delta(40000.0)), d);
        }
    }
}
