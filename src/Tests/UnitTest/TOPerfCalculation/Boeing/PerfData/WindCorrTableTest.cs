using NUnit.Framework;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace UnitTest.TOPerfCalculation.Boeing.PerfData
{
    [TestFixture]
    public class WindCorrTableTest
    {
        private const double delta = 1E-7;

        private WindCorrTable table = new WindCorrTable(
            new[] { 2000.0, 2400.0 },
            new[] { 0.0, 10.0 },
            new[] 
            {
                new[] { 2000.0, 2100.0 },
                new[] { 2400.0, 2600.0 }
            });

        [Test]
        public void CorrectedLengthTest()
        {
            Assert.AreEqual(2131.25, table.CorrectedLength(2100.0, 2.5), delta);
        }

        [Test]
        public void SlopeCorrectedLengthTest()
        {
            Assert.AreEqual(2100.0, table.SlopeCorrectedLength(2.5, 2131.25), delta);
        }
    }
}
