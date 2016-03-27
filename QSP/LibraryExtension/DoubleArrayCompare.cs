using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    public static class DoubleArrayCompare
    {
        public static bool Equals(this double[] item, double[] other, double delta)
        {
            if (item == null ||
                other == null ||
                item.Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < item.Length; i++)
            {
                if (Math.Abs(item[i] - other[i]) >= delta)
                {
                    return false;
                }
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
    }
}
