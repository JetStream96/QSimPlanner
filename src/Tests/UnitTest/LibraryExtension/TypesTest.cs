using NUnit.Framework;
using System.Linq;
using static QSP.LibraryExtension.Types;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class TypesTest
    {
        [Test]
        public void ListTest()
        {
            var list = List(8, 12, 6.0);
            var eps = 1e-7;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(8, list[0], eps);
            Assert.AreEqual(12, list[1], eps);
            Assert.AreEqual(6.0, list[2], eps);
        }

        [Test]
        public void ListSingleItemTest()
        {
            string[] array = { "A", "BC" };
            var list = List(array);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("A", list[0][0]);
            Assert.AreEqual("BC", list[0][1]);
        }

        [Test]
        public void ReadOnlySetTest()
        {
            var set = ReadOnlySet(-5, 1);
            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains(-5));
            Assert.IsTrue(set.Contains(1));
        }

        [Test]
        public void ReadOnlySetSingleItemTest()
        {
            string[] array = { "A" };
            var set = ReadOnlySet(array);
            Assert.AreEqual(1, set.Count);
            Assert.AreEqual(1, set.First().Length);
            Assert.AreEqual("A", set.First()[0]);
        }
    }
}