using System;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class IEnumerables
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
