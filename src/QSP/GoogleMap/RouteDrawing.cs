using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace QSP.GoogleMap
{
    public static class RouteDrawing
    {
        private static readonly string TemplatePath = "./GoogleMap/route-map-template.html";

        public static string MapDrawString(IReadOnlyList<Waypoint> route)
        {
            var template = File.ReadAllText(TemplatePath);

            var latLons = string.Join(",", route.Select(r => $"[{r.Lat}, {r.Lon}]"));
            var texts = string.Join(",", 
                route.Select(r => $"\"Ident: {r.ID}\\nLat:{r.Lat}\\nLon:{r.Lon}\""));
            
            // Center of the map
            var center = GetCenter(route);
            var replacement = $"var latLons = [{latLons}];\n" +
                              $"var texts = [{texts}];\n" +
                              $"var center = [{center.Lat}, {center.Lon}];\n";

            return Regex.Replace(template, 
                                 @"// DATA START.+?DATA END",
                                 replacement, 
                                 RegexOptions.Singleline);
        }
        
        private static ICoordinate GetCenter(IReadOnlyList<Waypoint> route)
        {
            if (route.Count < 2) throw new ArgumentException();
            var totalDis = route.TotalDistance();
            double dis = 0.0;
            Waypoint last = null;

            foreach (var i in route)
            {
                if (last != null)
                {
                    dis += last.Distance(i);
                    if (dis * 2.0 >= totalDis) return i;
                }

                last = i;
            }

            return last;
        }
    }
}
