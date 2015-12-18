using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    /// <summary>
    /// A readonly wrapper for Stack<T>.
    /// </summary>
    public class ReadOnlyStack<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        private Stack<T> _content;

        public ReadOnlyStack(Stack<T> item)
        {
            _content = item;
        }

        public int Count
        {
            get
            {
                return _content.Count();
            }
        }

        int ICollection.Count
        {
            get
            {
                return _content.Count;    
            }
        }

        public object SyncRoot
        {
            get
            {
                return ((ICollection)_content).SyncRoot;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)_content).IsSynchronized;
            }
        }

        public bool Contains(T item)
        {
            return _content.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public T Peek()
        {
            return _content.Peek();
        }

        public T[] ToArray()
        {
            return _content.ToArray();
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_content).CopyTo(array, index);
        }
    }
}
