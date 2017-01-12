using NUnit.Framework;
using QSP.TOPerfCalculation.Boeing.PerfData;
using static QSP.TOPerfCalculation.Boeing.PerfData.AlternateThrustTable;

namespace UnitTest.TOPerfCalculation.Boeing.PerfData
{
    [TestFixture]
    public class AlternateThrustTableTest
    {
        private const double delta = 1E-7;

        private AlternateThrustTable table = new AlternateThrustTable(
            new[] { 200.0, 250.0 },
            new[] { 190.0, 240.0 },
            new[] { 192.0, 242.0 },
            new[] { 180.0, 230.0 });

        [Test]
        public void CorrectedLimitWeightTest()
        {
            Assert.AreEqual(202.5,
                table.CorrectedLimitWeight(212.5, TableType.Dry),
                delta);

            Assert.AreEqual(204.5,
                table.CorrectedLimitWeight(212.5, TableType.Wet),
                delta);

            Assert.AreEqual(192.5,
                table.CorrectedLimitWeight(212.5, TableType.Climb),
                delta);
        }

        [Test]
        public void EquivalentFullThrustWeightTest()
        {
            Assert.AreEqual(212.5,
                table.EquivalentFullThrustWeight(202.5, TableType.Dry),
                delta);

            Assert.AreEqual(212.5,
                table.EquivalentFullThrustWeight(204.5, TableType.Wet),
                delta);

            Assert.AreEqual(212.5,
                table.EquivalentFullThrustWeight(192.5, TableType.Climb),
                delta);
        }
    }
}
