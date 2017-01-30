using System;
using NUnit.Framework;
using QSP.Utilities;

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
        public void DefaultIfThrowsTest()
        {
            Assert.AreEqual(2, ExceptionHelpers.DefaultIfThrows(() => 1 + 1));
            Assert.AreEqual(null, ExceptionHelpers.DefaultIfThrows(() =>
            {
                if ("".Length == 0) throw new ArgumentException();
                return "abc";
            }));
        }
    }
}