using NUnit.Framework;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class FormatDegreeMinuteTest
    {
        [Test]
        public void CustomFormatTest()
        {
            var result = QSP.AviationTools.Coordinates.FormatDegreeMinute.ToString(25.073133333, "F2");
            Assert.IsTrue("25° 4.39'" == result);
        }
    }
}
