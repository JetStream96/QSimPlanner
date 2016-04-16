using System;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{

    public static class BinarySearchForAll
    {

        public static Tuple<int, int> BinarySearchAllMatches<T>(List<T> lst, T target, IComparer<T> comparer)
        {
            //returns the indices of first and last match
            return BinarySearchAllMatches(lst, 0, lst.Count, target, comparer);
        }

        public static Tuple<int, int> BinarySearchAllMatches<T>(List<T> lst, int index, int count, T target, IComparer<T> comparer)
        {
            //returns the indices of first and last match

            Tuple<int, int, int> firstMatch = BinarySearchFirstMatch(lst, index, count, target, comparer);

            if (firstMatch.Item1 == -1)
            {
                return new Tuple<int, int>(-1, -1);
            }

            int first = BinarySearchLeft(lst, target, comparer, firstMatch.Item1, firstMatch.Item2);
            int last = BinarySearchRight(lst, target, comparer, firstMatch.Item2, firstMatch.Item3);

            return new Tuple<int, int>(first, last);
        }

        public static Tuple<int, int> BinarySearchAllMatches<T>(List<T> lst, int index, int count, ref T target)
        {
            IComparer<T> comp = Comparer<T>.Default;
            return BinarySearchAllMatches(lst, index, count, target, comp);
        }

        public static Tuple<int, int> BinarySearchAllMatches<T>(List<T> lst, T target)
        {
            IComparer<T> comp = Comparer<T>.Default;
            return BinarySearchAllMatches(lst, target, comp);
        }

        private static Tuple<int, int, int> BinarySearchFirstMatch<T>(List<T> lst, T target, IComparer<T> comparer)
        {
            return BinarySearchFirstMatch(lst, 0, lst.Count, target, comparer);
        }

        private static Tuple<int, int, int> BinarySearchFirstMatch<T>(List<T> lst, int index, int count, T target, IComparer<T> comparer)
        {
            int left = index;
            int right = index + count - 1;
            int middle = 0;
            int r = 0;
            
            while (true)
            {
                middle = (left + right) / 2;
                r = comparer.Compare(lst[middle], target);
                
                if (r > 0)
                {
                    if (right == middle - 1)
                    {
                        return new Tuple<int, int, int>(-1, -1, -1);
                        //not found
                    }
                    else
                    {
                        right = middle - 1;
                    }

                }
                else if (r < 0)
                {
                    if (left == middle + 1)
                    {
                        return new Tuple<int, int, int>(-1, -1, -1);
                        //not found
                    }
                    else
                    {
                        left = middle + 1;
                    }

                }
                else
                {
                    return new Tuple<int, int, int>(left, middle, right);
                }

            }

        }

        private static Tuple<int, int, int> BinarySearchFirstMatch<T>(List<T> lst, T target)
        {
            IComparer<T> comp = Comparer<T>.Default;
            return BinarySearchFirstMatch(lst, target, comp);
        }

        private static int BinarySearchLeft<T>(List<T> lst, T target, IComparer<T> comparer, int left, int right)
        {
            //between left and right, find the match with smallest index
            int leftLimit = left;
            int middle = 0;
            int r = 0;

            while (true)
            {
                middle = (left + right) / 2; 
                r = comparer.Compare(lst[middle], target);

                if (r > 0)
                {
                    if (right == middle - 1)
                    {
                        return -1;
                    }
                    right = middle - 1;
                }
                else if (r < 0)
                {
                    if (left == middle + 1)
                    {
                        return -1;
                    }
                    left = middle + 1;
                }
                else
                {
                    if (middle == leftLimit)
                    {
                        return leftLimit;
                    }

                    if (comparer.Compare(lst[middle - 1], target) == 0)
                    {
                        right = middle - 1;
                    }
                    else
                    {
                        return middle;
                    }
                }
            }
        }

        private static int BinarySearchRight<T>(List<T> lst, T target, IComparer<T> comparer, int left, int right)
        {
            //between left and right, find the match with largest index
            int rightLimit = right;
            int middle = 0;
            int r = 0;

            while (true)
            {
                middle = (left + right) / 2;
                r = comparer.Compare(lst[middle], target);

                if (r > 0)
                {
                    if (right == middle - 1)
                    {
                        return -1;
                    }
                    right = middle - 1;
                }
                else if (r < 0)
                {
                    if (left == middle + 1)
                    {
                        return -1;
                    }
                    left = middle + 1;
                }
                else
                {
                    if (middle == rightLimit)
                    {
                        return rightLimit;
                    }

                    if (comparer.Compare(lst[middle + 1], target) == 0)
                    {
                        left = middle + 1;
                    }
                    else
                    {
                        return middle;
                    }
                }
            }
        }
    }
}
