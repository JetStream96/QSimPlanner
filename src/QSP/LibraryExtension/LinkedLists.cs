using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class LinkedLists
    {
        public static void AddLast<T>(this LinkedList<T> item, IEnumerable<T> other)
        {
            foreach (var i in other) item.AddLast(i);
        }

        public static void AddAfter<T>(this LinkedList<T> item, 
            LinkedListNode<T> node, IEnumerable<T> other)
        {
            foreach (var i in other)
            {
                item.AddAfter(node, i);
                node = node.Next;
            }
        }

        /// <summary>
        /// Enumerate through the nodes of the linked list. Enables the use for LINQ methods.
        /// </summary>
        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> item)
        {
            var n = item.First;

            while (n != null)
            {
                yield return n;
                n = n.Next;
            }
        }
    }
}
