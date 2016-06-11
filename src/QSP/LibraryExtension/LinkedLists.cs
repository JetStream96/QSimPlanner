using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class LinkedLists
    {
        public static void AddLast<T>(
            this LinkedList<T> item, IEnumerable<T> other)
        {
            foreach (var i in other)
            {
                item.AddLast(i);
            }
        }
    }
}
