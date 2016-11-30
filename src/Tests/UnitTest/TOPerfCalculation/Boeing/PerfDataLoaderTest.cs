using NUnit.Framework;
using QSP.TOPerfCalculation.Boeing;
using System.Linq;
using System.Xml.Linq;

namespace UnitTest.TOPerfCalculation.Boeing
{
    [TestFixture]
    public class PerfDataLoaderTest
    {
        private const double delta = 1E-7;

        [Test]
        public void ReadTableTest()
        {
            var data = new TestData();
            var allTables = new PerfDataLoader().ReadTable(
                 XDocument.Parse(data.PerfXml).Root);

            Assert.AreEqual(1, allTables.Flaps.Count);

            var table = allTables.Tables[0];

            Assert.AreEqual(500.0, table.PacksOffDry, delta);
            Assert.AreEqual(500.0, table.PacksOffWet, delta);
            Assert.AreEqual(1700.0, table.PacksOffClimb, delta);
            Assert.AreEqual(0.0, table.AIEngDry, delta);
            Assert.AreEqual(0.0, table.AIEngWet, delta);
            Assert.AreEqual(0.0, table.AIEngClimb, delta);
            Assert.AreEqual(2050.0, table.AIBothDry, delta);
            Assert.AreEqual(2200.0, table.AIBothWet, delta);
            Assert.AreEqual(2100.0, table.AIBothClimb, delta);
            Assert.IsTrue(table.Flaps == "5");
            Assert.IsTrue(table.AltnRatingAvail);
            Assert.AreEqual(2, table.AlternateThrustTables.Count);

            Assert.IsTrue(table.AlternateThrustTables[0].Equals(
                data.AltnThrustTables[0], delta));
            Assert.IsTrue(table.AlternateThrustTables[1].Equals(
                data.AltnThrustTables[1], delta));

            Assert.IsTrue(Enumerable.SequenceEqual(table.ThrustRatings,
                new string[] { "TO", "TO1", "TO2" }));

            Assert.IsTrue(table.SlopeCorrDry.Equals(data.SlopeCorrDry, delta));
            Assert.IsTrue(table.SlopeCorrWet.Equals(data.SlopeCorrWet, delta));
            Assert.IsTrue(table.WindCorrDry.Equals(data.WindCorrDry, delta));
            Assert.IsTrue(table.WindCorrWet.Equals(data.WindCorrWet, delta));
            Assert.IsTrue(table.WeightTableDry.Equals(data.WtTableDry, delta));
            Assert.IsTrue(table.WeightTableWet.Equals(data.WtTableWet, delta));
            Assert.IsTrue(table.ClimbLimitWt.Equals(data.ClimbLimTable, delta));
        }
    }
}
