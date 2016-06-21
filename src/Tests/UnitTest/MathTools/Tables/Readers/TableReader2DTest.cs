using NUnit.Framework;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;

namespace UnitTest.MathTools.Tables.Readers
{
    [TestFixture]
    public class TableReader2DTest
    {
        private const double delta = 1E-7;

        private string format1 =
              @"
              -40 10 14 
              1500 51.2 47.0 46.7 
              1600 52.8 48.5 48.1
              1800 55.9 51.3 51.0 
              ";

        private string format2 =
              @"-40 10 14 
              1500 51.2 47.0 46.7
              1600 52.8 48.5 48.1
              1800 55.9 51.3 51.0";

        private string format3 =
              @"
              -40 10 14 
              1500 51.2 47.0 46.7
 
              1600 52.8 48.5 48.1
              1800 55.9 51.3 51.0";

        [Test]
        public void ReadTest1()
        {
            AssertTable(format1);
        }

        [Test]
        public void ReadTest2()
        {
            AssertTable(format2);
        }

        [Test]
        public void ReadTest3()
        {
            AssertTable(format3);
        }

        private void AssertTable(string source)
        {
            var table = TableReader2D.Read(source);

            var expected = new Table2D(
                new double[] { 1500.0, 1600.0, 1800.0 },
                new double[] { -40.0, 10.0, 14.0 },
                new double[][] {
                    new double[] {51.2, 47.0, 46.7},
                    new double[] {52.8, 48.5, 48.1},
                    new double[] {55.9, 51.3, 51.0}
                });

            Assert.IsTrue(table.Equals(expected, delta));
        }
    }
}
