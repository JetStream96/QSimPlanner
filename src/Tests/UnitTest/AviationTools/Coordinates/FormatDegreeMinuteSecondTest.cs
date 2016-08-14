using NUnit.Framework;
using static QSP.AviationTools.Coordinates.FormatDegreeMinuteSecond;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class FormatDegreeMinuteSecondTest
    {
        [Test]
        public void CustomFormatTest()
        {
            var result = ToDegreeMinuteSecondFormat(25.073133333, "F2");
            Assert.IsTrue("25° 4' 23.28\"" == result);
        }
    }
}
