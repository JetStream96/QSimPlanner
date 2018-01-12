using CommonLibrary.LibraryExtension;
using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class FixedIndexListTest
    {
        [Test]
        public void CreateListTest()
        {
            var item = new FixedIndexList<double>();
            Assert.AreEqual(0, item.Capacity);
            Assert.IsTrue(item.IndexUpperBound <= 0);
        }

        private static FixedIndexList<double> List(int count)
        {
            var item = new FixedIndexList<double>();
            Enumerable.Range(0, count).ForEach(i => item.Add(i));
            return item;
        }

        [Test]
        public void AddItemsTest()
        {
            List(100000);
        }

        [Test]
        public void ReadItemCorrectnessTest()
        {
            var list = new FixedIndexList<double>();
            var elem = Enumerable.Range(0, 100000);
            var indices = elem.Select(i => list.Add(i));

            Assert.IsTrue(elem.Zip(indices, (item, index) => new { item, index })
                .All(i => list[i.index] == i.item));
        }

        [Test]
        public void SmallListCorrectSizeTest()
        {
            Assert.AreEqual(4, List(1).Capacity);
            Assert.AreEqual(4, List(2).Capacity);
            Assert.AreEqual(4, List(3).Capacity);
            Assert.AreEqual(4, List(4).Capacity);
        }

        [Test]
        public void LargeListCorrectSizeTest()
        {
            int capacity = List(150000).Capacity;
            Assert.IsTrue(capacity >= 150000 && capacity <= 150000 * 2);

            capacity = List(31000).Capacity;
            Assert.IsTrue(capacity >= 31000 && capacity <= 31000 * 2);
        }

        [Test]
        public void AccessRemovedItemThrowException()
        {
            var item = List(10);
            item.RemoveAt(5);

            Assert.That(() =>
            {
                var x = item[5];
            }, Throws.Exception);
        }

        [Test]
        public void RemoveAlreadyRemovedIndexShouldDoNothing()
        {
            var list = new FixedIndexList<int>();
            var index = list.Add(0);
            list.RemoveAt(index);
            list.RemoveAt(index);
        }

        [Test]
        public void CtorSetCapacityShouldNotNeedResize()
        {
            var list = new FixedIndexList<int>(10);
            Assert.AreEqual(10, list.Capacity);
            Enumerable.Range(0, 10).ForEach(i => list.Add(i));
        }

        [Test]
        public void NegativeCapacityShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FixedIndexList<int>(-1));
        }

        [Test]
        public void SetValueCorrectnessTest()
        {
            var item = List(10);
            item[2] = 42;
            Assert.AreEqual(42, item[2]);
        }

        [Test]
        public void SetRemovedItemThrowException()
        {
            var item = List(5);
            item.RemoveAt(2);

            Assert.That(() => item[2] = 0.0, Throws.Exception);
        }

        [Test]
        public void SetCapacityTest()
        {
            var item = List(5);
            item.Capacity = 10;
            Enumerable.Repeat(0, 5).ForEach(i => item.Add(i));

            Assert.AreEqual(10, item.Capacity);
        }

        [Test]
        public void SetCapacityTooSmallThrowException()
        {
            var item = List(5);
            Assert.That(() => item.Capacity = 4, Throws.Exception);
        }

        [Test]
        public void InsertAfterRemovalTest()
        {
            var list = new FixedIndexList<int>();
            var indices = Enumerable.Range(0, 10).Select(i => list.Add(i)).ToList();
            list.RemoveAt(indices[5]);
            var i5 = list.Add(-1);

            Assert.IsTrue(indices.All((index, n) => n == 5 || index == n));
            Assert.IsTrue(list[i5] == -1);
        }

        [Test]
        public void ClearTest()
        {
            var list = new FixedIndexList<int>();
            Enumerable.Range(0, 10).ForEach(i => list.Add(i));
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void CountPropertyTest()
        {
            // New instance
            var list = new FixedIndexList<int>();
            Assert.AreEqual(0, list.Count);

            // Add elements
            var indices = Enumerable.Repeat(0, 50).Select(i => list.Add(i)).ToList();
            Assert.AreEqual(50, list.Count);

            // Remove elements
            indices.Take(20).ForEach(index => list.RemoveAt(index));
            Assert.AreEqual(30, list.Count);

            // Add into previously removed spots
            Enumerable.Repeat(0, 10).ForEach(i => list.Add(i));
            Assert.AreEqual(40, list.Count);
        }

        [Test]
        public void ForeachTest()
        {
            var list = new FixedIndexList<int>();
            var elem = Enumerable.Range(0, 10);
            var indices = elem.Select(i => list.Add(i)).ToList();

            list.RemoveAt(indices[8]);

            var set = new HashSet<int>();
            foreach (var i in list) set.Add(i);

            Assert.IsTrue(set.SetEquals(elem.Except(new[] { 8 })));
        }

        [Test]
        public void ForeachShouldThrowIfListChanged()
        {
            var list = new FixedIndexList<int>();
            Enumerable.Range(0, 10).ForEach(i => list.Add(i));

            Assert.IsTrue(ForeachThrows(list, lst => lst.Add(0)));
            Assert.IsTrue(ForeachThrows(list, lst => lst.RemoveAt(0)));
            Assert.IsTrue(ForeachThrows(list, lst => lst[2] = 0));
            Assert.IsTrue(ForeachThrows(list, lst => lst.Clear()));
        }

        private static bool ForeachThrows<T>(FixedIndexList<T> list, Action<FixedIndexList<T>> ac)
        {
            return ExceptionHelpers.Throws(() =>
            {
                foreach (var i in list)
                {
                    ac(list);
                }
            });
        }
    }
}