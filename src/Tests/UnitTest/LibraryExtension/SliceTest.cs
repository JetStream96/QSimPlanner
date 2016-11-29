using System;
using System.Linq;
using NUnit.Framework;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class SliceTest
    {
        [Test]
        public void BadOffSetShouldThrowException()
        {
            string[] arr = {"abc", "x", "y"};
            Assert.Throws<ArgumentException>(() => new Slice<string>(arr, -1));
            Assert.Throws<ArgumentException>(() => new Slice<string>(arr, 4));
        }

        [Test]
        public void SliceTestIndexerAndCount()
        {
            string[] arr = { "abc", "x", "y" };
            var s = new Slice<string>(arr, 1);

            Assert.AreEqual(2, s.Count);
            Assert.IsTrue("x" == s[0]);
            Assert.IsTrue("y" == s[1]);
        }

        [Test]
        public void SliceTestEnumerator()
        {
            int[] arr = { 8, 10, 42, 55};
            var s = new Slice<int>(arr, 2);

            Assert.AreEqual(2, s.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(new[] {42, 55}, s));
        }
    }
}