using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestClass]
    public class DoubleArraysTest
    {
        [TestMethod]
        public void StrictlyIncreasingTest()
        {
            Assert.IsTrue(new double[] { 3.0, 4.0 }
            .IsStrictlyIncreasing());

            Assert.IsTrue(new double[] { 3.0, 4.0, 5.0 }
            .IsStrictlyIncreasing());

            Assert.IsFalse(new double[] { 3.0, 2.0, 1.0 }
            .IsStrictlyIncreasing());
        }

        [TestMethod]
        public void OneElementAlwaysStrictlyIncreasing()
        {
            Assert.IsTrue(new double[] { 3.0 }
            .IsStrictlyIncreasing());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyArrayIsIncreasingThrowException()
        {
            Assert.IsTrue(new double[] { }
            .IsStrictlyIncreasing());
        }

        [TestMethod]
        public void StrictlyDecreasingTest()
        {
            Assert.IsTrue(new double[] { 3.0, 2.0 }
            .IsStrictlyDecreasing());

            Assert.IsTrue(new double[] { 3.0, 2.0, 1.0 }
            .IsStrictlyDecreasing());

            Assert.IsFalse(new double[] { 3.0, 4.0, 5.0 }
            .IsStrictlyDecreasing());
        }

        [TestMethod]
        public void OneElementAlwaysStrictlyDecreasing()
        {
            Assert.IsTrue(new double[] { 3.0 }
            .IsStrictlyDecreasing());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyArrayIsDecreasingThrowException()
        {
            Assert.IsTrue(new double[] { }
            .IsStrictlyDecreasing());
        }
    }
}
