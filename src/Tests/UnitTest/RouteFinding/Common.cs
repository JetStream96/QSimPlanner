using QSP.LibraryExtension;
using FakeItEasy;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Linq;

namespace UnitTest.RouteFinding
{
    public static class Common
    {
        /// <summary>
        /// Format: 
        /// Waypoint1, AirwayToNext, Distance,
        /// Waypoint2, AirwayToNext, Distance,
        /// ...
        /// WaypointN
        ///
        /// Use a negative distance for automatic calculation
        /// </summary>
        public static Route GetRoute(params object[] para)
        {
            if (para.Length % 3 != 1)
            {
                throw new ArgumentException();
            }

            var route = new Route();
            var nodes = route.Nodes;
            nodes.AddLast(new RouteNode((Waypoint)para.Last(), null));

            for (int i = para.Length - 2; i >= 0; i -= 3)
            {
                var dis = (double)para[i];
                var wpt = (Waypoint)para[i - 2];

                if (dis < 0.0)
                {
                    var prev = nodes.First.Value.Waypoint;
                    dis = prev.Distance(wpt);
                }

                var n = new Neighbor((string)para[i - 1], dis);
                nodes.AddFirst(new RouteNode(wpt, n));
            }

            return route;
        }

        public static AirportManager GetAirportManager(params IAirport[] items)
        {
            return new AirportManager(items);
        }

        public static IAirport GetAirport(string icao, params IRwyData[] rwys)
        {
            var a = A.Fake<IAirport>();
            A.CallTo(() => a.Icao).Returns(icao);
            A.CallTo(() => a.Rwys).Returns(rwys);
            return a;
        }

        public static IRwyData GetRwyData(string ident, double lat, double lon)
        {
            var r = A.Fake<IRwyData>();
            A.CallTo(() => r.RwyIdent).Returns(ident);
            A.CallTo(() => r.Lat).Returns(lat);
            A.CallTo(() => r.Lon).Returns(lon);
            return r;
        }

        public static void AddNeighbor(this WaypointList wptList, int index1,
            string airway, int index2)
        {
            wptList.AddNeighbor(index1, index2,
               new Neighbor(airway, wptList.Distance(index1, index2)));
        }

        public static WaypointList GetWptList(params Waypoint[] waypoints)
        {
            var wptList = new WaypointList();
            waypoints.ForEach(w => wptList.AddWaypoint(w));
            return wptList;
        }
    }
}
