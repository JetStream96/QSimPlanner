using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class Util
    {
        /// <summary>
        /// If the waypoint is in WaypointList, then return id. Otherwise, try 
        /// to parse the waypoint id as a coordinate. If success, returns
        /// the coordinate in ... format (e.g. 4820N15054E). Otherwise, returns 
        /// the waypoint id.
        /// </summary>
        public static string FormatWaypointId(this Waypoint w, ExportInput ei)
        {
            var id = w.ID;
            if (w.IsInWptList(ei)) return id;
            var c = Formatter.ParseLatLon(id);
            if (c == null) return id;
            return FormatDegMinNoSymbol.ToString(c);
        }

        public static bool IsInWptList(this Waypoint w, ExportInput ei)
        {
            var id = w.ID;
            var wptList = ei.Waypoints;
            return wptList.FindAllById(id).Any(i => wptList[i].Distance(w) <= 1);
        }

        public static List<string> FormatWaypointIds(this IEnumerable<Waypoint> w,
            ExportInput ei)
        {
            return w.Select(x => x.FormatWaypointId(ei)).ToList();
        }
    }
}
