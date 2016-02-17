using System;

namespace QSP.LibraryExtension
{
    public static class JaggedArrays
    {
        // Usage: int[][][] my3DArray = CreateJaggedArray<int[][][]>(1, 2, 3);
        // from: http://stackoverflow.com/questions/1738990/initializing-jagged-arrays
        // TODO: modify when possible (license issue)

        public static T CreateJaggedArray<T>(params int[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }

        public static object InitializeJaggedArray(Type type, int index, int[] lengths)
        {
            Array array = Array.CreateInstance(type, lengths[index]);
            Type elementType = type.GetElementType();

            if (elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    array.SetValue(
                        InitializeJaggedArray(elementType, index + 1, lengths), i);
                }
            }

            return array;
        }
    }
}
