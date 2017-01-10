using System;
using static System.Math;

namespace QSP.MathTools
{
    public static class Numbers
    {
        public static bool IsInteger(double x, double epsilon) => Abs(x % 1) < epsilon;

        public static int RoundToInt(double x) => (int)Round(x);

        public static int FloorInt(double x) => (int)Floor(x);

        /// <summary>
        /// To prevent rounding errors from causing troubles when evaluating
        /// Acos.
        /// </summary>
        public static double SafeAcos(double x)
        {
            if (x < -1.0)
            {
                x = -1.0;
            }
            else if (x > 1.0)
            {
                x = 1.0;
            }

            return Acos(x);
        }

        public static double Min(double x, double y, double z)
        {
            return Math.Min(Math.Min(x, y), z);
        }

        /// <summary>
        /// If the value if smaller than min, returns min.
        /// If the value if larger than max, returns max.
        /// Otherwise returns value.
        /// </summary>
        public static int LimitToRange(int value, int min, int max)
        {
            return Math.Min(Max(min, value), max);
        }

        /// <summary>
        /// If the value if smaller than min, returns min.
        /// If the value if larger than max, returns max.
        /// Otherwise returns value.
        /// </summary>
        public static double LimitToRange(double value, double min, double max)
        {
            return Math.Min(Max(min, value), max);
        }

        public static double Pow(double x, int pow)
        {
            double result = 1.0;
            bool powIsNegative = pow < 0;
            pow = Abs(pow);

            while (pow != 0)
            {
                if ((pow & 1) == 1) result *= x;
                x *= x;
                pow >>= 1;
            }

            if (powIsNegative) result = 1.0 / result;
            return result;
        }
    }
}
