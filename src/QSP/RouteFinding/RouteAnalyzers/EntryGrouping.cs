using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QSP.RouteFinding.RouteAnalyzers.AnalyzerWithCommands;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Group the route into several parts to seperate "AUTO" and "RAND"
    // from actual route.
    // 
    // Example 1. 'A B AUTO C D RAND E' is grouped into:
    // 'A B', 'AUTO', 'C D', 'RAND', 'E'
    //
    // Example 2. 'AUTO A B' is grouped into:
    // 'AUTO', 'A B'

    public static class EntryGrouping
    {
        public static IReadOnlyList<RouteSegment> Group(RouteString route)
        {
            var result = new List<RouteSegment>();
            var queue = new Queue<string>(route);

            while (queue.Count > 0)
            {
                var seg = GetNextSegment(queue);

                if (seg.IsAuto || seg.IsRand || seg.RouteString.Count > 0)
                {
                    result.Add(seg);
                }
            }

            return result;
        }

        private static RouteSegment GetNextSegment(Queue<string> queue)
        {
            var item = queue.Dequeue();

            if (item == "AUTO") return RouteSegment.Auto();
            if (item == "RAND") return RouteSegment.Rand();

            string[] cmds = { "AUTO", "RAND" };
            var route = new List<string> { item };

            while (queue.Count > 0)
            {
                var peek = queue.Peek();
                if (cmds.Contains(queue.Peek())) break;
                route.Add(queue.Dequeue());
            }

            return new RouteSegment(new RouteString(route));
        }
    }
}
