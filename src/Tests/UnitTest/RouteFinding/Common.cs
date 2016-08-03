using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
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
            route.AddLastWaypoint((Waypoint)para.Last());

            for (int i = para.Length - 2; i >= 0; i -= 3)
            {
                var dis = (double)para[i];

                if (dis < 0.0)
                {
                    route.AddFirstWaypoint(
                        (Waypoint)para[i - 2],
                        (string)para[i - 1]);
                }
                else
                {
                    route.AddFirstWaypoint(
                        (Waypoint)para[i - 2],
                        (string)para[i - 1],
                        dis);
                }
            }

            return route;
        }

        public static AirportManager GetAirportManager(params Airport[] items)
        {
            var col = new AirportCollection();
            items.ForEach(i => col.Add(i));
            return new AirportManager(col);
        }

        public static Airport GetAirport(string icao, params RwyData[] rwys)
        {
            return new Airport(
            icao, "", 0.0, 0.0, 0, true, 0, 0, 0, rwys.ToList());
        }

        public static RwyData GetRwyData(string ident, double lat, double lon)
        {
            return new RwyData(
            ident, "", 0, 0, true, true, "", "", lat, lon, 0, 0.0, 0, "", 0);
        }

        public static void AddNeighbor(WaypointList wptList, int index1, 
            string airway, AirwayType type, int index2)
        {
            wptList.AddNeighbor(index1, index2,
               new Neighbor(airway, type, wptList.Distance(index1, index2)));
        }
    }
}
