using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension.JaggedArrays;

namespace UnitTest.LibraryExtension.JaggedArrays
{
    [TestClass]
    public class LengthCheckerTest
    {
        [TestMethod]
        public void HasLength1DShouldReturnTrue()
        {
            Assert.IsTrue(LengthChecker.HasLength<double>(
                new double[10], 10));

            Assert.IsFalse(LengthChecker.HasLength<double>(
                new double[10], 11));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HasLength1DWrongLength()
        {
            Assert.IsFalse(LengthChecker.HasLength<double>(
                new double[10], 10, 10));
        }

        [TestMethod]
        public void HasLengthHigherDimensionTest()
        {
            Assert.IsTrue(LengthChecker.HasLength<double>(
               JaggedArray.Create<double[][]>(8, 9),
               8, 9));

            Assert.IsTrue(LengthChecker.HasLength<double>(
                JaggedArray.Create<double[][][]>(3, 4, 5),
                3, 4, 5));
        }
    }
}
