using QSP.RouteFinding.Containers;
using System.Collections.Generic;
using static QSP.AviationTools.Coordinates.Format5Letter;

namespace QSP.RouteFinding.RandomRoutes
{
    public static class RandomRouteFinderFactory
    {
        public static RandomRouteFinder GetInstance()
        {
            return new RandomRouteFinder(GetWaypoints(), 5, 5);
        }

        private static List<Waypoint> GetWaypoints()
        {
            // Using north Atlantic policy.

            var coordinates = new List<Waypoint>();

            for (int lat = -70; lat <= 70; lat++)
            {
                for (int lon = -180; lon < 180; lon += 5)
                {
                    coordinates.Add(createWptHelper(lat, lon));
                }
            }

            for (int lon = -180; lon < 180; lon += 20)
            {
                for (int lat = 71; lat <= 89; lat++)
                {
                    coordinates.Add(createWptHelper(lat, lon));
                }

                for (int lat = -89; lat <= -71; lat++)
                {
                    coordinates.Add(createWptHelper(lat, lon));
                }
            }

            coordinates.Add(createWptHelper(-90, 0));
            coordinates.Add(createWptHelper(90, 0));

            return coordinates;
        }

        private static Waypoint createWptHelper(int lat, int lon)
        {
            return new Waypoint(To5LetterFormat(lat, lon), lat, lon);
        }
    }
}
