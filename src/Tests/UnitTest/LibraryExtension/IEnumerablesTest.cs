using NUnit.Framework;
using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class IEnumerablesTest
    {
        [Test]
        public void MaxByTest()
        {
            var x = new[] { 5.0, 3.0, -8.0 };
            Assert.AreEqual(-8.0, x.MaxBy(t => t * t));
        }

        [Test]
        public void MaxByOnlyOneElement()
        {
            Assert.AreEqual(5, new[] { 5 }.MaxBy(t => t));
        }

        [Test]
        public void MinByTest()
        {
            var x = new[] { 5.0, 3.0, -8.0 };
            Assert.AreEqual(3.0, x.MinBy(t => (t - 3.0) * (t - 3.0)));
        }

        [Test]
        public void MinByOnlyOneElement()
        {
            Assert.AreEqual(3, new[] { 3 }.MinBy(t => t));
        }

        [Test]
        public void ForeachTest()
        {
            var list = new List<int>();
            new[] { 2, 1, 0 }.ForEach((item, index) => list.Add(item + index));
            Assert.IsTrue(list.SequenceEqual(2, 2, 2));
        }

        [Test]
        public void SequenceEqualTest()
        {
            Assert.IsTrue(new[] { 3, 2, 0 }.SequenceEqual(3, 2, 0));
        }
    }
}
