using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Format5Letter;

namespace Tests.AviationTools.Coordinates
{
    [TestClass]
    public class Format5LetterTest
    {
        [TestMethod()]
        public void WhenFormatIsWrongReturnFalse()
        {
            LatLon r;

            Assert.IsFalse(TryReadFrom5LetterFormat("35U055E", out r));
            Assert.IsFalse(TryReadFrom5LetterFormat("35S155S", out r));
            Assert.IsFalse(TryReadFrom5LetterFormat("35S15AW", out r));
            Assert.IsFalse(TryReadFrom5LetterFormat("35S155", out r));
            Assert.IsFalse(TryReadFrom5LetterFormat("35N200E", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectReturnTrue()
        {
            LatLon r;

            Assert.IsTrue(TryReadFrom5LetterFormat("35N00", out r));
            Assert.IsTrue(TryReadFrom5LetterFormat("35E50", out r));
            Assert.IsTrue(TryReadFrom5LetterFormat("35W50", out r));
            Assert.IsTrue(TryReadFrom5LetterFormat("35S50", out r));

            Assert.IsTrue(TryReadFrom5LetterFormat("8565N", out r));
            Assert.IsTrue(TryReadFrom5LetterFormat("8565S", out r));
            Assert.IsTrue(TryReadFrom5LetterFormat("8565E", out r));
            Assert.IsTrue(TryReadFrom5LetterFormat("8565W", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(ReadFrom5LetterFormat("36N50").Equals(new LatLon(36.0, -150.0)));
            Assert.IsTrue(ReadFrom5LetterFormat("4334E").Equals(new LatLon(43.0, 34.0)));
            Assert.IsTrue(ReadFrom5LetterFormat("55S13").Equals(new LatLon(-55.0, 113.0)));
            Assert.IsTrue(ReadFrom5LetterFormat("55W10").Equals(new LatLon(-55.0, -110.0)));
            Assert.IsTrue(ReadFrom5LetterFormat("05W05").Equals(new LatLon(-5.0, -105.0)));
        }

        [TestMethod]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(To5LetterFormat(36.0, -150.0).Equals("36N50"));
            Assert.IsTrue(To5LetterFormat(36.0, 150.0).Equals("36E50"));
            Assert.IsTrue(To5LetterFormat(-36.0, -150.0).Equals("36W50"));
            Assert.IsTrue(To5LetterFormat(-36.0, 150.0).Equals("36S50"));
        }
    }
}
