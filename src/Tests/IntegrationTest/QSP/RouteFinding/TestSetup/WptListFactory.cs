using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;

namespace IntegrationTest.QSP.RouteFinding.TestSetup
{
    public static class WptListFactory
    {
        // Contains all integral lat/lon waypoints
        public static WaypointList GetWptList(IEnumerable<string> Idents)
        {
            var wptList = new WaypointList();
            AddLatLons(wptList);
            AddWpts(wptList, Idents);
            return wptList;
        }

        private static void AddWpts(WaypointList wptList, IEnumerable<string> Idents)
        {
            var rd = new Random(123);

            foreach (var i in Idents)
            {
                wptList.AddWaypoint(
                    new Waypoint(i, rd.Next(-90, 91), rd.Next(-180, 181)));
            }
        }

        private static void AddLatLons(WaypointList wptList)
        {
            for (int lon = -180; lon <= 180; lon++)
            {
                for (int lat = -90; lat <= 90; lat++)
                {
                    wptList.AddWaypoint(
                        new Waypoint(Format5Letter.ToString(lat, lon), lat, lon));
                }
            }
        }
    }
}
