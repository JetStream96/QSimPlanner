using System;
using QSP.RouteFinding.Routes;
using static QSP.LibraryExtension.Types;

namespace QSP.RouteFinding.FileExport.Providers
{
    /// <summary>
    /// This is exactly the same format as FlightFactor 777.
    /// </summary>
    public static class AerosoftAirbusProvider
    {
        /// <summary>
        /// Get string of the flight plan to export.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static string GetExportText(Route route)
        {
            if (route.Count < 2) throw new ArgumentException();
            var from = route.FirstWaypoint.ID;
            var to = route.LastWaypoint.ID;
            var lines = List(
                "[CoRte]",
                $"ArptDep={from.Substring(0, 4)}",
                $"ArptArr={to.Substring(0, 4)}",
                $"RwyDep={from}",
                $"RwyArr={to}");

            var current = route.First;
            var index = 1;
            while (current.Next != route.Last)
            {
                var next = current.Next;
                var airway = current.Value.AirwayToNext.Airway;
                var isDirect = (index == 1 || airway == "DCT");
                var currentWpt = current.Value.Waypoint;
                var nextWpt = next.Value.Waypoint;

                if (isDirect)
                {
                    var lat = nextWpt.Lat.ToString("0.000000");
                    var lon = nextWpt.Lon.ToString("0.000000");
                    lines.Add($"DctWpt{index}={nextWpt.ID}");
                    lines.Add($"DctWpt{index}Coordinates={lat},{lon}");
                }
                else
                {
                    lines.Add($"Airway{index}={airway}");
                    lines.Add($"Airway{index}FROM={currentWpt.ID}");
                    lines.Add($"Airway{index}TO={nextWpt.ID}");
                }

                current = current.Next;
                index++;
            }

            lines.Add("");
            return string.Join("\n", lines);
        }
    }
}
