using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class IReadOnlyLists
    {
        public static int MinIndex<T>(this IReadOnlyList<T> list) where T : IComparable<T>
        {
            var min = list.Min();

            for (int i = 0; i < list.Count; i++)
            {
                if (min.CompareTo(list[i]) == 0) return i;
            }

            throw new ArgumentException();
        }
    }
}