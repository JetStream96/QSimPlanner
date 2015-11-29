using System;
using System.Collections.Generic;
using System.Linq;
using QSP.AviationTools;
using static QSP.LibraryExtension.Arrays;
using static QSP.MathTools.MathTools;

namespace QSP.RouteFinding
{
    // Some magic number (500) here to be fixed.

    public class RandomRouteFinder
    {
        private static readonly double MAX_ANGLE_DEG = ToDegree(500 / AviationConstants.RADIUS_EARTH_NM);
        private static readonly double MAX_ANGLE_DEG_INT = Math.Floor(MAX_ANGLE_DEG);

        private LatLon latLon1;
        private LatLon latLon2;

        public RandomRouteFinder(LatLon latLonStart, LatLon latLonEnd)
        {
            latLon1 = latLonStart;
            latLon2 = latLonEnd;
        }

        private List<LatLon> getRouteSameLon(double lat1, double lat2, double lon)
        {
            var route = new List<LatLon>();

            double smallLat = 0.0;
            double largeLat = 0.0;

            if (lat1 > lat2)
            {
                smallLat = lat2;
                largeLat = lat1;
            }
            else if (lat1 == lat2)
            {
                route.Add(new LatLon(lat1, lon));
                return route;
            }
            else
            {
                smallLat = lat1;
                largeLat = lat2;
            }

            double lat = smallLat;

            while (lat < largeLat)
            {
                route.Add(new LatLon(lat, lon));
                lat = Math.Floor(lat + MAX_ANGLE_DEG);
            }

            route.Add(new LatLon(lat2, lon));

            return route;

        }

        private double oppositeLon(double lon)
        {
            if (lon >= 0)
            {
                return lon - 180;
            }
            else
            {
                return lon + 180;
            }
        }

        private void trimLatLon(List<LatLon> item)
        {
            foreach (var i in item)
            {
                if (i.Lat > 90)
                {
                    i.Lat = 180 - i.Lat;
                    i.Lon = oppositeLon(i.Lon);
                }
                else if (i.Lat < -90)
                {
                    i.Lat = -180 - i.Lat;
                    i.Lon = oppositeLon(i.Lon);
                }
            }
        }

        public List<LatLon> Find()
        {
            //lat and lon of the random route
            if ((latLon1.Lon - latLon2.Lon) % 360 == 0 && latLon1.Lon % 1 == 0)
            {
                return getRouteSameLon(latLon1.Lat, latLon2.Lat, latLon1.Lon);
            }
            else if ((latLon1.Lon - latLon2.Lon) % 180 == 0 && latLon1.Lon % 1 == 0)
            {
                double goNorthAngle = 180 - latLon1.Lat - latLon2.Lat;
                List<LatLon> result = null;

                if (goNorthAngle <= 180)
                {
                    result = getRouteSameLon(latLon1.Lat, 180 - latLon2.Lat, latLon1.Lon);
                }
                else
                {
                    result = getRouteSameLon(-180 - latLon1.Lat, latLon2.Lat, latLon2.Lon);
                }

                trimLatLon(result);

                return result;
            }

            var route = new List<LatLon>();
            route.Add(latLon1);

            while (route.Last().Distance(latLon2) > 500)
            {
                route.Add(chooseCandidates(getCandidates(route.Last(), latLon2), route.Last()));
            }

            route.Add(latLon2);

            return route;
        }

        private List<LatLon> candidateSameLon(double latNow, double latDest, double lon)
        {
            var candidates = new List<LatLon>();

            if (latNow < latDest)
            {
                candidates.Add(new LatLon(Math.Floor(latNow + MAX_ANGLE_DEG), lon));
            }
            else
            {
                candidates.Add(new LatLon(Math.Ceiling(latNow - MAX_ANGLE_DEG), lon));
            }
            return candidates;
        }

        private List<LatLon> getCandidates(LatLon u, LatLon v2)
        {
            var candidates = new List<LatLon>();

            if ((u.Lon - v2.Lon) % 360 == 0)
            {
                return candidateSameLon(u.Lat, v2.Lat, u.Lon);
            }
            else if ((u.Lon - v2.Lon) % 180 == 0)
            {
                double goNorthAngle = 180 - u.Lat - v2.Lat;

                if (goNorthAngle <= 180)
                {
                    candidates = candidateSameLon(u.Lat, 180 - v2.Lat, u.Lon);
                }
                else
                {
                    candidates = candidateSameLon(-180 - u.Lat, v2.Lat, u.Lon);
                }

                trimLatLon(candidates);
                return candidates;
            }

            candidates = candidatesMeridianIntersection(u, v2);
            candidates.AddRange(candidatesCircleOfLatIntersection(u, v2));

            return candidates;
        }

        private enum FindLonDirection
        {
            East,
            West
        }

        private double getNextLon(double currentLon, FindLonDirection dir)
        {

            if (dir == FindLonDirection.East)
            {
                return trimLon(Math.Floor(currentLon + 1));
            }
            else
            {
                return trimLon(Math.Ceiling(currentLon - 1));
            }

        }

        private static double ModFunction(double n, int x)
        {
            return (n % x + x) % x;
        }

        private static double trimLon(double lon)
        {
            return ModFunction(lon + 180, 360) - 180;
        }

