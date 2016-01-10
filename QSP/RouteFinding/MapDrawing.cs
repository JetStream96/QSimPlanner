using System;
using System.Collections.Generic;
using System.Text;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding
{
    public static class MapDrawing
    {
        public static StringBuilder MapDrawString(ManagedRoute rte, int width, int height)
        {
            rte.Expand();
            var mapHtml = new StringBuilder();

            mapHtml.Append(
@"<!DOCTYPE html>
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

            int counter = 0;
            foreach (var i in rte)
            {
                mapHtml.AppendLine("var wpt" + (counter++).ToString() + "=new google.maps.LatLng(" +
                                   i.Waypoint.Lat + "," + i.Waypoint.Lon + ");");
            }

            //now the center of map
            double centerLat = (rte.First.Waypoint.Lat + rte.Last.Waypoint.Lat) / 2;
            double centerLon = (rte.First.Waypoint.Lon + rte.Last.Waypoint.Lon) / 2;

            mapHtml.AppendLine(string.Format("var centerP=new google.maps.LatLng({0},{1});", centerLat, centerLon));

            int zoomlevel = 6;

            mapHtml.Append(
@"var mapProp = {
center:centerP,
zoom:" + zoomlevel.ToString() + @",
mapTypeId:google.maps.MapTypeId.ROADMAP
};
var map=new google.maps.Map(document.getElementById(""googleMap""),mapProp);
var myTrip=[");

            counter = 0;
            foreach (var i in rte)
            {
                if (counter != rte.Count - 1)
                {
                    mapHtml.Append("wpt" + (counter++).ToString() + ",");
                }
                else
                {
                    mapHtml.AppendLine("wpt" + (counter++).ToString() + @"];
var flightPath=new google.maps.Polyline({
path:myTrip,
strokeColor:""#000000"",
strokeOpacity:1.0,
strokeWeight:3,
geodesic: true
});
flightPath.setMap(map);");
                }
            }

            counter = 0;
            foreach (var i in rte)
            {
                mapHtml.Append(string.Format(
@"var marker{0}  = new MarkerWithLabel({{
position: wpt{1},
icon:'pixel_trans.gif',
draggable: false,
raiseOnDrag: true,
map: map,
labelContent: ""{2}"",
labelAnchor: new google.maps.Point(0, 0),
labelClass: ""labels"", // the CSS class for the label
labelStyle: {{opacity: 0.75}}
}});

var iw{3} = new google.maps.InfoWindow({{
content: ""Home For Sale""
}});

", counter, counter, wptIdDisplay(rte, i.Waypoint, counter), counter++));

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

        private static string wptIdDisplay(ManagedRoute rte, Waypoint waypoint, int index)
        {
            if (index == 0 || index == rte.Count - 1)
            {
                return waypoint.ID.Substring(0, 4);
            }
            else
            {
                return waypoint.ID;
            }
        }
    }
}
