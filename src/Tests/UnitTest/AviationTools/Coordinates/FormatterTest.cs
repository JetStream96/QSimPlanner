using NUnit.Framework;
using static QSP.AviationTools.Coordinates.Formatter;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class FormatterTest
    {
        [Test]
        public void TryTransformCoordinateTest()
        {
            Assert.AreEqual("36N70", TryTransformCoordinate("36N170W"));
            Assert.AreEqual("S21.000000E135.500000", TryTransformCoordinate("2100S13530E"));
        }

        [Test]
        public void TryTransformCoordinateInputNotCoordinate()
        {
            Assert.AreEqual("HLG", TryTransformCoordinate("HLG"));
        }
    }
}