using System;
using NUnit.Framework;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class DoubleArraysTest
    {
        [Test]
        public void StrictlyIncreasingTest()
        {
            Assert.IsTrue(new double[] { 3.0, 4.0 }
            .IsStrictlyIncreasing());

            Assert.IsTrue(new double[] { 3.0, 4.0, 5.0 }
            .IsStrictlyIncreasing());

            Assert.IsFalse(new double[] { 3.0, 2.0, 1.0 }
            .IsStrictlyIncreasing());
        }

        [Test]
        public void OneElementAlwaysStrictlyIncreasing()
        {
            Assert.IsTrue(new double[] { 3.0 }
            .IsStrictlyIncreasing());
        }

        [Test]
        public void EmptyArrayIsIncreasingThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
            new double[] { }.IsStrictlyIncreasing());
        }

        [Test]
        public void StrictlyDecreasingTest()
        {
            Assert.IsTrue(new double[] { 3.0, 2.0 }
            .IsStrictlyDecreasing());

            Assert.IsTrue(new double[] { 3.0, 2.0, 1.0 }
            .IsStrictlyDecreasing());

            Assert.IsFalse(new double[] { 3.0, 4.0, 5.0 }
            .IsStrictlyDecreasing());
        }

        [Test]
        public void OneElementAlwaysStrictlyDecreasing()
        {
            Assert.IsTrue(new double[] { 3.0 }
            .IsStrictlyDecreasing());
        }

        [Test]
        public void EmptyArrayIsDecreasingThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
            new double[] { }.IsStrictlyDecreasing());
        }
    }
}
