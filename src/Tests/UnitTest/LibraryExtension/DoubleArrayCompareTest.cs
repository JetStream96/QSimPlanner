using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestClass]
    public class DoubleArrayCompareTest
    {
        private const double delta = 1E-10;

        [TestMethod]
        public void OneDimArrayCompareTest()
        {
            double[] a = new double[] { 1.0, 2.0, 3.0 };
            double[] b = new double[] { 1.0, 2.0, 3.0 };
            double[] c = new double[] { 3.0, 2.0, 1.0 };
            double[] d = new double[] { 1.0 + 5E-11, 2.0, 3.0 };
            double[] e = null;
            double[] f = new double[] { 1.0, 2.0 };

            Assert.IsTrue(DoubleArrayCompare.Equals(a, b, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, c, delta));
            Assert.IsTrue(DoubleArrayCompare.Equals(a, d, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, e, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, f, delta));
        }

        [TestMethod]
        public void TwoDimArrayCompareTest()
        {
            double[][] a = new double[][] {
                new double[] { 1.0,2.0,3.0},
                new double[] {8.0,-5.0,1.5} };

            double[][] b = new double[][] {
                new double[] { 1.0,2.0,3.0},
                new double[] {8.0,-5.0,1.5} };

            double[][] c = new double[][] {
                new double[] { 10.0,12.0,13.0},
                new double[] {8.0,-5.0,1.5} };

            Assert.IsTrue(DoubleArrayCompare.Equals(a, b, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, c, delta));
        }

        [TestMethod]
        public void ThreeDimArrayCompareTest()
        {
            var a =
                new double[][][] {
                    new double[][]
                    {
                        new double[] { 1.0, 2.0},
                        new double[] { 8.0, -5.0}
                    },
                    new double[][]
                    {
                        new double[] { 5.0, 6.0},
                        new double[] { 7.0, 8.0}
                    }
                };

            var b =
                new double[][][] {
                    new double[][]
                    {
                        new double[] { 1.0, 2.0},
                        new double[] { 8.0, -5.0}
                    },
                    new double[][]
                    {
                        new double[] { 5.0, 6.0},
                        new double[] { 7.0, 8.0}
                    }
                };

            var c =
                 new double[][][] {
                    new double[][]
                    {
                        new double[] { 1.0, 2.0},
                        new double[] { 8.0, -5.0}
                    },
                    new double[][]
                    {
                        new double[] { 5.0, 6.0},
                        new double[] { 0.0, 0.0}
                    }
                 };

            Assert.IsTrue(DoubleArrayCompare.Equals(a, b, delta));
            Assert.IsFalse(DoubleArrayCompare.Equals(a, c, delta));
        }
    }
}
