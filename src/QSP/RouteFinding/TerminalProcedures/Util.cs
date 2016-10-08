using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.TerminalProcedures
{
    public static class Util
    {
        public static List<T> WithoutFirstAndLast<T>(
            this IReadOnlyList<T> item)
        {
            return item.Skip(1).Take(item.Count - 2).ToList();
        }
    }
}
