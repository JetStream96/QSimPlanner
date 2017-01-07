using NUnit.Framework;
using QSP.LibraryExtension;
using System;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class DoubleArraysTest
    {
        [Test]
        public void StrictlyIncreasingTest()
        {
            Assert.IsTrue(new[] { 3.0, 4.0 }.IsStrictlyIncreasing());
            Assert.IsTrue(new[] { 3.0, 4.0, 5.0 }.IsStrictlyIncreasing());
            Assert.IsFalse(new[] { 3.0, 2.0, 1.0 }.IsStrictlyIncreasing());
        }

        [Test]
        public void OneElementAlwaysStrictlyIncreasing()
        {
            Assert.IsTrue(new[] { 3.0 }.IsStrictlyIncreasing());
        }

        [Test]
        public void EmptyArrayIsIncreasingThrowException()
        {
            Assert.Throws<ArgumentException>(() => new double[0].IsStrictlyIncreasing());
        }

        [Test]
        public void StrictlyDecreasingTest()
        {
            Assert.IsTrue(new[] { 3.0, 2.0 }.IsStrictlyDecreasing());
            Assert.IsTrue(new[] { 3.0, 2.0, 1.0 }.IsStrictlyDecreasing());
            Assert.IsFalse(new[] { 3.0, 4.0, 5.0 }.IsStrictlyDecreasing());
        }

        [Test]
        public void OneElementAlwaysStrictlyDecreasing()
        {
            Assert.IsTrue(new[] { 3.0 }.IsStrictlyDecreasing());
        }

        [Test]
        public void EmptyArrayIsDecreasingThrowException()
        {
            Assert.Throws<ArgumentException>(() => new double[0].IsStrictlyDecreasing());
        }

        [Test]
        public void StrictlyIncreasingCountTest()
        {
            Assert.AreEqual(0, new double[0].StrictlyIncreasingCount());
            Assert.AreEqual(1, new[] { 3.0 }.StrictlyIncreasingCount());
            Assert.AreEqual(3, new[] { 3.0, 4.0, 5.0 }.StrictlyIncreasingCount());
            Assert.AreEqual(2, new[] { 1.0, 3.0, 2.0 }.StrictlyIncreasingCount());
        }

        [Test]
        public void StrictlyDecreasingCountTest()
        {
            Assert.AreEqual(0, new double[0].StrictlyDecreasingCount());
            Assert.AreEqual(1, new[] { 3.0 }.StrictlyDecreasingCount());
            Assert.AreEqual(3, new[] { 5.0, 4.0, 3.0 }.StrictlyDecreasingCount());
            Assert.AreEqual(2, new[] { 4.0, 3.0, 5.0 }.StrictlyDecreasingCount());
        }
    }
}
