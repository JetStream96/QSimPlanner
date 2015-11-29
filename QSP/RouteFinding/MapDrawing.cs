using System;
using System.Collections.Generic;
using System.Text;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding
{

    public static class MapDrawing
    {

        public static StringBuilder MapDrawString(Route rte, int width, int height)
        {

            rte.ExpandNats();
            var waypoints = rte.Waypoints;
            StringBuilder mapHtml = new StringBuilder();
            
            mapHtml.Append(@"<!DOCTYPE html>
            <html>
            <head>
            <meta http-equiv=""content-type"" content=""text/html; charset=utf-8"" />
             <style type=""text/css"">
               .labels {
                 color: red;
                 background-color: white;
                 font-family: ""Lucida Grande"", ""Arial"", sans-serif;
                 font-size: 10px;
                 font-weight: bold;
                 text-align: center;
                 width: 40px;
                 border: 2px solid black;
                 white-space: nowrap;
               }
             </style>
             <script type=""text/javascript"" src=""http://maps.googleapis.com/maps/api/js?v=3&amp;sensor=False""></script>
             <script type = ""text/javascript"" src=""http://google-maps-utility-library-v3.googlecode.com/svn/tags/markerwithlabel/1.1.9/src/markerwithlabel.js""></script>
             <script type=""text/javascript"">
            function initialize()
            {
            ");

            for (int i = 0; i <= waypoints.Count - 1; i++)
            {
                mapHtml.Append("var wpt" + i.ToString() + "=new google.maps.LatLng(" + waypoints[i].Lat + "," + waypoints[i].Lon + ");" + Environment.NewLine);
            }

            //now the center of map
            double c_lat = 0;
            double c_lon = 0;
            c_lat = (waypoints[0].Lat + waypoints[waypoints.Count - 1].Lat) / 2;
            c_lon = (waypoints[0].Lon + waypoints[waypoints.Count - 1].Lon) / 2;

            mapHtml.Append("var centerP" + "=new google.maps.LatLng(" + Convert.ToString(c_lat) + "," + Convert.ToString(c_lon) + ");" + Environment.NewLine);

            int zoomlevel = 6;

            mapHtml.Append("var mapProp = {" + Environment.NewLine + "  center:centerP," + Environment.NewLine + "  zoom:" + zoomlevel.ToString() + "," + Environment.NewLine + "  mapTypeId:google.maps.MapTypeId.ROADMAP" + Environment.NewLine + "  };" + Environment.NewLine + "var map=new google.maps.Map(document.getElementById(\"googleMap\"),mapProp);" + Environment.NewLine + "var myTrip=[");

            for (int i = 0; i <= waypoints.Count - 2; i++)
            {
                mapHtml.Append("wpt" + i.ToString() + ",");
            }
            mapHtml.Append("wpt" + (waypoints.Count - 1).ToString() + "];" + Environment.NewLine + "var flightPath=new google.maps.Polyline({" + Environment.NewLine + "  path:myTrip," + Environment.NewLine + "  strokeColor:\"#000000\"," + Environment.NewLine + "  strokeOpacity:1.0," + Environment.NewLine + "  strokeWeight:3," + Environment.NewLine + "  geodesic: true" + Environment.NewLine + "  });" + Environment.NewLine + "flightPath.setMap(map);" + Environment.NewLine);


            for (int i = 0; i <= waypoints.Count - 1; i++)
            {
                mapHtml.Append("var marker" + i.ToString() + " = new MarkerWithLabel({" + Environment.NewLine + "       position: " + "wpt" + i.ToString() + "," + Environment.NewLine + "icon:'pixel_trans.gif'," + Environment.NewLine + "       draggable: false," + Environment.NewLine + "       raiseOnDrag: true," + Environment.NewLine + "       map: map," + Environment.NewLine + "       labelContent: \"" + wptIdDisplay(waypoints, i) + "\"," + Environment.NewLine + "       labelAnchor: new google.maps.Point(0, 0)," + Environment.NewLine + "       labelClass: \"labels\", // the CSS class for the label" + Environment.NewLine + "       labelStyle: {opacity: 0.75}" + Environment.NewLine + "     });" + Environment.NewLine + "" + Environment.NewLine + "var iw" + i.ToString() + " = new google.maps.InfoWindow({" + Environment.NewLine + "       content: \"Home For Sale\"" + Environment.NewLine + "     });" + Environment.NewLine + "" + Environment.NewLine);

            }
            
            mapHtml.Append(@"}
            
            google.maps.event.addDomListener(window, 'load', initialize);
            </script>
            </head>
            
            <body>
            <div id=""googleMap"" style=""width: " + width.ToString() + "px;height:" + height.ToString() + @"px;""></div>
            </body>
            </html>");

            return mapHtml;

        }

        private static string wptIdDisplay(List<Waypoint> item, int index)
        {
            if (index == 0 || index == item.Count - 1)
            {
                return item[index].ID.Substring(0, 4);
            }
            else
            {
                return item[index].ID;
            }
        }

    }

}
