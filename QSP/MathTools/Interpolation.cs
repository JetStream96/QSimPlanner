using System.Linq;
using System;

namespace QSP.MathTools
{
    public static class Interpolation
    {
        #region Common Tools

        public enum ArrayOrder
        {
            Increasing,
            Decreasing
        }

        public static ArrayOrder GetOrder(this double[] item)
        {
            return item[1] > item[0] ? ArrayOrder.Increasing : ArrayOrder.Decreasing;
        }

        private static int getIndex(double[] array, double value, ArrayOrder arrayOrder)
        {
            if (arrayOrder == ArrayOrder.Increasing)
            {
                return getIndexIncreasing(array, value);
            }
            else
            {
                return getIndexDecreasing(array, value);
            }
        }

        private static int getIndexIncreasing(double[] array, double value)
        {
            if (value <= array.First())
            {
                return 0;
            }

            int len = array.Length;

            for (int i = 0; i < len - 1; i++)
            {
                if (value >= array[i] && value <= array[i + 1])
                {
                    return i;
                }
            }
            return len - 2;
        }

        private static int getIndexDecreasing(double[] array, double value)
        {

            if (value >= array.First())
            {
                return 0;
            }

            int len = array.Length;

            for (int i = 0; i < len - 1; i++)
            {
                if (value <= array[i] && value >= array[i + 1])
                {
                    return i;
                }
            }
            return len - 2;
        }

        #endregion

        #region 1-D 

        public static double Interpolate(double x0, double x1, double x, double y0, double y1)
        {
            if (x0 == x1 && x0 == x)
            {
                if (y0 == y1)
                {
                    return y0;
                }
                throw new ArgumentException("x, x0, and x1 are identical while y0 is different from y1.");
            }
            else
            {
                return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
            }
        }

        public static double Interpolate(double[] xArray, double x, double[] Value)
        {
            return Interpolate(xArray, x, Value, xArray.GetOrder());
        }

        public static double Interpolate(double[] xArray, double x, double[] Value, ArrayOrder xOrder)
        {
            int index = getIndex(xArray, x, xOrder);
            return Interpolate(xArray[index], xArray[index + 1], x, Value[index], Value[index + 1]);
        }

        // Use a subarray of a 2-D array as Value[].
        public static double Interpolate(double[] xArray, double x, double[,] Value, int firstIndexValueArray, ArrayOrder xOrder)
        {
            int index = getIndex(xArray, x, xOrder);
            return Interpolate(xArray[index], xArray[index + 1], x,
                Value[firstIndexValueArray, index], Value[firstIndexValueArray, index + 1]);
        }

        #endregion

        #region 2-D

        public static double Interpolate(double x0, double x1, double x, double y0, double y1, double y,
            double f_x0_y0, double f_x0_y1, double f_x1_y0, double f_x1_y1)
        {
            double i = 0;
            double j = 0;
            i = Interpolate(x0, x1, x, f_x0_y0, f_x1_y0);
            j = Interpolate(x0, x1, x, f_x0_y1, f_x1_y1);

            return Interpolate(y0, y1, y, i, j);
        }

        public static double Interpolate(double[] xArray, double[] yArray, double x, double y, double[,] Value,
            ArrayOrder xOrder, ArrayOrder yOrder)
        {
            int xIndex = getIndex(xArray, x, xOrder);

            double result1 = Interpolate(yArray , y, Value, xIndex, xOrder);
            double result2 = Interpolate(yArray, y, Value, xIndex + 1, xOrder);

            return Interpolate(xArray[xIndex], xArray[xIndex + 1], x, result1, result2);
        }

        #endregion

        #region 3-D

        public delegate double f_xIndex_yIndex(int x, int y);

        /// <summary>
        /// Interpolate with the given x and y.
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="f">A function from two integers to a double. Say at index a in xArray the value is x1, 
        /// and at index b in yArray the value is y1. Then f(a,b) must be the value of f corresponding to (x1,y1). </param>
        /// <param name="xOrder"></param>
        /// <param name="yOrder"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double Interpolate(double[] xArray, double[] yArray, double x, double y, f_xIndex_yIndex f, ArrayOrder xOrder, ArrayOrder yOrder)
        {
            int xIndex = getIndex(xArray, x, xOrder);
            int yIndex = getIndex(yArray, y, yOrder);

            return Interpolate(xArray[xIndex], xArray[xIndex + 1], x, yArray[yIndex], yArray[yIndex + 1], y,
                f(xIndex, yIndex), f(xIndex, yIndex + 1), f(xIndex + 1, yIndex), f(xIndex + 1, yIndex + 1));
        }

        public static double Interpolate(double[] xArray, double[] yArray, double[] zArray, double x, double y, double z, double[,,] f, ArrayOrder xOrder, ArrayOrder yOrder, ArrayOrder zOrder)
        {

            int xIndex = getIndex(xArray, x, xOrder);

            return Interpolate(xArray[xIndex], xArray[xIndex + 1], x, Interpolate(yArray, zArray, y, z, (b, c) => f[xIndex, b, c], yOrder, zOrder), Interpolate(yArray, zArray, y, z, (b, c) => f[xIndex + 1, b, c], yOrder, zOrder));

        }

        #endregion
    }

}

