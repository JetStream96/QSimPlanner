using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Format7Digit;

namespace Tests.AviationTools.Coordinates
{
    [TestClass]
    public class Format7DigitTest
    {
        [TestMethod()]
        public void WhenFormatIsWrongReturnFalse()
        {
            LatLon r;

            Assert.IsFalse(TryReadFrom7DigitFormat("37U055E", out r));
            Assert.IsFalse(TryReadFrom7DigitFormat("37S155S", out r));
            Assert.IsFalse(TryReadFrom7DigitFormat("37S15AW", out r));
            Assert.IsFalse(TryReadFrom7DigitFormat("37S155", out r));
            Assert.IsFalse(TryReadFrom7DigitFormat("37N200E", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectReturnTrue()
        {
            LatLon r;

            Assert.IsTrue(TryReadFrom7DigitFormat("37N055E", out r));
            Assert.IsTrue(TryReadFrom7DigitFormat("37S155W", out r));
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
