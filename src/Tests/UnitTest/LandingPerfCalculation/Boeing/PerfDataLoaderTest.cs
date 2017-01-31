using NUnit.Framework;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.IEnumerables;

namespace UnitTest.LandingPerfCalculation.Boeing
{
    [TestFixture]
    public class PerfDataLoaderTest
    {
        private const double delta = 1E-7;

        [Test]
        public void GetItemTest()
        {
            string text = new TestData().AllText;
            var doc = XDocument.Parse(text);
            var table = new PerfDataLoader().GetItem(doc);

            Assert.AreEqual(table.WeightRef, 50000.0, delta);
            Assert.AreEqual(table.WeightStep, 5000.0, delta);

            Assert.IsTrue(table.BrakesAvailable(SurfaceCondition.Dry).SequenceEqual(
                "MAX MANUAL", "MAX AUTO"));

            Assert.IsTrue(table.BrakesAvailable(SurfaceCondition.Good).SequenceEqual(
                "MAX MANUAL"));

            Assert.IsTrue(table.Flaps.SequenceEqual("30", "40"));
            Assert.IsTrue(table.Reversers.SequenceEqual("Both", "One Rev", "No Rev"));

            Assert.IsTrue(table.DataDry.Equals(new TableDry(
                new[]
                {
                    new[]
                    {
                        new[] { 750, 55, -40, 15, 15, -30, 95, 5, -5, 15, -15, 55, 10, 25.0 },
                        new[] { 955, 60, -60, 20, 20, -35, 125, 0, 0, 20, -20, 95, 0, 0.0 }
                    },
                    new[]
                    {
                        new[] { 750,55,-40,15,15,-30,100, 10,-5,15,-15,60,10,25.0 },
                        new[] { 930, 55, -55, 20, 20, -35, 125, 0, 0, 20, -20, 95, 0, 0.0 }
                    }
                }),
                delta));

            Assert.IsTrue(table.DataWet.Equals(new TableWet(
                new[]
                {
                    new[]
                    {
                        new[]
                        {
                            new[] { 1200, 85, -80, 30, 30, -55,205, 30, -20, 30, -25, 100, 65, 145.0 }
                        },
                        new[]
                        {
                            new[] { 1595, 130, -120, 50, 50, -85, 335, 70, -50, 45, -40, 130, 175, 430.0 }
                        },
                        new[]
                        {
                            new[] { 2045, 185, -170, 65, 65, -130, 525, 170, -105, 55, -50, 150, 370, 1030.0 }
                        }
                    },
                    new[]
                    {
                        new[]
                        {
                            new[] { 1185, 85, -70, 30, 30, -55, 205, 30, -20, 30, -25, 100, 65, 140.0 }
                        },
                        new[]
                        {
                            new[] { 1565, 130, -120, 45, 45, -85, 330, 70, -50, 45, -40, 130, 165, 405.0 }
                        },
                        new[]
                        {
                            new[] { 2005, 180, -165, 60, 60, -130, 525, 165, -105, 55, -50, 150, 345, 945.0 }
                        }
                    }
                }),
                delta));

        }
    }
}
