using System;
using static QSP.LibraryExtension.Utilities;

namespace QSP.MathTools
{
    public static class Integration
    {
        /// <summary>
        /// Numerical integration
        /// </summary>
        public static double Integrate(
            Func<double, double> f,
            double lowerLimit, double upperLimit, double delta)
        {
            bool swapped = false;

            if (lowerLimit == upperLimit)
            {
                return 0.0;
            }
            else if (lowerLimit > upperLimit)
            {
                Swap(ref lowerLimit, ref upperLimit);
                swapped = true;
            }

            double sum = 0.0;
            int numIntervals = (int)Math.Ceiling((upperLimit - lowerLimit) / delta);

            for (int i = 0; i < numIntervals - 1; i++)
            {
                sum += delta * f(lowerLimit + (i + 0.5) * delta);
            }

            sum += f((lowerLimit + (numIntervals - 1)) * delta) *
                (upperLimit - lowerLimit - (numIntervals - 1) * delta);

            if (swapped)
            {
                sum = -sum;
            }

            return sum;
        }
    }
}
