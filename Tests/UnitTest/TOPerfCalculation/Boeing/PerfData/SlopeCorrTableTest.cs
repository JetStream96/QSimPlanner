using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace UnitTest.TOPerfCalculation.Boeing.PerfData
{
    [TestClass]
    public class SlopeCorrTableTest
    {
        private const double delta = 1E-7;

        private SlopeCorrTable table = new SlopeCorrTable(
                new double[] { 2000.0, 2400.0 },
                new double[] { 0.0, 1.0 },
                new double[][] {
                    new double[] { 2000.0, 1900.0 },
                    new double[] { 2400.0, 2200.0 }
                });

        [TestMethod]
        public void CorrectedLengthTest()
        {
            Assert.AreEqual(2068.75, table.CorrectedLength(2100.0, 0.25), delta);
        }

        [TestMethod]
        public void FieldLengthRequiredTest()
        {
            Assert.AreEqual(2100.0, table.FieldLengthRequired(0.25, 2068.75), delta);
        }
    }
}
