using NUnit.Framework;
using static QSP.AviationTools.RwyIdentConversion;

namespace UnitTest.AviationTools
{
    [TestFixture]
    public class RwyIdentConversionTest
    {
        [Test]
        public void RwyIdentOppositeDirTest()
        {
            Assert.AreEqual("18", RwyIdentOppositeDir("36"));
            Assert.AreEqual("15", RwyIdentOppositeDir("33"));
            Assert.AreEqual("19", RwyIdentOppositeDir("01"));
            Assert.AreEqual("23L", RwyIdentOppositeDir("05R"));
        }
    }
}
