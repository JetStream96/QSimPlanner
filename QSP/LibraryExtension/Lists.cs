using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Lists
    {
        private static void Resize<T>(this List<T> item, int newSize)
        {
            item.AddRange(Enumerable.Repeat(default(T), newSize - item.Count));
        }

        public static List<T> WithoutDuplicates<T>(this List<T> item)
        {
            return item.Distinct().ToList();
        }

        /// <summary>
        /// Add the new list, starting from the given index, to the end of the original one.
        /// </summary>
        public static void AddRange<T>(this List<T> item, List<T> list, int index)
        {
            for (int i = index; i < list.Count; i++)
            {
                item.Add(list[i]);
            }
        }

        /// <summary>
        /// Remove the last item, if the list is not null or empty.
        /// Return value indicates whether the removal was successful.
        /// </summary>
        public static bool TryRemoveLast<T>(this List<T> item)
        {
            if (item == null || item.Count == 0)
            {
                return false;
            }
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
    }
}
