using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Format7Letter;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class Format7LetterTest
    {
        [Test]
        public void WhenFormatIsWrongReturnNull()
        {
            Assert.IsNull(Parse("37U055E"));
            Assert.IsNull(Parse("37S155S"));
            Assert.IsNull(Parse("37S15AW"));
            Assert.IsNull(Parse("37S155"));
            Assert.IsNull(Parse("37N200E"));
        }
        
        [Test]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(Parse("36N170W")
                .Equals(new LatLon(36.0, -170.0)));

            Assert.IsTrue(Parse("43N034E")
                .Equals(new LatLon(43.0, 34.0)));

            Assert.IsTrue(Parse("57S113E")
                .Equals(new LatLon(-57.0, 113.0)));

            Assert.IsTrue(Parse("57S110W")
                .Equals(new LatLon(-57.0, -110.0)));
        }

        [Test]
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
