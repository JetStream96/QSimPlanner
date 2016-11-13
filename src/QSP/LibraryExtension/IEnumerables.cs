using System;
using System.Linq;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class IEnumerables
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        public static T MaxBy<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            return source.Aggregate((x, y) => selector(x) > selector(y) ? x : y);
        }

        public static T MinBy<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            return source.Aggregate((x, y) => selector(x) < selector(y) ? x : y);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
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
    }
}
