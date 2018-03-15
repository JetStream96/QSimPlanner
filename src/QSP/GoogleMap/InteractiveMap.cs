using System.IO;

namespace QSP.GoogleMap
{
    public static class InteractiveMap
    {
        /// <summary>
        /// Return a string containg the HTML code, centered at the
        /// given lat/lon. Returns null if failed.
        /// </summary>
        public static string GetHtml(double lat, double lon)
        {
            try
            {
                return File.ReadAllText("./GoogleMap/airport-map-template.html")
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
