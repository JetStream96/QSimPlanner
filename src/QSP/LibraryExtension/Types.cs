using QSP.LibraryExtension.Sets;
using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    /// <summary>
    /// A more concise way to instantiate frequently used data structures.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Usage: ReadOnlySet("A", "BC")
        /// </summary>
        public static IReadOnlySet<T> ReadOnlySet<T>(T x, params T[] xs)
        {
            if (xs.Length == 0) return new ReadOnlySet<T>(new HashSet<T> { x });

            var set = new HashSet<T>(xs);
            set.Add(x);
            return new ReadOnlySet<T>(set);
        }

        /// <summary>
        /// Usage: List(1, 2, 3) => [1, 2, 3]
        /// </summary>
        public static List<T> List<T>(T x, params T[] xs)
        {
            var list = new List<T> { x };
            if (xs.Length > 0) list.AddRange(xs);
            return list;
        }
        
        /// <summary>
        /// Usage: Arr(1, 2, 3) => [1, 2, 3]
        /// </summary>
        public static T[] Arr<T>(T x, params T[] xs)
        {
            return List(x, xs).ToArray();
        }
        
        /// <summary>
         /// Usage: Dict(("key0", 0), ("key1", 1), ("key2", 2)) => 
         /// {
         ///     "key0": 0,
         ///     "key1": 1,
         ///     "key2": 2,
         /// }
         /// </summary>
        public static Dictionary<TKey, TValue> Dict<TKey, TValue>((TKey, TValue) x,
            params (TKey, TValue)[] xs)
        {
            return List(x, xs).ToDictionary(y => y.Item1, y => y.Item2);
        }
    }
}