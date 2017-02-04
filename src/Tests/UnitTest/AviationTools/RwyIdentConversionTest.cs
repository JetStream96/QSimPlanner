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

        [Test]
        public void OppositeDirInvalidInput()
        {
            Assert.IsNull(RwyIdentOppositeDir("#"));
            Assert.IsNull(RwyIdentOppositeDir("18X"));
            Assert.IsNull(RwyIdentOppositeDir("18X"));
            Assert.IsNull(RwyIdentOppositeDir("3L")); // Should be 03L
        }
    }
}
