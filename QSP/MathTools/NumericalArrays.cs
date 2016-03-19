using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.MathTools
{
    public static class NumericalArrays
    {
        /// <summary>
        /// Assuming the array is strictly increasing or decreasing,
        /// returns a bool.
        /// </summary>
        public static bool IsIncreasing(this double[] item)
        {
            return item[1] > item[0];
        }
    }
}
