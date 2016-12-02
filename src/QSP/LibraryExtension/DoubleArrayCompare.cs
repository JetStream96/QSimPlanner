using System;

namespace QSP.LibraryExtension
{
    public static class DoubleArrayCompare
    {
        public static bool Equals(this double[] item, double[] other, double delta)
        {
            if (!SameLength(item, other)) return false;

            for (int i = 0; i < item.Length; i++)
            {
                if (Math.Abs(item[i] - other[i]) >= delta) return false;
            }

            return true;
        }

        public static bool Equals(this double[][] item, double[][] other, double delta)
        {
            if (!SameLength(item, other)) return false;

            for (int i = 0; i < item.Length; i++)
            {
                if (Equals(item[i], other[i], delta) == false) return false;
            }

            return true;
        }

        public static bool Equals(this double[][][] item, double[][][] other, double delta)
        {
            if (!SameLength(item, other)) return false;

            for (int i = 0; i < item.Length; i++)
            {
                if (Equals(item[i], other[i], delta) == false) return false;
            }

            return true;
        }

        public static bool Equals(this double[][][][] item, double[][][][] other, double delta)
        {
            if (!SameLength(item, other)) return false;

            for (int i = 0; i < item.Length; i++)
            {
                if (Equals(item[i], other[i], delta) == false) return false;
            }

            return true;
        }

        private static bool SameLength(Array x, Array y)
        {
            return x != null && y != null && x.Length == y.Length;
        }
    }
}
