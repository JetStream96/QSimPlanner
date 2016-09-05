using System;
using System.Linq;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class IEnumerables
    {
        public static void ForEach<T>(
            this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static T MaxBy<T>(
            this IEnumerable<T> source, Func<T, double> selector)
        {
            return source.Aggregate((x, y) =>
            selector(x) > selector(y) ? x : y);
        }

        public static T MinBy<T>(
            this IEnumerable<T> source, Func<T, double> selector)
        {
            return source.Aggregate((x, y) =>
            selector(x) < selector(y) ? x : y);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
