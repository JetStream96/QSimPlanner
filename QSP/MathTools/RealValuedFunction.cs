using System;
using static QSP.LibraryExtension.LibraryExtension;

namespace QSP
{
    public class RealValuedFunction
    {
        public delegate double RealFcn(double u);
        //should be the function returning a real value
        private RealFcn f;

        public RealValuedFunction(RealFcn func)
        {
            f = func;
        }

        public double ValueAt(double t)
        {
            return f(t);
        }

        public double Integrate(double lowerLimit, double upperLimit, double delta)
        {
            //numerical integration

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

            for (int i = 0; i <= numIntervals - 2; i++)
            {
                sum += delta * f(lowerLimit + (i + 1.0 / 2) * delta);
            }

            sum += f((lowerLimit + (numIntervals - 1)) * delta) * (upperLimit - lowerLimit - (numIntervals - 1) * delta);

            if (swapped)
            {
                sum = -sum;
            }

            return sum;
        }

    }
}
