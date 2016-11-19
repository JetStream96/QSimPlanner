using QSP.Utilities;
using System;

namespace QSP.LibraryExtension.JaggedArrays
{
    public static class LengthChecker
    {
        public static bool HasLength<T>(object JaggedArray, params int[] lengths)
        {
            var array = (Array)JaggedArray;

            if (array.GetType().GetElementType() == typeof(T))
            {
                ExceptionHelpers.Ensure<ArgumentException>(lengths.Length == 1);
                return array.Length >= lengths[0];
            }
            else
            {
                if (array.Length < lengths[0]) return false;

                var newLen = new int[lengths.Length - 1];
                Array.Copy(lengths, 1, newLen, 0, newLen.Length);
                
                foreach (var i in array)
                {
                    if (HasLength<T>(i, newLen) == false) return false;
                }

                return true;
            }
        }
    }
}
