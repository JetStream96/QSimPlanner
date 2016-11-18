using System;

namespace QSP.MathTools.TableNew
{
    public class Util
    {
        public static double Interpolate(double x0, double x1, double x, double f0, double f1)
        {
            if (x0 == x1)
            {
                if (x0 == x && f0 == f1) return f0;
                throw new ArgumentException("x0 cannot be equal to x1.");
            }

            return f0 + (f1 - f0) * (x - x0) / (x1 - x0);
        }

        public static bool IsIncreasing(double[] arr)
        {
            var len = arr.Length;
            if (len < 2) throw new ArgumentException("Too few elements.");
            var increasing = arr[1] > arr[0];

            for (var i = 0; i < len - 1; i++)
            {
                if (increasing ^ (arr[i] <= arr[i + 1]))
                {
                    throw new ArgumentException("Array must be either increasing or decreasing.");
                }
            }

            return increasing;
        }
    }
}