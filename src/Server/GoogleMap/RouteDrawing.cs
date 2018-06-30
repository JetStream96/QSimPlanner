using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace Server.GoogleMap
{
    public static class RouteDrawing
    {
        private class PlotPoint
        {
            public string Lat, Lon, Id;
        }

        private static string MapDrawString(IReadOnlyList<PlotPoint> points,
            string CenterLat, string CenterLon)
        {
            var latLons = string.Join(",", points.Select(r => $"[{r.Lat}, {r.Lon}]"));
            var texts = string.Join(",",
                points.Select(r => $"\"<p>Ident: {r.Id}<br>Lat: {r.Lat}<br>Lon: {r.Lon}</p>\""));

            var replacement = $"var latLons = [{latLons}];\n" +
                              $"var texts = [{texts}];\n" +
                              $"var center = [{CenterLat}, {CenterLon}];\n";

            return Regex.Replace(template,
                                 @"// DATA START.+?DATA END",
                                 replacement,
                                 RegexOptions.Singleline);
        }

        private static string template;

        public static void LoadTemplate() =>
            template = File.ReadAllText(HostingEnvironment.MapPath(
                "~/GoogleMap/route-map-template.html"));

        /// <summary>
        /// Request format: [url]/map/route?LatLonId=10,22,id0;8,13,id1&Center=9,18
        /// </summary>
        public static string Respond(NameValueCollection queryStrings)
        {
            try
            {
                var center = queryStrings["Center"].Split(',');
                var centerLat = center[0];
                var centerLon = center[1];
                var latLonId = queryStrings["LatLonId"].Split(';').Select(x =>
                 {
                     var y = x.Split(',');
                     return new PlotPoint()
                     {
                         Lat = y[0],
                         Lon = y[1],
                         Id = y[2]
                     };
                 });

                return MapDrawString(latLonId.ToList(), centerLat, centerLon)
                    .Replace("{API_KEY}", Maps.ApiKey);
            }
            catch
            {
                return null;
            }
        }
    }
}
