using NUnit.Framework;
using QSP.AviationTools.Coordinates;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class FormatDegreeMinuteSecondTest
    {
        [Test]
        public void CustomFormatTest()
        {
            var result = FormatDegreeMinuteSecond.ToString(25.073133333, "F1");
            Assert.AreEqual("25° 4' 23.3\"" , result);
        }

        [Test]
        public void DefaultFormatTest0()
        {
            var result = FormatDegreeMinuteSecond.ToString(25.073133333);
            Assert.AreEqual("25° 4' 23.28\"", result);
        }

        [Test]
        public void DefaultFormatTest1()
        {
            var result = FormatDegreeMinuteSecond.ToString(25.20000001);
            Assert.AreEqual("25° 12' 0.00\"", result);
        }

        [Test]
        public void DefaultFormatTest2()
        {
            var result = FormatDegreeMinuteSecond.ToString(25.9999999999);
            Assert.AreEqual("26° 0' 0.00\"", result);
        }
    }
}
