using NUnit.Framework;
using QSP.MathTools.Tables;
using System;

namespace UnitTest.MathTools.Tables
{
    [TestFixture]
    public class Table2DTest
    {
        private double[][] f = new[]
            {
                new[] {-5.0,3.5,4.5},
                new[] {-8.0,1.0,2.8},
                new[] {-16.0,0.4,0.8}
            };

        private double[] x = { 3.0, 4.0, 5.0 };
        private double[] y = { 8.0, 7.5, 1.35 };

        [Test]
        public void ValidateTest()
        {
            var table = new Table2D(x, y, f);
            table.Validate();
        }

        [Test]
        public void TableTooSmallNotValid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var table = new Table2D(x, y, new[] { f[0], f[1] });
                table.Validate();
            });
        }
    }
}
