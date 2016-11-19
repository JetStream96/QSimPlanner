using System;

namespace QSP.LibraryExtension.JaggedArrays
{
    public static class JaggedArray
    {
        // Usage: var array = CreateJaggedArray<double[][]>(3, 5);
        public static T Create<T>(params int[] arrayLengths)
        {
            return (T)GetJaggedArray(typeof(T).GetElementType(), 0, arrayLengths);
        }

        private static object GetJaggedArray(Type type, int index, params int[] lengths)
        {
            var result = Array.CreateInstance(type, lengths[index]);
            var elementType = type.GetElementType();

            if (elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    result.SetValue(
                        GetJaggedArray(elementType, index + 1, lengths), i);
                }
            }
            return result;
        }

        public static void Multiply(this object jaggedArray, double c)
        {
            var array = (Array)jaggedArray;

            if (array.GetType().GetElementType().IsArray == false)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array.SetValue((double)(array.GetValue(i)) * c, i);
                }
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array.GetValue(i).Multiply(c);
                }
            }
        }
    }
}
