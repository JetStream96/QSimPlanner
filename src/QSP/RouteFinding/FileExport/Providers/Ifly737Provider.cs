using QSP.MathTools;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Types;
using static System.Linq.Enumerable;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class Ifly737Provider
    {
        // In the waypoint part, each line contain a waypoint and a true heading. 
        // E.g.
        // ...
        // ... WPT0 ... heading0
        // ... WPT1 ... heading1
        // 
        // heading0 is the direct heading from WPT0 to WPT1, at WPT0.
        
        public static string GetExportText(Route route)
        {
            if (route.Count < 2) throw new ArgumentException();
            var linesPart1 = List(
                route.FirstWaypoint.ID.Substring(0, 4),
                route.LastWaypoint.ID.Substring(0, 4),
                route.FirstWaypoint.ID.Substring(4)
            ).Concat(Repeat("", 7))
             .Concat(List("-1", "", "", "", "", "0"));

            var linesPart2 = new List<string>();
            var prev = route.First;
            var current = prev.Next;

            while (current != route.Last)
            {
                var airway = prev.Value.AirwayToNext.Airway;
                bool isDirect = (prev == route.First) || (airway == "DCT");

                var w = current.Value.Waypoint;
                var heading = EarthGeometry.TrueHeading(prev.Value.Waypoint, w);

                linesPart2.Add(string.Format(
                    "{0},{1},0, {2} {3},0,0, {4},0,0,1,-1,0.000,0,-1000,-1000,-1,-1,-1,0,0,000.00000,0,0,,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1000,0",
                    isDirect ? "DIRECT,3" : $"{airway},2",
                    w.ID.FormatWaypointId(),
                    w.Lat.ToString("0.000000"),
                    w.Lon.ToString("0.000000"),
                    heading.ToString("0.00000")));

                prev = current;
                current = current.Next;
            }

            linesPart2.Add("");

            return string.Join(",\n", linesPart1.Concat(linesPart2));
        }
    }
}
