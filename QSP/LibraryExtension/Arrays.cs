using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    public static class Arrays
    {
        public static int MinIndex<T>(this T[] array) where T : IComparable<T>
        {
            int index = 0;
            T min = array[0];

            for (int i = 1; i <= array.Length - 1; i++)
            {
                if (array[i].CompareTo(min) < 0)
                {
                    index = i;
                    min = array[i];
                }
            }

            return index;
        }

        public static int MaxIndex<T>(this T[] array) where T : IComparable<T>
        {

            int index = 0;
            T max = array[0];

            for (int i = 1; i <= array.Length - 1; i++)
            {
                if (array[i].CompareTo(max) > 0)
                {
                    index = i;
                    max = array[i];
                }
            }

            return index;
        }

        public static T[] RemoveFirstElement<T>(T[] array)
        {
            T[] u = new T[array.Length - 1];

            for (int i = 1; i <= array.Length - 1; i++)
            {
                u[i - 1] = array[i];
            }
            return u;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Returns a subarray starting with the given index. Total number of elements examined is given by "length". 
        /// The returning array may have fewer elements than "length" since any item in ignoredItems is NOT added to resulting array.
        /// </summary>
        public static T[] SubArray<T>(this T[] data, int index, int length, T[] ignoredItems)
        {
            List<T> result = new List<T>();

            for (int i = index; i <= index + length - 1; i++)
            {
                if (!ignoredItems.Contains(data[i]))
                {
                    result.Add(data[i]);
                }
            }
            return result.ToArray();
        }

        public static T[] Exclude<T>(this T[] data, T[] ignoredItems)
        {
            return SubArray(data, 0, data.Length, ignoredItems);
        }

        public static void multiply(this double[] item, double c)
        {
            for (int i = 0; i < item.Length; i++)
            {
                item[i] *= c;
            }
        }

        public static void multiply(this double[,] item, double c)
        {
            for (int i = 0; i < item.GetLength(0); i++)
            {
                for (int j = 0; j < item.GetLength(1); j++)
                {
                    item[i, j] *= c;
                }
            }
        }

        public static void multiply(this double[,,] item, double c)
        {
            for (int i = 0; i < item.GetLength(0); i++)
            {
                for (int j = 0; j < item.GetLength(1); j++)
                {
                    for (int k = 0; k < item.GetLength(2); k++)
                    {
                        item[i, j, k] *= c;
                    }
                }
            }
        }

        public static void multiply(this double[,,,] item, double c)
        {
            for (int i = 0; i < item.GetLength(0); i++)
            {
                for (int j = 0; j < item.GetLength(1); j++)
                {
                    for (int k = 0; k < item.GetLength(2); k++)
                    {
                        for (int m = 0; m < item.GetLength(3); m++)
                        {
                            item[i, j, k, m] *= c;
                        }
                    }
                }
            }
        }

        public static T[] RemoveElements<T>(this T[] array, T item)
        {
            var result = new List<T>();

            foreach (var i in array)
            {
                if (!i.Equals(item))
                {
                    result.Add(i);
                }
            }
            return result.ToArray();
        }

    }
}