        private List<LatLon> candidatesCircleOfLatIntersection(LatLon u, LatLon v2)
        {
            var candidates = new List<LatLon>();
            FindLatDirection dir = default(FindLatDirection);

            if (getTangent(u, v2).z >= 0)
            {
                dir = FindLatDirection.North;
            }
            else
            {
                dir = FindLatDirection.South;
            }

            var latMax = maxLat(u, v2);
            double latMin = -latMax;

            Tuple<double, double> lon = null;
            double[] lonInt = new double[4];
            double lat = u.Lat;
            bool exitLoop = false;

            while (exitLoop == false)
            {
                exitLoop = true;

                lat = getNextLat(lat, dir);
                lon = getLonCircleOfLatIntersection(u, v2, lat);

                lonInt[0] = trimLon(Math.Floor(lon.Item1));
                lonInt[1] = trimLon(Math.Ceiling(lon.Item1));
                lonInt[2] = trimLon(Math.Floor(lon.Item2));
                lonInt[3] = trimLon(Math.Ceiling(lon.Item2));


                foreach (double i in lonInt)
                {
                    if (u.Distance(lat, i) <= 500)
                    {
                        candidates.Add(new LatLon(lat, i));
                        exitLoop = false;
                    }

                }

            }

            return candidates;

        }

        private List<LatLon> candidatesMeridianIntersection(LatLon u, LatLon v2)
        {
            var candidates = new List<LatLon>();
            FindLonDirection dir = default(FindLonDirection);

            if (Math.Abs(u.Lon - v2.Lon) < 180)
            {
                if (u.Lon > v2.Lon)
                {
                    dir = FindLonDirection.West;
                }
                else
                {
                    dir = FindLonDirection.East;
                }
            }
            else
            {
                if (u.Lon > v2.Lon)
                {
                    dir = FindLonDirection.East;
                }
                else
                {
                    dir = FindLonDirection.West;
                }
            }

            double lon = u.Lon;
            double lat = 0;
            bool exitLoop = false;

            while (exitLoop == false)
            {
                exitLoop = true;

                lon = getNextLon(lon, dir);
                lat = getLatMeridianIntersection(u, v2, lon);

                if (Math.Floor(lat) >= -90 && GreatCircleDistance(u.Lat, u.Lon, Math.Floor(lat), lon) <= 500)
                {
                    candidates.Add(new LatLon(Math.Floor(lat), lon));
                    exitLoop = false;
                }

                if (Math.Ceiling(lat) <= 90 && GreatCircleDistance(u.Lat, u.Lon, Math.Ceiling(lat), lon) <= 500)
                {
                    candidates.Add(new LatLon(Math.Ceiling(lat), lon));
                    exitLoop = false;
                }
            }
            return candidates;
        }

        private double getLatMeridianIntersection(LatLon u, LatLon v2, double lon)
        {
            Vector3D v3 = LatLonToVector3D(u).CrossProductWith(LatLonToVector3D(v2));
            double lonRad = ToRadian(lon);
            double a = Math.Cos(lonRad) * v3.x + Math.Sin(lonRad) * v3.y;
            double c = a / v3.z;

            return ToDegree(Math.Asin(Math.Sqrt(c * c / (1 + c * c))) * (c < 0 ? 1.0 : -1.0));
        }

        private Tuple<double, double> getLonCircleOfLatIntersection(LatLon u, LatLon v2, double lat)
        {

            Vector3D w = LatLonToVector3D(u).CrossProductWith(LatLonToVector3D(v2));
            double latRad = ToRadian(lat);
            double a = Math.Sin(latRad) * w.z;
            double b = Math.Cos(latRad) * w.x;
            double c = Math.Cos(latRad) * w.y;
            double d = Math.Atan2(c, b);
            double g = Math.Acos(-a / Math.Sqrt(b * b + c * c));

            return new Tuple<double, double>(trimLon(ToDegree(d + g)), trimLon(ToDegree(d - g)));

        }

        private LatLon chooseCandidates(List<LatLon> candidates, LatLon startLatLon)
        {

            if (candidates.Count == 1)
            {
                return candidates[0];
            }

            Vector3D w = getTangent(LatLonToVector3D(startLatLon), LatLonToVector3D(latLon2));
            double[] innerProducts = new double[candidates.Count];

            for (int i = 0; i < candidates.Count; i++)
            {
                innerProducts[i] = getTangent(LatLonToVector3D(startLatLon), LatLonToVector3D(candidates[i])).InnerProductWith(w);
            }

            return candidates[innerProducts.MaxIndex()];

        }

        private Vector3D getTangent(Vector3D v, Vector3D w)
        {
            //returns the unit vector tantgent to the path from v to w at v
            return v.CrossProductWith(w.CrossProductWith(v)).Normalize();
        }

        private Vector3D getTangent(LatLon v, LatLon w)
        {
            //returns the unit vector tantgent to the path from v to w at v
            return getTangent(LatLonToVector3D(v), LatLonToVector3D(w));
        }

        public enum FindLatDirection
        {
            North,
            South
        }

        private double maxLat(LatLon u, LatLon v2)
        {
            return ToDegree(Math.Acos(Math.Cos(u.Lat) * Math.Abs(unitVectorUDir(u.Lon).InnerProductWith(getTangent(u, v2)))));
        }

        private Vector3D unitVectorUDir(double lon)
        {
            double lonRad = ToRadian(lon);
            return new Vector3D(-1 * Math.Sin(lonRad), Math.Cos(lonRad), 0);
        }

        private double getNextLat(double lat, FindLatDirection dir)
        {

            if (dir == FindLatDirection.North)
            {
                return Math.Floor(lat + 1);
            }
            else
            {
                return Math.Ceiling(lat - 1);
            }

        }

    }

}
