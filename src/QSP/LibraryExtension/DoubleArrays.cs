using System;
using QSP.Utilities;

namespace QSP.LibraryExtension
{
    public static class DoubleArrays
    {
        /// <exception cref="ArgumentException">
        /// Array length cannot be 0.</exception>
        public static bool IsStrictlyIncreasing(this double[] item)
        {
            ExceptionHelpers.Ensure<ArgumentException>(item.Length != 0);

            for (int i = 0; i < item.Length - 1; i++)
            {
                if (item[i] >= item[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        /// <exception cref="ArgumentException">
        /// Array length cannot be 0.</exception>
        public static bool IsStrictlyDecreasing(this double[] item)
        {
            ExceptionHelpers.Ensure<ArgumentException>(item.Length != 0);

            for (int i = 0; i < item.Length - 1; i++)
            {
                if (item[i] <= item[i + 1])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
