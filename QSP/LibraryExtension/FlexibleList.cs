using System;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    /// <summary>
    /// A generic list such that each element added would never change index, unless removed.
    /// Implemeted using an array.
    /// Total number of elements is NOT known at any time, although 
    /// it's guranteed that no element has index larger or equal to MaxSize().
    /// </summary>
    public class FlexibleList<T>
    {
        private const int initCapacity = 4;
        static readonly Entry[] emptyArray = new Entry[0];

        private Entry[] _items;
        private int _size;
        private int _free;

        private struct Entry
        {
            public int next;
            public T value;

            public Entry(int next, T value)
            {
                this.next = next;
                this.value = value;
            }
        }

        public FlexibleList()
        {
            _items = emptyArray;
            _size = 0;
            _free = -1;
        }

        public FlexibleList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _items = new Entry[capacity];
            _size = 0;
            _free = -1;
        }

        public int Add(T item)
        {
            if (_free >= 0)
            {
                int index = tryFillDeletedSpot(item);
                if (index >= 0)
                {
                    return index;
                }
            }

            if (_size == _items.Length)
            {
                increaseCapacity(1);
            }
            _items[_size] = new Entry(-1, item);
            return _size++;
        }

        private void increaseCapacity(int newCount)
        {
            int minimumSize = _size + newCount;
            if (minimumSize > _items.Length)
            {
                Capacity = Math.Max(Math.Max(Capacity * 2, initCapacity), minimumSize);
            }
        }

        /// <summary>
        /// Try to add the item to a previously deleted place.
        /// Returns whether the insertion is successful.
        /// </summary>
        private int tryFillDeletedSpot(T item)
        {
            while (_free>=0)
            {
                if (_free < _size)
                {
                    int tmp = _free;
                    _free = _items[_free].next;
                    _items[tmp] = new Entry(-1, item);
                   
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

        public int MaxSize
        {
            get
            {
                return _size;
            }
        }

        public T this[int index]
        {
            get
            {
                if (isRemoved(index))
                {
                    throw new ArgumentException("The element at given index is already removed.");
                }
                return _items[index].value;
            }
            set
            {
                if (isRemoved(index))
                {
                    throw new ArgumentException("The element at given index is already removed.");
                }
                _items[index].value = value;
            }
        }

        public void Clear()
        {
            _items = emptyArray;
            _free = -1;
            _size = 0;
        }

        private bool isRemoved(int index)
        {
            if( _items[index].next >=0 || index == _free )
            {
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if(isRemoved(index))
            {
                return;
            }
            
            _items[index].next = _free;
            _free = index;
        }

    }
}


