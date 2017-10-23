using CommonLibrary.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace QSP.GoogleMap
{
    public static class RouteDrawing
    {
        public static StringBuilder MapDrawString(
            IReadOnlyList<Waypoint> route, int width=-1, int height=-1)
        {
            var mapHtml = new StringBuilder();

            string path = Assembly.GetExecutingAssembly().Location;
            var scriptPath = Path.GetDirectoryName(path) +
                @"\GoogleMap\Library\markerwithlabel.js";

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
    <script type = ""text/javascript"" src=""http://maps.googleapis.com/maps/api/js?v=3&amp;sensor=False""></script>
    <script type = ""text/javascript"" src=""" + scriptPath + @"""></script>
    <script type = ""text/javascript"">

function initialize()
{
");

            route.ForEach((wpt, index) =>
            {
                mapHtml.AppendLine(
                    $"var wpt{index}=new google.maps.LatLng(" +
                    $"{wpt.Lat},{wpt.Lon});");
            });

            // Center of map
            var center = GetCenter(route);
            mapHtml.AppendLine($"var centerP=new google.maps.LatLng({center.Lat},{center.Lon});");

            const int zoomlevel = 6;

            mapHtml.Append(
@"var mapProp = {
center:centerP,
zoom:" + zoomlevel.ToString() + @",
mapTypeId:google.maps.MapTypeId.ROADMAP
};
var map=new google.maps.Map(document.getElementById(""googleMap""),mapProp);
var myTrip=[");

            route.ForEach((wpt, index) =>
            {
                if (index != route.Count - 1)
                {
                    mapHtml.Append($"wpt{index},");
                }
                else
                {
                    mapHtml.AppendLine("wpt" + index + @"];
    var flightPath=new google.maps.Polyline({
    path:myTrip,
    strokeColor:""#000000"",
    strokeOpacity:1.0,
    strokeWeight:3,
    geodesic: true
    });
    flightPath.setMap(map);");
                }
            });

            route.ForEach((wpt, index) =>
            {
                mapHtml.Append(string.Format(
                    @"
    var marker{0}  = new MarkerWithLabel({{
    position: wpt{0},
    icon:'pixel_trans.gif',
    draggable: false,
    raiseOnDrag: true,
    map: map,
    labelContent: ""{1}"",
    labelAnchor: new google.maps.Point(0, 0),
    labelClass: ""labels"", // the CSS class for the label
    labelStyle: {{opacity: 0.75}}
    }});

    var iw{0} = new google.maps.InfoWindow({{
    content: ""Home For Sale""
    }});

", index, wpt.ID));
            });

            mapHtml.Append(@"}
            
            google.maps.event.addDomListener(window, 'load', initialize);
            </script>
            </head>
            
            <body>
            <div id=""googleMap"" style=""" + GetDivStyle(width, height) + @"""></div>
            </body>
            </html>");

            return mapHtml;
        }

        private static string GetDivStyle(int width = -1, int height = -1)
        {
            return width < 0 ?
                "position: absolute; top: 0; right: 0; bottom: 0; left: 0;" :
                $"width:{width}px;height:{height}px;";
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
