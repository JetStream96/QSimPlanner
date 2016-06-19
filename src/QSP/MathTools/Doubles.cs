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
    }
}
