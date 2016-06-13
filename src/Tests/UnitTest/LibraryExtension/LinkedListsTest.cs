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

        [Test]
        public void AddAfterTest()
        {
            var x = new LinkedList<int>(new int[] { 0, 1, 2 });
            var y = new int[] { 4, 5 };

            x.AddAfter(x.First.Next, y);

            Assert.IsTrue(
                Enumerable.SequenceEqual(x, new int[] { 0, 1, 4, 5, 2 }));
        }

        [Test]
        public void FindAllTest()
        {
            var x = new LinkedList<int>(new int[] { 0, 15, 22, -3, 4 });

            var matches = x.FindAll((n) =>
            n.Value % 2 == 0 && n.Next != null && n.Next.Value > 0);

            Assert.IsTrue(matches.Count == 1);
            Assert.AreEqual(0, matches[0].Value);
        }
    }
}
