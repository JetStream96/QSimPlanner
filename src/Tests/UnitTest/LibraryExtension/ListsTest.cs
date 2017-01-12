using System.Linq;
using NUnit.Framework;
using static QSP.LibraryExtension.Lists;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class ListsTest
    {
        [Test]
        public void WithoutFirstAndLastTest()
        {
            int[] array = { 3, 5, 7, 9 };
            int[] expected = { 5, 7 };

            Assert.IsTrue(array.WithoutFirstAndLast().SequenceEqual(expected));
        }

        [Test]
        public void WithoutFirstAndLastTooSmallCollection()
        {
            int[] array1 = { 3, 5 };
            int[] array2 = { 3 };
            int[] array3 = {};

            Assert.AreEqual(0, array1.WithoutFirstAndLast().Count);
            Assert.AreEqual(0, array2.WithoutFirstAndLast().Count);
            Assert.AreEqual(0, array3.WithoutFirstAndLast().Count);
        }
    }
}
