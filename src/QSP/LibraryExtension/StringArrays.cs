using System;
using System.Linq;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class StringArrays
    {
        public static double[] ToDoubles(this IEnumerable<string> item)
        {
            return item.Select(s => Convert.ToDouble(s)).ToArray();
        }
    }
}
