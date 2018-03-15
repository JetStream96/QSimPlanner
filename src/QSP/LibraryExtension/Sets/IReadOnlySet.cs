using System.Collections.Generic;

namespace QSP.LibraryExtension.Sets
{
    public interface IReadOnlySet<T> : IEnumerable<T>
    {
        bool Contains(T item);
        int Count { get; }
    }
}