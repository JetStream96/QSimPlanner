using System.Linq;
using NUnit.Framework;
using QSP.MathTools.Tables;

namespace UnitTest.MathTools.Tables
{
    [TestFixture]
    public class TableUtilTest
    {
        [Test]
        public void Truncate1Element()
        {
            double[] x = { 1.0 };
            double[] f = { -5.0 };

            var table = new Table1D(x, f).TruncateInvalidXValues();

            Assert.IsTrue(table.x.SequenceEqual(x));
            Assert.IsTrue(table.f.SequenceEqual(f));
        }

        [Test]
        public void TruncateIncreasingX()
        {
            double[] x = { 1.0, 2.0, 3.0, 3.0 };
            double[] f = { -5.0, 12.0, -9.0, 6.0 };

            var table = new Table1D(x, f).TruncateInvalidXValues();

            Assert.IsTrue(table.x.SequenceEqual(x.Take(3)));
            Assert.IsTrue(table.f.SequenceEqual(f.Take(3)));
        }

        [Test]
        public void TruncateDecreasingX()
        {
            double[] x = { -1.0, -2.0, -3.0, -3.0 };
            double[] f = { -5.0, 12.0, -9.0, 6.0 };

            var table = new Table1D(x, f).TruncateInvalidXValues();

            Assert.IsTrue(table.x.SequenceEqual(x.Take(3)));
            Assert.IsTrue(table.f.SequenceEqual(f.Take(3)));
        }

        [Test]
        public void TruncateRepeatedX()
        {
            double[] x = { 3.0, 3.0, 3.0 };
            double[] f = { -5.0, 12.0, -9.0 };

            var table = new Table1D(x, f).TruncateInvalidXValues();

            Assert.IsTrue(table.x.SequenceEqual(x.Take(1)));
            Assert.IsTrue(table.f.SequenceEqual(f.Take(1)));
        }
    }
}