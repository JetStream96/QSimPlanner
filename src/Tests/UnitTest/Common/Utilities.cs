using System;

namespace UnitTest.Common
{
    public static class Utilities
    {
        public static bool WithinPrecisionPercent(
            double expected, double actual, double tolerancePercent)
        {
            return Math.Abs(actual - expected) <=
                actual * tolerancePercent / 100.0;
        }
    }
}
