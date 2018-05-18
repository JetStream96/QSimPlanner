using QSP.AviationTools.Coordinates;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class Util
    {
        /// <summary>
        /// Try to parse the waypoint id as a coordinate. If success, returns
        /// the coordinate in ... format (e.g. 4820N15054E). Otherwise, returns 
        /// the waypoint id.
        /// </summary>
        public static string FormatWaypointId(this string id)
        {
            var c = Formatter.ParseLatLon(id);
            if (c == null) return id;
            return FormatDegMinNoSymbol.ToString(c);
        }

        public static List<string> FormatWaypointIds(this IEnumerable<string> w)
        {
            return w.Select(FormatWaypointId).ToList();
        }
    }
}
