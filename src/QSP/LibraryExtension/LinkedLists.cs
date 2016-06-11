using System;
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
        /// Find the first LinkedListNode matching the given conditions.
        /// Returns null if no match is found.
        /// </summary>
        public static LinkedListNode<T> FindFirst<T>(
            this LinkedList<T> item,
            LinkedListNode<T> target,
            IEqualityComparer<LinkedListNode<T>> comparer)
        {
            var n = item.First;

            while (n != null)
            {
                if (comparer.Equals(n, target))
                {
                    return n;
                }

                n = n.Next;
            }

            return null;
        }

        /// <summary>
        /// Find all LinkedListNodes matching the given conditions.
        /// </summary>
        public static List<LinkedListNode<T>> FindAll<T>(
            this LinkedList<T> item,
            Predicate<LinkedListNode<T>> predicate)
        {
            var result = new List<LinkedListNode<T>>();
            var n = item.First;

            while (n != null)
            {
                if (predicate(n))
                {
                    result.Add(n);
                }

                n = n.Next;
            }

            return result;
        }
    }
}
