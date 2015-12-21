using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    public static  class Lists
    {
        private static void Resize<T>(this List<T> item, int newSize)
        {
            item.AddRange(Enumerable.Repeat(default(T), newSize - item.Count));
        }

        public static List<T> WithoutDuplicates<T>(this List<T> item)
        {
            return item.Distinct().ToList();
        }
    }
}
