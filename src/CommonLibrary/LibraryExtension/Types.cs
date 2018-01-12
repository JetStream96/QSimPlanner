using CommonLibrary.LibraryExtension.Sets;
using System.Collections.Generic;

namespace CommonLibrary.LibraryExtension
{
    public static class Types
    {
        public static IReadOnlySet<T> ReadOnlySet<T>(T x, params T[] xs)
        {
            if (xs.Length == 0) return new ReadOnlySet<T>(new HashSet<T> { x });

            var set = new HashSet<T>(xs);
            set.Add(x);
            return new ReadOnlySet<T>(set);
        }
    }
}