using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Lists
    {
        /// <summary>
        /// Remove the last item, if the list is not null or empty.
        /// Return value indicates whether the removal was successful.
        /// </summary>
        public static bool TryRemoveLast<T>(this List<T> item)
        {
            if (item == null || item.Count == 0) return false;
            item.RemoveAt(item.Count - 1);
            return true;
        }

        public static List<T> CreateList<T>(params T[] items)
        {
            return new List<T>(items);            
        }

        public static T Last<T>(this List<T> item)
        {
            return item[item.Count - 1];
        }

        // TODO: Add test.
        public static List<T> WithoutFirstAndLast<T>(
            this IReadOnlyCollection<T> item)
        {
            return item.Skip(1).Take(item.Count - 2).ToList();
        }
    }
}
