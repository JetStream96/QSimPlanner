using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.Format5Letter;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class Format5LetterTest
    {
        [Test]
        public void WhenFormatIsWrongReturnNull()
        {
            Assert.IsNull(Parse("35U055E"));
            Assert.IsNull(Parse("35S155S"));
            Assert.IsNull(Parse("35S15AW"));
            Assert.IsNull(Parse("35S155"));
            Assert.IsNull(Parse("35N200E"));
        }
        
        [Test]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(Parse("36N50").Equals(new LatLon(36.0, -150.0)));
            Assert.IsTrue(Parse("4334E").Equals(new LatLon(43.0, 34.0)));
            Assert.IsTrue(Parse("55S13").Equals(new LatLon(-55.0, 113.0)));
            Assert.IsTrue(Parse("55W10").Equals(new LatLon(-55.0, -110.0)));
            Assert.IsTrue(Parse("05W05").Equals(new LatLon(-5.0, -105.0)));
        }

        [Test]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(To5LetterFormat(36.0, -150.0).Equals("36N50"));
            Assert.IsTrue(To5LetterFormat(36.0, 150.0).Equals("36E50"));
            Assert.IsTrue(To5LetterFormat(-36.0, -150.0).Equals("36W50"));
            Assert.IsTrue(To5LetterFormat(-36.0, 150.0).Equals("36S50"));
        }
    }
}
