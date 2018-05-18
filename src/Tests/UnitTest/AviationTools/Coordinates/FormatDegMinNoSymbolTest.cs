using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.FormatDegMinNoSymbol;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class FormatDegMinNoSymbolTest
    {
        [Test]
        public void ToFormatDegMinNoSymbolTest()
        {
            Assert.AreEqual("2100S13530E", FormatDegMinNoSymbol.ToString( new LatLon(-21, 135.5), 0));
            Assert.AreEqual("2121.40S13521.19E", FormatDegMinNoSymbol.ToString(new LatLon(-21.3567, 135.3531), 2));
        }

        [Test]
        public void ParseTest()
        {
            Assert.AreEqual(Parse("2100S13530E"), new LatLon(-21, 135.5));
            Assert.AreEqual(Parse("2100N13530W"), new LatLon(21, -135.5));
            Assert.AreEqual(Parse("2100.8N13530W"), new LatLon(21 + 0.8 / 60.0, -135.5));
            Assert.AreEqual(Parse("2100N03530W"), new LatLon(21, -35.5));
        }

        [Test]
        public void ParseInvalidInputTest()
        {
            Assert.IsNull(Parse("35sa"));
            Assert.IsNull(Parse("2100S53530E"));
        }
    }
}