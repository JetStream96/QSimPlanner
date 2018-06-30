using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.GoogleMap
{
    public static class RouteDrawing
    {
        public static string GetPostData(IReadOnlyList<Waypoint> route)
        {
            var latLonId = string.Join(";", route.Select(w => w.Lat + "," + w.Lon + "," + w.ID));

            // Center of the map
            var center = GetCenter(route);
            var centerStr = center.Lat + "," + center.Lon;
            return $"LatLonId={latLonId}&Center={centerStr}";

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
