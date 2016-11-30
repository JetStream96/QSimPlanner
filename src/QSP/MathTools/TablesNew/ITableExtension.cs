using System.Linq;
using QSP.LibraryExtension;

namespace QSP.MathTools.TablesNew
{
    public static class ITableExtension
    {
        public static bool Equals(this ITable x, ITable y, double delta = 0.0)
        {
            if (x.Dimension != y.Dimension ||
                !DoubleArrayCompare.Equals(x.XValues, y.XValues, delta))
            {
                return false;
            }

            if (x.Dimension == 0) return true;

            for (int i = 0; i < x.FValues.Count; i++)
            {
                if (!Equals(x.FValues[i], y.FValues[i], delta)) return false;
            }

            return true;
        }
    }
}