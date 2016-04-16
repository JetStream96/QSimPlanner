using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Format7Letter;

namespace UnitTest.AviationTools.Coordinates
{
    [TestClass]
    public class Format7LetterTest
    {
        [TestMethod()]
        public void WhenFormatIsWrongReturnFalse()
        {
            LatLon r;

            Assert.IsFalse(TryReadFrom7LetterFormat("37U055E", out r));
            Assert.IsFalse(TryReadFrom7LetterFormat("37S155S", out r));
            Assert.IsFalse(TryReadFrom7LetterFormat("37S15AW", out r));
            Assert.IsFalse(TryReadFrom7LetterFormat("37S155", out r));
            Assert.IsFalse(TryReadFrom7LetterFormat("37N200E", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectReturnTrue()
        {
            LatLon r;

            Assert.IsTrue(TryReadFrom7LetterFormat("37N055E", out r));
            Assert.IsTrue(TryReadFrom7LetterFormat("37S155W", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(ReadFrom7LetterFormat("36N170W").Equals(new LatLon(36.0, -170.0)));
            Assert.IsTrue(ReadFrom7LetterFormat("43N034E").Equals(new LatLon(43.0, 34.0)));
            Assert.IsTrue(ReadFrom7LetterFormat("57S113E").Equals(new LatLon(-57.0, 113.0)));
            Assert.IsTrue(ReadFrom7LetterFormat("57S110W").Equals(new LatLon(-57.0, -110.0)));
        }

        [TestMethod]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(To7LetterFormat(36.0, -170.0).Equals("36N170W"));
            Assert.IsTrue(To7LetterFormat(36.0, 170.0).Equals("36N170E"));
            Assert.IsTrue(To7LetterFormat(-36.0, -170.0).Equals("36S170W"));
            Assert.IsTrue(To7LetterFormat(-36.0, 170.0).Equals("36S170E"));
            Assert.IsTrue(To7LetterFormat(-6.0, 7.0).Equals("06S007E"));
        }
    }
}
