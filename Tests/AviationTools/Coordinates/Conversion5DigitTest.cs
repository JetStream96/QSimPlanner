using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Conversion5Digit;

namespace Tests.AviationTools.Coordinates
{
    [TestClass]
    public class Conversion5DigitTest
    {
        [TestMethod()]
        public void WhenFormatIsWrongReturnFalse()
        {
            LatLon r;

            Assert.IsFalse(TryReadFrom5DigitFormat("35U055E", out r));
            Assert.IsFalse(TryReadFrom5DigitFormat("35S155S", out r));
            Assert.IsFalse(TryReadFrom5DigitFormat("35S15AW", out r));
            Assert.IsFalse(TryReadFrom5DigitFormat("35S155", out r));
            Assert.IsFalse(TryReadFrom5DigitFormat("35N200E", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectReturnTrue()
        {
            LatLon r;

            Assert.IsTrue(TryReadFrom5DigitFormat("35N00", out r));
            Assert.IsTrue(TryReadFrom5DigitFormat("35E50", out r));
            Assert.IsTrue(TryReadFrom5DigitFormat("35W50", out r));
            Assert.IsTrue(TryReadFrom5DigitFormat("35S50", out r));

            Assert.IsTrue(TryReadFrom5DigitFormat("8565N", out r));
            Assert.IsTrue(TryReadFrom5DigitFormat("8565S", out r));
            Assert.IsTrue(TryReadFrom5DigitFormat("8565E", out r));
            Assert.IsTrue(TryReadFrom5DigitFormat("8565W", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(ReadFrom5DigitFormat("36N50").Equals(new LatLon(36.0, -150.0)));
            Assert.IsTrue(ReadFrom5DigitFormat("4334E").Equals(new LatLon(43.0, 34.0)));
            Assert.IsTrue(ReadFrom5DigitFormat("55S13").Equals(new LatLon(-55.0, 113.0)));
            Assert.IsTrue(ReadFrom5DigitFormat("55W10").Equals(new LatLon(-55.0, -110.0)));
        }

        [TestMethod]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(To5DigitFormat(36.0, -150.0).Equals("36N50"));
            Assert.IsTrue(To5DigitFormat(36.0, 150.0).Equals("36E50"));
            Assert.IsTrue(To5DigitFormat(-36.0, -150.0).Equals("36W50"));
            Assert.IsTrue(To5DigitFormat(-36.0, 150.0).Equals("36S50"));
        }
    }
}
