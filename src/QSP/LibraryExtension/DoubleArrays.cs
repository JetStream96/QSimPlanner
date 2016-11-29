using System;
using System.Collections.Generic;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.LibraryExtension
{
    public static class DoubleArrays
    {
        /// <exception cref="ArgumentException">
        /// Array length cannot be 0.</exception>
        public static bool IsStrictlyIncreasing(this IReadOnlyList<double> item)
        {
            Ensure<ArgumentException>(item.Count != 0);

            for (int i = 0; i < item.Count - 1; i++)
            {
                if (item[i] >= item[i + 1]) return false;
            }

            return true;
        }

        /// <exception cref="ArgumentException">
        /// Array length cannot be 0.</exception>
        public static bool IsStrictlyDecreasing(this IReadOnlyList<double> item)
        {
            Ensure<ArgumentException>(item.Count != 0);

            for (int i = 0; i < item.Count - 1; i++)
            {
                if (item[i] <= item[i + 1]) return false;
            }

            return true;
        }
    }
}
