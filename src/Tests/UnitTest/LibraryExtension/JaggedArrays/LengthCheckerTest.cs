using System;
using NUnit.Framework;
using QSP.LibraryExtension.JaggedArrays;

namespace UnitTest.LibraryExtension.JaggedArrays
{
    [TestFixture]
    public class LengthCheckerTest
    {
        [Test]
        public void HasLength1DShouldReturnTrue()
        {
            Assert.IsTrue(LengthChecker.HasLength<double>(new double[10], 10));

            Assert.IsFalse(LengthChecker.HasLength<double>(new double[10], 11));
        }

        [Test]
        public void HasLength1DWrongLength()
        {
            Assert.Throws<ArgumentException>(() =>
            LengthChecker.HasLength<double>(new double[10], 10, 10));
        }

        [Test]
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
