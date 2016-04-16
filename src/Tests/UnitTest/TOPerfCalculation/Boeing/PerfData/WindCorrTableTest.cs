using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace UnitTest.TOPerfCalculation.Boeing.PerfData
{
    [TestClass]
    public class WindCorrTableTest
    {
        private const double delta = 1E-7;

        private WindCorrTable table = new WindCorrTable(
                new double[] { 2000.0, 2400.0 },
                new double[] { 0.0, 10.0 },
                new double[][] {
                    new double[] { 2000.0, 2100.0 },
                    new double[] { 2400.0, 2600.0 }
                });

        [TestMethod]
        public void CorrectedLengthTest()
        {
            Assert.AreEqual(2131.25, table.CorrectedLength(2100.0, 2.5), delta);
        }

        [TestMethod]
        public void SlopeCorrectedLengthTest()
        {
            Assert.AreEqual(2100.0, table.SlopeCorrectedLength(2.5, 2131.25), delta);
        }
    }
}
