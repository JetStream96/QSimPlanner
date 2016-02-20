using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.LibraryExtension.JaggedArrays;

namespace UnitTest.LibraryExtension
{
    [TestClass]
    public class JaggedArraysTest
    {
        [TestMethod]
        public void JaggedArray1DCreateTest()
        {
            var a = CreateJaggedArray<int[]>(3);

            Assert.AreEqual(3, a.Length);
        }

        [TestMethod]
        public void JaggedArray3DCreateTest()
        {
            var a = CreateJaggedArray<double[][][]>(3, 4, 5);

            Assert.AreEqual(3, a.Length);
            Assert.IsTrue(a.GetType() == typeof(double[][]));

            Assert.AreEqual(4, a[0].Length);
            Assert.IsTrue(a[0].GetType() == typeof(double[]));

            Assert.AreEqual(5, a[0][0].Length);
            Assert.IsTrue(a[0][0].GetType() == typeof(double));
        }

        [TestMethod]
        public void JaggedArrayMultiplyTest()
        {
            double[] a = { 1, 2, 3, 4 };
            double[] b = { 8, 9, 10, 11 };
            double[][] c = { a, b };

            double[] a1 = { 1, 2, 3, 4 };
            double[] b1 = { 8, 9, 10, 11 };
            double[][] c1 = { a1, b1 };

            double[][][] d = { c, c1 };
            d.Multiply(3.0);

            Assert.AreEqual(24.0, d[1][1][0], 1E-6);
            Assert.AreEqual(9.0, d[0][0][2], 1E-6);
            Assert.AreEqual(33.0, d[0][1][3], 1E-6);
        }
    }
}
