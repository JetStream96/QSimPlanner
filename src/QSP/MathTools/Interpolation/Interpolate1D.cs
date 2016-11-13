using System;
using static QSP.MathTools.Interpolation.Common;

namespace QSP.MathTools.Interpolation
{
    public static class Interpolate1D
    {
        public static double Interpolate(double x0, double x1, double f0, double f1, double x)
        {
            if (x0 == x1)
            {
                if (x0 == x && f0 == f1) return f0;
                throw new ArgumentException("x0 cannot be equal to x1.");
            }

            return f0 + (f1 - f0) * (x - x0) / (x1 - x0);
        }

        public static double Interpolate(double[] xArray, double[] f, double x)
        {
            return Interpolate(xArray, f, GetIndex(xArray, x), x);
        }

        public static double Interpolate(double[] xArray, double[] f, int index, double x)
        {
            return Interpolate(xArray[index], xArray[index + 1], f[index], f[index + 1], x);
        }
    }
}
