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
        [Test]
        public void MachToTasTest()
        {
            Assert.IsTrue(WithinPrecisionPercent(491.45, TasKnots(0.8, 20000.0), 0.1));
            Assert.IsTrue(WithinPrecisionPercent(413.138, TasKnots(0.72, 36000.0), 0.1));
        }

        [Test]
        public void IasToTasTest()
        {
            const double delta = 0.1;
            Assert.AreEqual(409.0, Ktas(250.0, 30000.0), delta);
        }
    }
}
