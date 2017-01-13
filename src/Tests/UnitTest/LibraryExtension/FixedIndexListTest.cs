using NUnit.Framework;
using QSP.LibraryExtension;
using System;
using System.Collections.Generic;

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

        private FixedIndexList<double> CreateList()
        {
            var item = new FixedIndexList<double>();

            for (int i = 0; i < 100000; i++)
            {
                item.Add(i);
            }

            return item;
        }

        private FixedIndexList<double> CreateList(int count)
        {
            var item = new FixedIndexList<double>();

            for (int i = 0; i < count; i++)
            {
                item.Add(i);
            }
            return item;
        }

        [Test]
        public void AddItemsTest()
        {
            CreateList();
        }

        [Test]
        public void ReadItemCorrectnessTest()
        {
            var item = CreateList();

            for (int i = 0; i < 100000; i++)
            {
                Assert.AreEqual(i, item[i]);
            }
        }

        [Test]
        public void SmallListCorrectSizeTest()
        {
            Assert.AreEqual(4, CreateList(1).Capacity);
            Assert.AreEqual(4, CreateList(2).Capacity);
            Assert.AreEqual(4, CreateList(3).Capacity);
            Assert.AreEqual(4, CreateList(4).Capacity);
        }

        [Test]
        public void LargeListCorrectSizeTest()
        {
            int capacity = CreateList(150000).Capacity;
            Assert.IsTrue(capacity >= 150000 && capacity <= 150000 * 2);

            capacity = CreateList(31000).Capacity;
            Assert.IsTrue(capacity >= 31000 && capacity <= 31000 * 2);
        }

        [Test]
        public void RemoveAtCorrectnessTest()
        {
            var item = CreateList(58200);

            item.RemoveAt(35688);

            for (int i = 0; i < 58200; i++)
            {
                if (i != 35688)
                {
                    Assert.AreEqual(i, item[i]);
                }
            }
        }

        [Test]
        public void AccessRemovedItemThrowException()
        {
            var item = CreateList(58200);
            item.RemoveAt(35688);

            Assert.That(() =>
            {
                var x = item[35688];
            }, Throws.Exception);
        }

        [Test]
        public void SetValueCorrectnessTest()
        {
            var item = CreateList(1280);

            for (int i = 0; i < 1280; i++)
            {
                item[i] = -i;
            }

            for (int i = 0; i < 1280; i++)
            {
                Assert.AreEqual(-i, item[i]);
            }
        }

        [Test]
        public void SetRemovedItemThrowException()
        {
            var item = CreateList(58200);
            item.RemoveAt(35688);

            Assert.That(() =>
            {
                item[35688] = 123.005;
            }, Throws.Exception);
        }

        [Test]
        public void SetCapacityTest()
        {
            var item = CreateList(1280);
            item.Capacity = 1280;

            for (int i = 0; i < 1280; i++)
            {
                item[i] = -i;
            }

            for (int i = 0; i < 1280; i++)
            {
                Assert.AreEqual(-i, item[i]);
            }
        }

        [Test]
        public void SetCapacityTooSmallThrowException()
        {
            var item = CreateList(1280);

            Assert.That(() =>
            {
                item.Capacity = 1279;
            }, Throws.Exception);
        }

        [Test]
        public void InsertAfterRemovalTest()
        {
            var item = CreateList(58200);
            item.Capacity = 58200;

            for (int i = 0; i < 582; i++)
            {
                item.RemoveAt(i * 100);
            }

            Assert.AreEqual(58200, item.Capacity);

            for (int i = 0; i < 582; i++)
            {
                item.Add(10.0);
            }

            Assert.AreEqual(58200, item.Capacity);

            for (int i = 0; i < 58200; i++)
            {
                if (i % 100 == 0)
                {
                    Assert.AreEqual(10.0, item[i], 0.000001);
                }
                else
                {
                    Assert.AreEqual(i, item[i], 0.000001);
                }
            }
        }

        [Test]
        public void InsertionAfterRemovalHasCorrectValue()
        {
            var item = CreateList(58200);
            item.Capacity = 58200;

            for (int i = 0; i < 582; i++)
            {
                item.RemoveAt(i * 100);
            }

            var indices = new List<int>();

            for (int i = 0; i < 582; i++)
            {
                // The ith item added to indices has value -i.
                indices.Add(item.Add(-i));
            }

            for (int i = 0; i < 58200; i++)
            {
                if (i % 100 == 0)
                {
                    Assert.AreEqual(-i / 100, item[indices[i / 100]], 0.000001);
                }
                else
                {
                    Assert.AreEqual(i, item[i], 0.000001);
                }
            }
        }

        [Test]
        public void CountPropertyTest()
        {
            // New instance
            var x = new FixedIndexList<int>();
            Assert.AreEqual(0, x.Count);

            // Add elements
            for (int i = 0; i < 580; i++)
            {
                x.Add(i % 10);
            }
            Assert.AreEqual(580, x.Count);

            // Remove elements
            for (int i = 0; i < 8; i++)
            {
                x.RemoveAt(i * 15);
            }
            Assert.AreEqual(572, x.Count);

            // Add into previously removed spots
            for (int i = 0; i < 3; i++)
            {
                x.Add(i * i);
            }
            Assert.AreEqual(575, x.Count);

        }

        [Test]
        public void ForeachTest()
        {
            // Add and then remove some items.
            var x = new FixedIndexList<int>();

            for (int i = 0; i < 100; i++)
            {
                x.Add(i);
            }

            for (int i = 0; i < 10; i++)
            {
                x.RemoveAt(i * 10);
            }

            int counter = 1;

            foreach (var item in x)
            {
                Assert.AreEqual(counter, item);

                if (counter % 10 == 9)
                {
                    counter += 2;
                }
                else
                {
                    counter++;
                }
            }

            // Check the number of loop excution is correct.
            Assert.AreEqual(101, counter);
        }

    }
}