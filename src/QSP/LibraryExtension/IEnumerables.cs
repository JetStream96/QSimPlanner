using System;
using System.Linq;
using System.Collections.Generic;
using QSP.LibraryExtension.Sets;

namespace QSP.LibraryExtension
{
    public static class IEnumerables
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        public static T MaxBy<T, U>(this IEnumerable<T> source, Func<T, U> selector)
            where U : IComparable<U>
        {
            return source.Aggregate((x, y) => selector(x).CompareTo(selector(y)) > 0 ? x : y);
        }

        public static T MinBy<T, U>(this IEnumerable<T> source, Func<T, U> selector)
            where U : IComparable<U>
        {
            return source.Aggregate((x, y) => selector(x).CompareTo(selector(y)) < 0 ? x : y);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static ReadOnlySet<T> ToReadOnlySet<T>(this IEnumerable<T> item)
        {
            return new ReadOnlySet<T>(item.ToHashSet());
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T x)
        {
            return source.Concat(new T[] { x });
        }

        public static int HashCodeByElem<T>(this IEnumerable<T> item)
        {
            int hash = 19;

            foreach (var i in item)
            {
                hash *= 31;
                hash += i == null ? 0 : i.GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// Returns a hash code which is independent of the order of elements.
        /// Throws exception if item is empty.
        /// </summary>
        public static int HashCodeSymmetric<T>(this IEnumerable<T> item)
        {
            return item.Select(i => i == null ? 0 : i.GetHashCode()).Aggregate((x, y) => x ^ y);
        }

        public static double[] ToDoubles(this IEnumerable<string> item)
        {
            return item.Select(s => Convert.ToDouble(s)).ToArray();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in source) action(e, i++);
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> source, params T[] items)
        {
            return source.SequenceEqual((IEnumerable<T>)items);
        }

        public static bool SetEquals<T>(this IEnumerable<T> source, params T[] items)
        {
            return source.ToHashSet().SetEquals(items);
        }

        public static bool SetEquals<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return source.ToHashSet().SetEquals(items);
        }
    }
}

