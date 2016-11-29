using System.Collections.Generic;

namespace QSP.MathTools.Interpolation
{
    public static class Common
    {
        public static int GetIndex(IReadOnlyList<double> array, double value)
        {
            if (array[1] > array[0]) return GetIndexIncreasing(array, value);
            return GetIndexDecreasing(array, value);
        }

        // TODO: Not the best solution. 
        // Use binary search if performance is a concern.
        private static int GetIndexIncreasing(IReadOnlyList<double> array, double value)
        {
            if (value <= array[0])
            {
                return 0;
            }

            int len = array.Count;

            for (int i = 0; i < len - 1; i++)
            {
                if (value >= array[i] && value <= array[i + 1])
                {
                    return i;
                }
            }
            return len - 2;
        }

        private static int GetIndexDecreasing(IReadOnlyList<double> array, double value)
        {
            if (value >= array[0])
            {
                return 0;
            }

            int len = array.Count;

            for (int i = 0; i < len - 1; i++)
            {
                if (value <= array[i] && value >= array[i + 1])
                {
                    return i;
                }
            }
            return len - 2;
        }
    }
}
