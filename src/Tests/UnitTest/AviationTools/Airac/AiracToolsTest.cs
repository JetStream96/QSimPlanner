using System;
using NUnit.Framework;
using QSP.AviationTools.Airac;

namespace UnitTest.AviationTools.Airac
{
    [TestFixture]
    public class AiracToolsTest
    {
        [Test]
        public void IncorrectAiracFormatShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => AiracTools.ParsePeriod("6JUN23JUL/14"));
        }

        [Test]
        public void ParseAiracPeriodTest()
        {
            var p = AiracTools.ParsePeriod("26JUN23JUL/14");
            var s = p.Start;
            var e = p.End;
            Assert.IsTrue(s.Year == 2014 && s.Month == 6 && s.Day == 26);
            Assert.IsTrue(e.Year == 2014 && e.Month == 7 && e.Day == 23);
        }

        [Test]
        public void ParseAiracPeriodCrossYear()
        {
            var p = AiracTools.ParsePeriod("08DEC05JAN/16");
            var s = p.Start;
            var e = p.End;
            Assert.IsTrue(s.Year == 2016 && s.Month == 12 && s.Day == 8);
            Assert.IsTrue(e.Year == 2017 && e.Month == 1 && e.Day == 5);
        }
    }
}