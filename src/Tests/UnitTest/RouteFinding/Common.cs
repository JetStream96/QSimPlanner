using CommonLibrary.LibraryExtension;
using Moq;
using QSP.LibraryExtension;
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
        // Format: 
        // Waypoint1, AirwayToNext, Distance,
        // Waypoint2, AirwayToNext, Distance,
        // ...
        // WaypointN
        //
        // Use a negative distance for automatic calculation
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
            var a = new Mock<IAirport>();
            a.Setup(i => i.Icao).Returns(icao);
            a.Setup(i => i.Rwys).Returns(rwys);
            return a.Object;
        }

        public static IRwyData GetRwyData(string ident, double lat, double lon)
        {
            var r = new Mock<IRwyData>();
            r.Setup(i => i.RwyIdent).Returns(ident);
            r.Setup(i => i.Lat).Returns(lat);
            r.Setup(i => i.Lon).Returns(lon);
            return r.Object;
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
