using QSP.AviationTools.Coordinates;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class Util
    {
        /// <summary>
        /// If the waypoint is in format like 1030N, then return id. Otherwise, try 
        /// to parse the waypoint id as a coordinate. If success, returns
        /// the coordinate in ... format (e.g. 4820N15054E). Otherwise, returns 
        /// the id.
        /// </summary>
        public static string FormatWaypointId(this string id)
        {
            if (Format5Letter.Parse(id) != null) return id;
            var c = Formatter.ParseLatLon(id);
            if (c == null) return id;
            return FormatDegMinNoSymbol.ToString(c);
        }
        
        public static List<string> FormatWaypointIds(this IEnumerable<string> id)
        {
            return id.Select(FormatWaypointId).ToList();
        }
    }
}
