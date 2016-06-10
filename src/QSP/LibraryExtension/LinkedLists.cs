using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class LinkedLists
    {
        public static void Append<T>(
            this LinkedList<T> item, LinkedList<T> other)
        {
            for (var node = other.First;
                 node != null;
                 node = node.Next)
            {
                item.AddLast(node.Value);
            }
        }
    }
}
