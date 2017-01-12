using NUnit.Framework;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace UnitTest.TOPerfCalculation.Boeing.PerfData
{
    [TestFixture]
    public class SlopeCorrTableTest
    {
        private const double delta = 1E-7;

        private SlopeCorrTable table = new SlopeCorrTable(
            new[] { 2000.0, 2400.0 },
            new[] { 0.0, 1.0 },
            new[] 
            {
                new[] { 2000.0, 1900.0 },
                new[] { 2400.0, 2200.0 }
            });

        [Test]
        public void CorrectedLengthTest()
        {
            Assert.AreEqual(2068.75, table.CorrectedLength(2100.0, 0.25), delta);
        }

        [Test]
        public void FieldLengthRequiredTest()
        {
            Assert.AreEqual(2100.0, table.FieldLengthRequired(0.25, 2068.75), delta);
        }
    }
}
