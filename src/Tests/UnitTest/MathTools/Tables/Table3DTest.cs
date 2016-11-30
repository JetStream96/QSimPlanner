using NUnit.Framework;
using QSP.MathTools.Tables;
using System;

namespace UnitTest.MathTools.Tables
{
    [TestFixture]
    public class Table3DTest
    {
        private double[][][] f =
        {
            new[] {
                new[] {-5.0,3.5,4.5},
                new[] {-8.0,1.0,2.8},
                new[] {-16.0,0.4,0.8}
            },
            new[]
            {
                new[] {2.0,18.5,14.5},
                new[] {-0.8,14.0,12.8},
                new[] {-1.0,11.0,4.8}
            }
        };

        private double[] x = { 3.0, 4.0 };
        private double[] y = { 8.0, 7.5, 1.35 };
        private double[] z = { 3.0, 4.0, 5.0 };

        [Test]
        public void ValidateTest()
        {
            var table = new Table3D(x, y, z, f);
            table.Validate();
        }

        [Test]
        public void TableTooSmallNotValid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var table = new Table3D(x, y, z, new[] { f[0] });
                table.Validate();
            });
        }
    }
}
