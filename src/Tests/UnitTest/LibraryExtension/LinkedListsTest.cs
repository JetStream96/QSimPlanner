using NUnit.Framework;
using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class LinkedListsTest
    {
        [Test]
        public void AppendTest()
        {
            var x = new LinkedList<int>(new int[] { 1, 2, 3 });
            var y = new LinkedList<int>(new int[] { 4, 5 });

            x.AddLast(y);

            Assert.IsTrue(
                Enumerable.SequenceEqual(x, new int[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void AppendEmptyList()
        {
            var x = new LinkedList<int>(new int[] { 1, 2, 3 });
            var y = new int[] { };

            x.AddLast(y);

            Assert.IsTrue(
                Enumerable.SequenceEqual(x, new int[] { 1, 2, 3 }));
        }

        [Test]
        public void EmptyListAppend()
        {
            var x = new LinkedList<int>(new int[] { });
            var y = new int[] { 4, 5, 6 };

            x.AddLast(y);

            Assert.IsTrue(
                Enumerable.SequenceEqual(x, new int[] { 4, 5, 6 }));
        }
    }
}
