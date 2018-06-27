using System.Collections.Specialized;
using System.IO;
using System;
using System.Web.Hosting;

namespace Server.GoogleMap
{
    public static class InteractiveMap
    {
        private static string template;

        public static void LoadTemplate() =>
            template = File.ReadAllText(HostingEnvironment.MapPath(
                "~/GoogleMap/airport-map-template.html"));

        public static string Respond(NameValueCollection queryStrings)
        {
            try
            {
                var lat = double.Parse(queryStrings["lat"]);
                var lon = double.Parse(queryStrings["lon"]);
                return GetHtml(lat, lon);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Return a string containg the HTML code, centered at the
        /// given lat/lon. Returns null if failed.
        /// </summary>
        public static string GetHtml(double lat, double lon)
        {
            try
            {
                return template.Replace("{API_KEY}", Maps.ApiKey)
                               .Replace("{lat}", lat.ToString())
                               .Replace("{lon}", lon.ToString());
            }
            catch
            {
                return null;
            }
        }
    }
}
