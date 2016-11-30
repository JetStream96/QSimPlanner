using QSP.LibraryExtension;
using static System.Math;

namespace QSP.MathTools.TablesNew
{
    public static class ITableExtension
    {
        public static bool Equals(this ITable x, ITable y, double delta = 0.0)
        {
            if (x.Dimension != y.Dimension) return false;

            // Both are doubles.
            if (x.Dimension == 0) return Abs(x.ValueAt(null) - y.ValueAt(null)) <= delta;
            if (!DoubleArrayCompare.Equals(x.XValues, y.XValues, delta)) return false;

            for (int i = 0; i < x.FValues.Count; i++)
            {
                if (!Equals(x.FValues[i], y.FValues[i], delta)) return false;
            }

            return true;
        }
    }
}