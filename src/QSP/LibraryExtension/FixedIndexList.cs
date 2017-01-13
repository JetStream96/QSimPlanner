using System;
using System.Collections;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    // Test result of adding 10000000 integers:
    //
    // Initial capacity not set:
    //      FixedIndexList: 216ms
    //      List:           130ms
    // 
    // Initial capacity set to 10000000:
    //      FixedIndexList: 85ms
    //      List:           60ms
    //
    // Result for adding without resize (increase number of elements 
    // from 2^24+1 to 2^25):
    //      FixedIndexList: 138ms
    //      List:           99ms
    //
    // Remove the element in the middle, with 100000 integers in the list:
    //      FixedIndexList: 4.84ms/100000 times (tested with 10000000 times)
    //      List:           2857ms/100000 times (tested with 100000 times)

    /// <summary>
    /// A generic list such that each element added would never 
    /// change index, unless removed.
    /// Implemeted using an array.
    /// </summary>
    public class FixedIndexList<T> : IEnumerable<T>, IEnumerable
    {
        private const int initCapacity = 4;
        private static readonly Entry[] emptyArray = new Entry[0];

        private Entry[] _items;
        private int _size;
        private int _free;  // Either -2, or the index of a removed item.
        private int _count;

        private struct Entry
        {
            public int next;
            public T value;

            public Entry(int next, T value)
            {
                // If >=0, it's the index of the next removed item.
                // If -1, it indicates this item is not removed.
                // If -2, this item is already removed, 
                // but there isn't a next removed item.
                this.next = next;

                this.value = value;
            }
        }

        public FixedIndexList()
        {
            _items = emptyArray;
            _size = 0;
            _free = -2;
            _count = 0;
        }

        public FixedIndexList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            _items = new Entry[capacity];
            _size = 0;
            _free = -2;
            _count = 0;
        }

        public int Add(T item)
        {
            if (_free >= 0)
            {
                int index = TryFillDeletedSpot(item);
                if (index >= 0)
                {
                    return index;
                }
            }

            if (_size == _items.Length)
            {
                IncreaseCapacity(1);
            }

            _items[_size] = new Entry(-1, item);
            _count++;
            return _size++;
        }

        private void IncreaseCapacity(int newCount)
        {
            int minimumSize = _size + newCount;
            if (minimumSize > _items.Length)
            {
                Capacity = Math.Max(Math.Max(Capacity * 2, initCapacity), minimumSize);
            }
        }

        /// <summary>
        /// Try to add the item to a previously deleted place.
        /// Returns the index at which the item is added, or -1 if the
        /// insertion is unsuccessful.
        /// </summary>
        private int TryFillDeletedSpot(T item)
        {
            while (_free >= 0)
            {
                if (_free < _size)
                {
                    int tmp = _free;
                    _free = _items[_free].next;
                    _items[tmp] = new Entry(-1, item);
                    _count++;

                    return tmp;
                }
                else
                {
                    int tmp = _free;
                    _free = _items[_free].next;
                    _items[tmp].next = -1;
                }
            }

            return -1;
        }

        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if ((uint)value < (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                Array.Resize(ref _items, value);
            }
        }

        public int Count => _count;

        /// <summary>
        /// The upper bound of indices of elements, if the list is non-empty.
        /// If the list is empty this number can be negative.
        /// </summary>
        public int IndexUpperBound => _size - 1;

        /// <exception cref="Exception"></exception>
        public T this[int index]
        {
            get
            {
                if (IsRemoved(index))
                {
                    throw new ArgumentOutOfRangeException(
                        "The element at given index is already removed.");
                }

                return _items[index].value;
            }
            set
            {
                if (IsRemoved(index))
                {
                    throw new ArgumentOutOfRangeException(
                        "The element at given index is already removed.");
                }

                _items[index].value = value;
            }
        }

        public void Clear()
        {
            _items = emptyArray;
            _free = -2;
            _size = 0;
            _count = 0;
        }

        private bool IsRemoved(int index)
        {
            return _items[index].next != -1;
        }

        public bool ItemExists(int index)
        {
            return !IsRemoved(index);
        }

        public void RemoveAt(int index)
        {
            if (IsRemoved(index))
            {
                return;
            }

            _items[index].value = default(T);
            _items[index].next = _free;
            _free = index;
            _count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private FixedIndexList<T> list;
            private int index;
            private T current;

            public Enumerator(FixedIndexList<T> item)
            {
                list = item;
                index = 0;
                current = default(T);
            }

            public T Current => current;

            object IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == list._size + 1)
                    {
                        throw new InvalidOperationException("Cannot enumerate.");
                    }

                    return Current;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                FixedIndexList<T> localList = list;

                while (((uint)index < (uint)localList._size))
                {
                    if (localList.IsRemoved(index) == false)
                    {
                        current = localList._items[index].value;
                        index++;
                        return true;
                    }
                    else
                    {
                        index++;
                    }
                }

                return false;
            }

            public void Reset()
            {
                index = 0;
                current = default(T);
            }
        }
    }
}
