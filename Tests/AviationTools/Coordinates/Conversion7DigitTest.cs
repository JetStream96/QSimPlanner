using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Conversion7Digit;

namespace Tests.AviationTools.Coordinates
{
    [TestClass]
    public class Conversion7DigitTest
    {
        [TestMethod()]
        public void WhenFormatIsWrongReturnFalse()
        {
            Assert.IsFalse(Is7DigitFormat("37U055E"));
            Assert.IsFalse(Is7DigitFormat("37S155S"));
            Assert.IsFalse(Is7DigitFormat("37S15AW"));
            Assert.IsFalse(Is7DigitFormat("37S155"));
            Assert.IsFalse(Is7DigitFormat("37N200E"));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectReturnTrue()
        {
            Assert.IsTrue(Is7DigitFormat("37N055E"));
            Assert.IsTrue(Is7DigitFormat("37S155W"));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(ReadFrom7DigitFormat("36N170W").Equals(new LatLon(36.0, -170.0)));
            Assert.IsTrue(ReadFrom7DigitFormat("43N034E").Equals(new LatLon(43.0, 34.0)));
            Assert.IsTrue(ReadFrom7DigitFormat("57S113E").Equals(new LatLon(-57.0, 113.0)));
            Assert.IsTrue(ReadFrom7DigitFormat("57S110W").Equals(new LatLon(-57.0, -110.0)));
        }

        [TestMethod]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(To7DigitFormat(36.0, -170.0).Equals("36N170W"));
            Assert.IsTrue(To7DigitFormat(36.0, 170.0).Equals("36N170E"));
            Assert.IsTrue(To7DigitFormat(-36.0, -170.0).Equals("36S170W"));
            Assert.IsTrue(To7DigitFormat(-36.0, 170.0).Equals("36S170E"));
        }
    }
}
