using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System;
using System.Linq;
using static CommonLibrary.LibraryExtension.Types;
using static System.Linq.Enumerable;

namespace QSP.RouteFinding.FileExport.Providers
{
    // TODO: Add test.
    public static class Ifly737Provider
    {
        public static string GetExportText(Route route, AirportManager airports)
        {
            if (route.Count < 2) throw new ArgumentException();
            var linesPart1 = new[]
            {
                route.FirstWaypoint.ID.Substring(0, 4),
                route.FirstWaypoint.ID.Substring(4),
                route.LastWaypoint.ID.Substring(0,4)
            }.Concat(Repeat("", 7))
             .Concat(new[] { "-1", "", "", "", "", "0" });

            var node = route.First.Next;
            var part2 = List(("DIRECT", node.Value.Waypoint));
            node = node.Next;

            while (node != null)
            {
                var a = node.Previous.Value.Neighbor.Airway;
                var airway = a == "DCT" ? "DIRECT" : a;
                part2.Add((a, node.Value.Waypoint));
                node = node.Next;
            }

            var linesPart2 = part2.Select(v =>
            {
                var (airway, wpt) = v;
                var words = List(
                    airway,
                    airway == "DIRECT" ? "3" : "2",
                    wpt.ID,
                    string.Format("{0} {1}", wpt.Lat, wpt.Lon),
                    "0",
                    "0",
                    "Put heading here"); //TODO: calculate true heading towards next wpt
                return string.Join(",", words) +
                       ",0,0,1,-1,0.000,0,-1000,-1000,-1,-1,-1,0,0,000.00000,0,0,,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1,-1,-1000,0,-1000,-1000,0";
            });

            return string.Join(",\n", linesPart1.Concat(linesPart2));
        }
    }
}