using System.Collections;
using System.Collections.Generic;

namespace QSP.LibraryExtension.Sets
{
    public class ReadOnlySet<T> : IReadOnlySet<T>
    {
        private readonly HashSet<T> set;

        public ReadOnlySet() : this(new HashSet<T>()) { }

        public ReadOnlySet(params T[] values) : this(new HashSet<T>(values)) { }

        public ReadOnlySet(HashSet<T> set)
        {
            this.set = set;
        }

        public int Count => set.Count;
        public bool Contains(T item) => set.Contains(item);
        public IEnumerator<T> GetEnumerator() => set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}