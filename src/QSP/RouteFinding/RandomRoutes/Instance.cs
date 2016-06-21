using System;
using QSP.RouteFinding.Containers;
using System.Collections.Generic;
using static QSP.AviationTools.Coordinates.Format5Letter;

namespace QSP.RouteFinding.RandomRoutes
{
    public static class Instance
    {
        public static RandomRouteFinder FinderInstance =
            new RandomRouteFinder(GetWaypoints(), 5, 5);

        private static List<Waypoint> GetWaypoints()
        {
            // Using north Atlantic policy.

            var coordinates = new List<Waypoint>();

            for (int lat = -90; lat <= 90; lat++)
            {
                int increment = 0;
                int latAbs = Math.Abs(lat);

                if (latAbs == 90)
                {
                    increment = 360;
                }
                else if (latAbs >= 70)
                {
                    increment = 20;
                }
                else if (latAbs >= 30)
                {
                    increment = 10;
                }
                else
                {
                    increment = 5;
                }

                for (int lon = -180; lon < 180; lon += increment)
                {
                    coordinates.Add(CreateWptHelper(lat, lon));
                }
            }
            
            return coordinates;
        }

        private static Waypoint CreateWptHelper(int lat, int lon)
        {
            return new Waypoint(To5LetterFormat(lat, lon), lat, lon);
        }
    }
}
