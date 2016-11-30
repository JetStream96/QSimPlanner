using NUnit.Framework;
using QSP.MathTools.TablesNew;
using System;

namespace UnitTest.MathTools
{
    [TestFixture]
    public class TableTest
    {
        [Test]
        public void ConstructorValidInputShouldPass()
        {
            double[] xValues = { 3.0, 5.0, 7.0, 10.0 };
            double[] fValues = { -6.0, -30.0, 15.0, 8.0 };
            TableBuilder.Build1D(xValues, fValues);
        }

        [Test]
        public void ConstructorTooShortFValue()
        {
            double[] xValues = { 3.0, 5.0, 7.0, 10.0 };
            double[] fValues = { -6.0, -30.0 };
            Assert.Throws<ArgumentException>(() => TableBuilder.Build1D(xValues, fValues));
        }

        [Test]
        public void ConstructorXValuesNotIncreasingNorDecreasing()
        {
            double[] xValues = { 3.0, 5.0, 7.0, -10.0 };
            double[] fValues = { -6.0, -30.0, 15.0, 8.0 };
            Assert.Throws<ArgumentException>(() => TableBuilder.Build1D(xValues, fValues));
        }

        [Test]
        public void ConstructorFValuesInconsistentDimension()
        {
            double[] xValues = { 3.0, 5.0 };
            ITable[] fValues = { 8.0.Wrap(), Get1DTable() };
            Assert.Throws<ArgumentException>(() => new Table(xValues, fValues));
        }

        private static Table Get1DTable()
        {
            return new Table(
                new[] { 3.0, 5.0, 7.0, 10.0 },
                new[] { -6.0, -30.0, 15.0, 8.0 }.WrapperList());
        }

        [Test]
        public void InterpolateTest1D()
        {
            double[] xValues = { 3.0, 5.0, 7.0, 9.0 };
            double[] fValues = { -9.0, -1.0, 12.0, 14.0 };
            var table = TableBuilder.Build1D(xValues, fValues);
            Assert.AreEqual(12.2, table.ValueAt(7.2), 1E-10);
        }

        [Test]
        public void InterpolateTest2D()
        {
            double[] xValues = { 0.0, 2.0, 4.0 };
            double[] yValues = { 0.0, 3.0, 6.0 };
            double[][] fValues =
            {
               new[]{ 0.0, 0.0, 0.0},
               new[]{ 0.0, 6.0, 12.0 },
               new[]{ 0.0, 12.0, 24.0 }
            };

            var table = TableBuilder.Build2D(xValues, yValues, fValues);

            Assert.AreEqual(6.0, table.ValueAt(3.0, 2.0), 1E-10);
        }
    }
}