using NUnit.Framework;
using QSP.LibraryExtension;
using System;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public static class ControlFlow
    {
        [Test]
        public static void MatchCaseTest()
        {
            Assert.AreEqual(2, "a".MatchCase(("x", 10), ("b", -9), ("a", 2)));
        }

        [Test]
        public static void MatchCaseOneCaseOnly()
        {
            Assert.AreEqual(2, "a".MatchCase(("a", 2)));
        }

        [Test]
        public static void MatchCaseException()
        {
            Assert.Throws<ArgumentException>(() => "a".MatchCase(("b", -9), ("c", 2)));
        }

        [Test]
        public static void TryMatchCaseSuccess()
        {
            var (matched, result) = "a".TryMatchCase(("x", 10), ("b", -9), ("a", 2));
            Assert.IsTrue(matched);
            Assert.AreEqual(2, result);
        }

        [Test]
        public static void TryMatchCaseFail()
        {
            var (matched, result) = "a".TryMatchCase(("x", 10), ("b", -9), ("c", 2));
            Assert.IsFalse(matched);
        }

        [Test]
        public static void TryMatchCaseDefaultFound()
        {
            Assert.AreEqual(2, "a".MatchCaseDefault(15, ("x", 10), ("b", -9), ("a", 2)));
        }

        [Test]
        public static void TryMatchCaseDefaultNotFound()
        {
            Assert.AreEqual(15, "a".MatchCaseDefault(15, ("x", 10), ("b", -9), ("c", 2)));
        }
    }
}
