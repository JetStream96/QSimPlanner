using QSP.AviationTools.Airac;
using QSP.AviationTools.Coordinates;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.NavData;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Navaids;
using System;
using System.Linq;
using static QSP.LibraryExtension.Types;
using static System.Math;

namespace QSP.RouteFinding.FileExport.Providers
{
    /// <summary>
    /// Implements the "1100 version" format. Supports x-plane 11 and up.
    /// Specs: https://developer.x-plane.com/article/flightplan-files-v11-fms-file-format/
    /// </summary>
    public static class Xplane11Provider
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
            var navdatapath = OptionManager.ReadFromFile().NavDataLocation;
            var cycle = AiracTools.AiracCyclePeriod(navdatapath);
            var to = route.LastWaypoint;
            var s = @"I
1100 Version
CYCLE " + cycle.Cycle;

            s += "\r\nADEP " + from.ID.Substring(0, 4);
            s += "\r\nDEPRWY RW" + from.ID.Substring(4);
            var sid = route.First.Value.AirwayToNext.Airway;
            if (sid != "DCT")
            {
                var sidtrans = sid.Split('.');

                s += "\r\nSID " + sidtrans[0];

                if (sidtrans.Length > 1)
                {
                    s += "\r\nSIDTRANS " + sidtrans[1];
                }
            }

            s += "\r\nADES " + to.ID.Substring(0, 4);
            s += "\r\nDESRWY RW" + to.ID.Substring(4);
            var star = route.Last.Previous.Value.AirwayToNext.Airway;
            if (star != "DCT")
            {
                var startrans = star.Split('.');

                s += "\r\nSTAR " + startrans[0];

                if (startrans.Length > 1)
                {
                    s += "\r\nSTARTRANS " + startrans[1];
                }
            }

            s += "\r\nNUMENR " + (route.Count);
            

            var firstLine = GetLine(from.ID.Substring(0, 4), "ADEP", from, 1);
            var lastLine = GetLine(to.ID.Substring(0, 4), "ADES", to, 1);
            var middleLines = route.WithoutFirstAndLast().Select(n =>
            {
                var w = n.Waypoint;
                var a = n.AirwayToNext.Airway;
                var id = w.ID;
                var navaid = navaids.Find(id, w);
                if (navaid != null && navaid.IsVOR) return GetLine(id, a, w, 3);
                if (navaid != null && navaid.IsNDB) return GetLine(id, a, w, 2);

                var coordinate = id.ParseLatLon();
                if (coordinate == null || Format5Letter.Parse(w.ID) != null)
                {
                    return GetLine(id, a, w, 11);
                }

                return GetLine(coordinate.FormatLatLon(), a, w, 28);
            });

            var lines = List(s, firstLine)
                .Concat(middleLines)
                .Concat(lastLine)
                .Concat("");

            return string.Join("\n", lines);
        }

        // Types:
        // 1 - Airport ICAO
        // 2 - NDB
        // 3 - VOR
        // 11 - Fix
        // 28 - Lat/Lon Position
        private static string GetLine(string id, string a, ICoordinate c, int type)
        {
            var lat = c.Lat.ToString("0.000000");
            var lon = c.Lon.ToString("0.000000");
            var airway = a == "DCT" ? "DRCT" : a;
            return $"{type} {id} {airway} 0 {lat} {lon}";
        }
    }
}
