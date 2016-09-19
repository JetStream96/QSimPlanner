using System;

namespace QSP.MathTools
{
    public static class Integers
    {
        public static double Pow(double x, int pow)
        {
            double result = 1.0;
            bool powIsNegative = pow < 0;
            pow = Math.Abs(pow);

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
