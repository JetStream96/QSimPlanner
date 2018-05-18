using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QSP.AviationTools.Coordinates.FormatDegreeMinute;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class Fs9Provider
    {
        /// <summary>
        /// Returns the text of the file to export.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static string GetExportText(Route route, AirportManager airports)
        {
            if (route.Count < 2) throw new ArgumentException();

            var orig = route.FirstWaypoint;
            var origId = orig.ID;
            var origIcao = origId.Substring(0, 4);
            var origAirport = airports[origIcao];
            var origLatLonAlt = LatLonAlt(orig, origAirport.Elevation);
            var origLine = $"{origIcao}, A, {origLatLonAlt}, ";

            var dest = route.LastWaypoint;
            var destId = dest.ID;
            var destIcao = destId.Substring(0, 4);
            var destAirport = airports[destIcao];
            var destLatLonAlt = LatLonAlt(dest, destAirport.Elevation);
            var destLine = $"{destIcao}, A, {destLatLonAlt}, ";

            var result = new StringBuilder();
            result.AppendLine("[flightplan]");
            result.AppendLine($"title={origIcao} to {destIcao}");
            result.AppendLine($"description={origIcao}, {destIcao}");
            result.AppendLine(@"type=IFR
routetype=3
cruising_altitude=10000");
            result.AppendLine($"departure_id={origIcao}, {origLatLonAlt}");
            result.AppendLine($"destination_id={destIcao}, {destLatLonAlt}");
            result.AppendLine($"departure_name={origAirport.Name}");
            result.AppendLine($"destination_name={destAirport.Name}");

            var lines = new List<string>();
            lines.Add(origLine);

            var wptLines = route
                .WithoutFirstAndLast()
                .Select(n =>
                {
                    var id = n.Waypoint.ID.FormatWaypointId();
                    return $"{id}, I, {LatLonAlt(n.Waypoint, 0.0)}, ";
                });

            lines.AddRange(wptLines);
            lines.Add(destLine);
            ConvertLines(lines);
            lines.ForEach(i => result.AppendLine(i));

            return result.ToString();
        }

        private static void ConvertLines(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = $"waypoint.{i}={lines[i]}";
            }
        }

        public static string LatLonAlt(ICoordinate latLon, double altitudeFt)
        {
            var format = "F2";
            var lat = LatToString(latLon.Lat, format);
            var lon = LonToString(latLon.Lon, format);
            var alt = altitudeFt.ToString(format);

            if (alt[0] != '-') alt = '+' + alt;
            alt = alt[0] + alt.Substring(1).PadLeft(9, '0');

            return (lat + ", " + lon + ", " + alt).Replace('°', '*');
        }
    }
}
