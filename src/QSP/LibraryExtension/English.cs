using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class English
    {
        public static string Combined(this IReadOnlyList<string> items)
        {
            var count = items.Count;
            if (count == 1) return items[0];
            return string.Join(", ", items.Take(count - 1)) + " and " + items[count - 1];
        }

        public static string Combined(params string[] items)
        {
            return ((IReadOnlyList<string>)items).Combined();
        }
    }
}