using System;

namespace QSP.LibraryExtension
{
    public static class Arrays
    {
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

        public static T[] ArrayCopy<T>(this T[] arr)
        {
            var copy = new T[arr.Length];
            Array.Copy(arr, copy, arr.Length);
            return copy;
        }
    }
}
