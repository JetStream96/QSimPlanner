using System.Collections.Generic;

namespace CommonLibrary.LibraryExtension.Sets
{
    public interface IReadOnlySet<T> : IEnumerable<T>
    {
        bool Contains(T item);
        int Count { get; }
    }
}