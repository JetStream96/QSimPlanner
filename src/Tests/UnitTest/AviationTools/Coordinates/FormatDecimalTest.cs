using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.FormatDecimal;

namespace UnitTest.AviationTools.Coordinates
{
    [TestFixture]
    public class FormatDecimalTest
    {
        [Test]
        public void WhenFormatIsWrongReturnNull()
        {
            Assert.IsNull(Parse("U123.01321W15.2"));
            Assert.IsNull(Parse("N15.51656EW1532.0"));
            Assert.IsNull(Parse("ABCDE"));
            Assert.IsNull(Parse("N100.00E35.265"));
            Assert.IsNull(Parse("S35.11W200.22"));
        }
        
        [Test]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(Parse("N31.54E55.896").Equals(new LatLon(31.54, 55.896)));
            Assert.IsTrue(Parse("N31.54W55.896").Equals(new LatLon(31.54, -55.896)));
            Assert.IsTrue(Parse("S31.54E55.896").Equals(new LatLon(-31.54, 55.896)));
            Assert.IsTrue(Parse("S31.54W55.896").Equals(new LatLon(-31.54,-55.896)));
        }

        [Test]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(ToDecimalFormat(36.0, -150.0).Equals("N36.000000W150.000000"));
            Assert.IsTrue(ToDecimalFormat(36.0, 150.0).Equals("N36.000000E150.000000"));
            Assert.IsTrue(ToDecimalFormat(-36.0, -150.0).Equals("S36.000000W150.000000"));
            Assert.IsTrue(ToDecimalFormat(-36.0, 150.0).Equals("S36.000000E150.000000"));
        }
    }
}
