using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class Util
    {
        /// <summary>
        /// Try to parse the waypoint id as a coordinate. If success, returns
        /// the coordinate in ... format (e.g. 4820N15054E). Otherwise, returns 
        /// the waypoint id.
        /// </summary>
        public static string FormatWaypointId(this Waypoint w)
        {
            var c = Formatter.ParseLatLon(w.ID);
            if (c == null) return w.ID;
            return FormatDegMinNoSymbol.ToString(c);
        }
    }
}
