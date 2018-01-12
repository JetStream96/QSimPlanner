using CommonLibrary.LibraryExtension.Sets;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static CommonLibrary.LibraryExtension.Types;

namespace CommonLibraryTest.LibraryExtension.Sets
{
    [TestFixture]
    public class ReadOnlySetTest
    {
        [Test]
        public void CountTest()
        {
            var s = new ReadOnlySet<int>(new HashSet<int> { 10, 42 });
            Assert.AreEqual(2, s.Count);
        }

        [Test]
        public void ContainsTest()
        {
            var s = new ReadOnlySet<string>(new HashSet<string> { "ABC", "pqr"});
            Assert.IsTrue(s.Contains("ABC"));
            Assert.IsTrue(s.Contains("pqr"));
        }
    }
}
