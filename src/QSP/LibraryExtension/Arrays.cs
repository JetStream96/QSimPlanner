using System;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Arrays
    {
        public static int MinIndex<T>(this T[] array) where T : IComparable<T>
        {
            int index = 0;
            T min = array[0];

            for (int i = 1; i < array.Length; i++)
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

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i].CompareTo(max) > 0)
                {
                    index = i;
                    max = array[i];
                }
            }
            return index;
        }

        public static T[] SubArray<T>(this T[] data, int index)
        {
            return data.SubArray(index, data.Length - index);
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Returns a subarray starting with the given index. 
        /// Total number of elements examined is given by "length". 
        /// The returning array may have fewer elements than "length" 
        /// since any item in ignoredItems is NOT added to resulting array.
        /// </summary>
        public static T[] SubArray<T>(this T[] data, 
                                      int index, 
                                      int length, 
                                      T[] ignoredItems)
        {
            var result = new T[length];
            int currentIndex = 0;

            for (int i = index; i < index + length; i++)
            {
                if (!ignoredItems.Contains(data[i]))
                {
                    result[currentIndex++] = data[i];
                }
            }
            return result.SubArray(0, currentIndex);
        }

        public static void Multiply(this double[] item, double c)
        {
            for (int i = 0; i < item.Length; i++)
            {
                item[i] *= c;
            }
        }
        
        public static T Last<T>(this T[] array)
        {
            return array[array.Length - 1];
        }        
    }
}
