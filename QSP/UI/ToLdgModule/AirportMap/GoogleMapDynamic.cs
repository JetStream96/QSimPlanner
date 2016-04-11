namespace QSP.UI.ToLdgModule.AirportMap
{
    public static class GoogleMapDynamic
    {
        private static string originalHtml = @"<!DOCTYPE html>
        <html>
        <head>
        <script
        src=""http://maps.googleapis.com/maps/api/js"">
        </script>
        
        <script>
        function initialize() {
          var mapProp = {
            center:new google.maps.LatLng(51.508742,-0.120850),
            zoom:13,
            mapTypeId:google.maps.MapTypeId.ROADMAP
          };
          var map=new google.maps.Map(document.getElementById(""googleMap""), mapProp);
        }
        google.maps.event.addDomListener(window, 'load', initialize);
        </script>
        </head>
        
        <body>
        <div id=""googleMap"" style=""width:500px;height:380px;""></div>
        
        </body>
        </html>";


        /// <summary>
        /// Return a string containg the HTML code, centered at the given lat/lon.
        /// </summary>
        public static string GetHtml(double lat,
                                     double lon,
                                     int windowWidth,
                                     int windowHeight)
        {
            return originalHtml
                   .Replace("51.508742,-0.120850", lat.ToString() + "," + lon.ToString())
                   .Replace("width:500px;height:380px",
                            "width:" + (windowWidth - 10).ToString() +
                            "px;height:" + (windowHeight - 20).ToString() + "px");
        }
    }
}
