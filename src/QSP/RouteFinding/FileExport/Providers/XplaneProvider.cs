using QSP.AviationTools.Coordinates;
using QSP.LibraryExtension;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Navaids;
using QSP.RouteFinding.Routes;
using System;
using System.Linq;
using static QSP.LibraryExtension.Types;
using static System.Math;

namespace QSP.RouteFinding.FileExport.Providers
{
    /// <summary>
    /// Implements the "3 version" format. Supports x-plane 8 to 11.
    /// Specs: https://flightplandatabase.com/dev/specification
    /// </summary>
    /// 
    /// For newer format, see https://developer.x-plane.com/?article=flightplan-files-v11-fms-file-format
    public static class XplaneProvider
    {
        /// <summary>
        /// Get string of the flight plan to export.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static string GetExportText(ExportInput input)
        {
            var (route, navaids, wptList) = (input.Route, input.Navaids, input.Waypoints);
            if (route.Count < 2) throw new ArgumentException();
            var from = route.FirstWaypoint;
            var to = route.LastWaypoint;
            var s = @"I
3 version
1
" + (route.Count - 1);


            var firstLine = GetLine(from.ID.Substring(0, 4), from, 1);
            var lastLine = GetLine(to.ID.Substring(0, 4), to, 1);
            var middleLines = route.WithoutFirstAndLast().Select(n =>
            {
                var w = n.Waypoint;
                var id = w.ID;
                var navaid = navaids.Find(id, w);
                if (navaid != null && navaid.IsVOR) return GetLine(id, w, 3);
                if (navaid != null && navaid.IsNDB) return GetLine(id, w, 2);

                var coordinate = id.ParseLatLon();
                if (coordinate == null || Format5Letter.Parse(w.ID) != null)
                {
                    return GetLine(id, w, 11);
                }

                return GetLine(coordinate.FormatLatLon(), w, 28);
            });

            var lines = List(s, firstLine)
                .Concat(middleLines)
                .Concat(lastLine)
                .Concat(Enumerable.Repeat("0 ---- 0 0.000000 0.000000", 4))
                .Concat("");

            return string.Join("\n", lines);
        }

        // Types:
        // 1 - Airport ICAO
        // 2 - NDB
        // 3 - VOR
        // 11 - Fix
        // 28 - Lat/Lon Position
        private static string GetLine(string id, ICoordinate c, int type)
        {
            var lat = c.Lat.ToString("0.000000");
            var lon = c.Lon.ToString("0.000000");
            return $"{type} {id} 0 {lat} {lon}";
        }

        public static string FormatLatLon(this ICoordinate c)
        {
            string Format(double d, bool isLat)
            {
                var sign = d >= 0 ? '+' : '-';
                var num = Abs(d).ToString("F3").PadLeft(isLat ? 6 : 7, '0');
                return sign + num;
            }

            return Format(c.Lat, true) + '_' + Format(c.Lon, false);
        }
    }
}
