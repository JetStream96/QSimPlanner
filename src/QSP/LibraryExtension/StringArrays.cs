using System;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class StringArrays
    {
        public static double[] ToDoubles(this string[] item)
        {
            return item.Select(s => Convert.ToDouble(s)).ToArray();
        }
    }
}
