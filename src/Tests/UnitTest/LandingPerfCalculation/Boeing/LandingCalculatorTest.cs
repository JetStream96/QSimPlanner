using NUnit.Framework;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System.Xml.Linq;

namespace UnitTest.LandingPerfCalculation.Boeing
{
    [TestFixture]
    public class LandingCalculatorTest
    {
        [Test]
        public void DistanceRequiredTest()
        {
            string text = new TestData().AllText;
            var doc = XDocument.Parse(text);
            var table = new PerfDataLoader().GetItem(doc);

            var para = new LandingParameters(
                55000.0,
                0.0,
                1000.0,
                -10.0,
                -1.0,
                15.0,
                1013.25,
                5.0,
                ReverserOption.NoRev,
                SurfaceCondition.Good,
                0,
                0);

            var dis = new LandingCalculator(table, para)
                        .DistanceRequiredMeter();

            double expectedDistance = 1200.0 + 85.0 + 30.0 + 205.0 +
                30.0 + 1.98 / 10.0 * 30.0 + 100.0 * 0.5 + 145.0;

            Assert.AreEqual(expectedDistance, dis, 1E-7);
        }
    }
}
