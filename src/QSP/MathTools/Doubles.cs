using System;

namespace QSP.MathTools
{
    public static class Doubles
    {
        public static bool IsInteger(double x, double epsilon)
        {
            return Math.Abs(x % 1) < epsilon;
        }

        public static int RoundToInt(double x)
        {
            return (int)Math.Round(x);
        }

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

            return Math.Acos(x);
        }
    }
}
