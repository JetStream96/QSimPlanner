using NUnit.Framework;
using QSP.Utilities;
using System;
using static QSP.Utilities.ExceptionHelpers;

namespace UnitTest.Utilities
{
    [TestFixture]
    public class ExceptionHelpersTest
    {
        [Test]
        public void IgnoreExceptionTest()
        {
            Action a = () => { throw new ArgumentException(); };
            ExceptionHelpers.IgnoreException(a);
        }

        [Test]
        public void ThrowsTest()
        {
            Assert.IsTrue(ExceptionHelpers.Throws(() => { throw new ArgumentException(); }));
            Assert.IsFalse(ExceptionHelpers.Throws(() => { }));
        }

        [Test]
        public void DefaultIfThrowsTestNoException()
        {
            Assert.AreEqual(2, DefaultIfThrows(() => 1 + 1));
        }

        [Test]
        public void DefaultIfThrowsTestException()
        {
            Assert.AreEqual(0, DefaultIfThrows(() => int.Parse("a")));
            Assert.AreEqual(4, DefaultIfThrows(() => int.Parse("a"), 4));
        }
    }
}