using QSP.LibraryExtension;

namespace QSP.MathTools.Tables
{
    public static class Validator
    {
        public static bool IsValidAxis(this double[] item)
        {
            return item.IsStrictlyIncreasing() || item.IsStrictlyDecreasing();
        }
    }
}
