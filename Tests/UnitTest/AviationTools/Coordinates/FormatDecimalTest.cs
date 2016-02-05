using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.AviationTools.Coordinates.FormatDecimal;

namespace UnitTest.AviationTools.Coordinates
{
    [TestClass]
    public class FormatDecimalTest
    {
        [TestMethod()]
        public void WhenFormatIsWrongReturnFalse()
        {
            LatLon r;

            Assert.IsFalse(TryReadFromDecimalFormat("U123.01321W15.2", out r));
            Assert.IsFalse(TryReadFromDecimalFormat("N15.51656EW1532.0", out r));
            Assert.IsFalse(TryReadFromDecimalFormat("ABCDE", out r));
            Assert.IsFalse(TryReadFromDecimalFormat("N100.00E35.265", out r));
            Assert.IsFalse(TryReadFromDecimalFormat("S35.11W200.22", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectReturnTrue()
        {
            LatLon r;

            Assert.IsTrue(TryReadFromDecimalFormat("N31.54E55.896", out r));
            Assert.IsTrue(TryReadFromDecimalFormat("N31.54W55.896", out r));
            Assert.IsTrue(TryReadFromDecimalFormat("S31.54E55.896", out r));
            Assert.IsTrue(TryReadFromDecimalFormat("S31.54W55.896", out r));
        }

        [TestMethod()]
        public void WhenFormatIsCorrectConvert()
        {
            Assert.IsTrue(ReadFromDecimalFormat("N31.54E55.896").Equals(new LatLon(31.54, 55.896)));
            Assert.IsTrue(ReadFromDecimalFormat("N31.54W55.896").Equals(new LatLon(31.54, -55.896)));
            Assert.IsTrue(ReadFromDecimalFormat("S31.54E55.896").Equals(new LatLon(-31.54, 55.896)));
            Assert.IsTrue(ReadFromDecimalFormat("S31.54W55.896").Equals(new LatLon(-31.54,-55.896)));
        }

        [TestMethod]
        public void OutputStringAsExpected()
        {
            Assert.IsTrue(ToDecimalFormat(36.0, -150.0).Equals("N36.000000W150.000000"));
            Assert.IsTrue(ToDecimalFormat(36.0, 150.0).Equals("N36.000000E150.000000"));
            Assert.IsTrue(ToDecimalFormat(-36.0, -150.0).Equals("S36.000000W150.000000"));
            Assert.IsTrue(ToDecimalFormat(-36.0, 150.0).Equals("S36.000000E150.000000"));
        }
    }
}
