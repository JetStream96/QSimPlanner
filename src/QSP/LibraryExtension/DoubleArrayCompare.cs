using System;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class DoubleArrayCompare
    {
        public static bool Equals(this IReadOnlyList<double> item,
            IReadOnlyList<double> other, double delta = 0.0)
        {
            if (item == null ||
                other == null ||
                item.Count != other.Count)
            {
                return false;
            }

            for (int i = 0; i < item.Count; i++)
            {
                if (Math.Abs(item[i] - other[i]) >= delta) return false;
            }

            return true;
        }

        public static bool Equals(this double[][] item, double[][] other, double delta)
        {
            if (item == null ||
                other == null ||
                item.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < item.Length; i++)
            {
                if (Equals(item[i], other[i], delta) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Equals(this double[][][] item, double[][][] other, double delta)
        {
            if (item == null ||
                other == null ||
                item.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < item.Length; i++)
            {
                if (Equals(item[i], other[i], delta) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Equals(this double[][][][] item, double[][][][] other, double delta)
        {
            if (item == null ||
                other == null ||
                item.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < item.Length; i++)
            {
                if (Equals(item[i], other[i], delta) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
