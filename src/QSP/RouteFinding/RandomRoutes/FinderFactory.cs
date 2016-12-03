using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using static QSP.AviationTools.Coordinates.Format5Letter;

namespace QSP.RouteFinding.RandomRoutes
{
    public static class FinderFactory
    {
        private static RandomRouteFinder finderInstance;

        public static RandomRouteFinder GetInstance()
        {
            if (finderInstance == null)
            {
                finderInstance = new RandomRouteFinder(GetWaypoints(), 5, 5);
            }

            return finderInstance;
        }

        private static List<Waypoint> GetWaypoints()
        {
            // Using north Atlantic policy.

            var coordinates = new List<Waypoint>();

            for (int lat = -90; lat <= 90; lat++)
            {
                int increment = GetIncrement(lat);

                for (int lon = -180; lon < 180; lon += increment)
                {
                    coordinates.Add(CreateWptHelper(lat, lon));
                }
            }

            return coordinates;
        }

        private static int GetIncrement(double lat)
        {
            double latAbs = Math.Abs(lat);
            if (latAbs > 90.0) throw new ArgumentException();
            if (latAbs == 90.0) return 360;
            if (latAbs >= 70.0) return 20;
            if (latAbs >= 30.0) return 10;
            return 5;
        }

        private static Waypoint CreateWptHelper(int lat, int lon)
        {
            return new Waypoint(To5LetterFormat(lat, lon), lat, lon);
        }
    }
}
