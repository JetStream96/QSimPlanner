using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Routes
{
    public static class RouteText
    {/*
        public static string GetString(
            this IReadOnlyRoute Route,
            bool ShowDct,
            bool OnlyShowTrackId)
        {
            throw new NotImplementedException();
            /*
            if (Route.Count < 2)
            {
                throw new InvalidOperationException(
                    "Number of waypoints in the route is less than 2.");
            }

            var dct = ShowDct ? "DCT" : "";
            var result = new StringBuilder();
            var first = Route.First;
            var last = Route.Last;
            
            if (ShowFirstLastInnerList)
            {
                result.Append(
                    string.Join(dct, first.Neighbor.InnerWaypoints) + ' ');
            }

            result.Append(first.Neighbor.Airway + ' ');

            string lastAirway = first.Neighbor.Airway;

            foreach (var i in Route.Skip(1))
            {
                if (i == last) break;

                var airway = i.Neighbor.Airway;
                var id = i.Waypoint.ID;

                if (airway == "DCT")
                {
                    result.Append(id + ' ');
                    if (ShowDct) result.Append("DCT" + ' ');

                }
                else if (airway != lastAirway)
                {
                    result.Append(id + ' ');
                    result.Append(airway + ' ');
                    lastAirway = airway;
                }
            }

            if (node.Value.AirwayToNext != "DCT" || ShowDct)
            {
                result.Append(node.Value.AirwayToNext + ' ');
            }
            
            return result.ToString();
        }*/
    }
}
