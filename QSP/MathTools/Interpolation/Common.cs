namespace QSP.MathTools.Interpolation
{
    public static class Common
    {
        public static int GetIndex(double[] array, double value)
        {
            if (array[1] > array[0])
            {
                return getIndexIncreasing(array, value);
            }
            else
            {
                return getIndexDecreasing(array, value);
            }
        }

        // Not the fastest solution. 
        // Use binary search if performance is a concern.
        private static int getIndexIncreasing(double[] array, double value)
        {
            if (value <= array[0])
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
            if (value >= array[0])
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
    }
}
