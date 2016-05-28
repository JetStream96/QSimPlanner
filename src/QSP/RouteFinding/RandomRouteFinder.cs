using QSP.AviationTools.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Modulo;
using static QSP.MathTools.GCDis;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static QSP.RouteFinding.Constants;
using static System.Math;

namespace QSP.RouteFinding
{
    public class RandomRouteFinder
    {
        private static readonly double MaxAngleDeg =
            ToDegree(MaxLegDis / AviationTools.Constants.EarthRadiusNm);
        private static readonly double MaxAngleDegInt = Floor(MaxAngleDeg);

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
                lat = Floor(lat + MaxAngleDeg);
            }

            route.Add(new LatLon(lat2, lon));

            return route;
        }

        private double oppositeLon(double lon)
        {
            return lon >= 0 ? (lon - 180) : (lon + 180);
        }

        private void trimLatLon(List<LatLon> item)
        {
            for (int i = 0; i < item.Count; i++)
            {
                var x = item[i];

                if (x.Lat > 90)
                {
                    x = new LatLon(180 - x.Lat, oppositeLon(x.Lon));
                }
                else if (item[i].Lat < -90)
                {
                    x = new LatLon(-180 - x.Lat, oppositeLon(x.Lon));
                }
            }
        }

        /// <summary>
        /// Returns a List of LatLon, which includes each point of the random route.
        /// First and last points are also included.
        /// </summary>
        /// <returns></returns>
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

            while (route.Last().Distance(latLon2) > 500.0)
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
                candidates.Add(new LatLon(Floor(latNow + MaxAngleDeg), lon));
            }
            else
            {
                candidates.Add(new LatLon(Ceiling(latNow - MaxAngleDeg), lon));
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
                return trimLon(Floor(currentLon + 1));
            }
            else
            {
                return trimLon(Ceiling(currentLon - 1));
            }
        }

        private static double trimLon(double lon)
        {
            return (lon + 180).Mod(360) - 180;
        }

        private List<LatLon> candidatesCircleOfLatIntersection(LatLon u, LatLon v2)
        {
            var candidates = new List<LatLon>();
            FindLatDirection dir = default(FindLatDirection);

            if (getTangent(u, v2).Z >= 0)
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

                lonInt[0] = trimLon(Floor(lon.Item1));
                lonInt[1] = trimLon(Ceiling(lon.Item1));
                lonInt[2] = trimLon(Floor(lon.Item2));
                lonInt[3] = trimLon(Ceiling(lon.Item2));

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
            var dir = default(FindLonDirection);

            if (Abs(u.Lon - v2.Lon) < 180)
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

                if (Floor(lat) >= -90.0 && Distance(u.Lat, u.Lon, Floor(lat), lon) <= 500.0)
                {
                    candidates.Add(new LatLon(Floor(lat), lon));
                    exitLoop = false;
                }

                if (Ceiling(lat) <= 90.0 && Distance(u.Lat, u.Lon, Ceiling(lat), lon) <= 500.0)
                {
                    candidates.Add(new LatLon(Ceiling(lat), lon));
                    exitLoop = false;
                }
            }
            return candidates;
        }

        private double getLatMeridianIntersection(LatLon u, LatLon v2, double lon)
        {
            var v3 = u.ToVector3D().Cross(v2.ToVector3D());
            double lonRad = ToRadian(lon);
            double a = Cos(lonRad) * v3.X + Sin(lonRad) * v3.Y;
            double c = a / v3.Z;

            return ToDegree(Asin(Sqrt(c * c / (1 + c * c))) * (c < 0 ? 1.0 : -1.0));
        }

        private Tuple<double, double> getLonCircleOfLatIntersection(LatLon u, LatLon v2, double lat)
        {
            var w = u.ToVector3D().Cross(v2.ToVector3D());
            double latRad = ToRadian(lat);
            double cosLat = Cos(latRad);
            double a = Sin(latRad) * w.Z;
            double b = cosLat * w.X;
            double c = cosLat * w.Y;
            double d = Atan2(c, b);
            double g = Acos(-a / Sqrt(b * b + c * c));

            return new Tuple<double, double>(trimLon(ToDegree(d + g)), trimLon(ToDegree(d - g)));
        }

        private LatLon chooseCandidates(List<LatLon> candidates, LatLon startLatLon)
        {
            if (candidates.Count == 1)
            {
                return candidates[0];
            }

            var w = getTangent(startLatLon, latLon2);
            double[] innerProducts = new double[candidates.Count];

            for (int i = 0; i < candidates.Count; i++)
            {
                innerProducts[i] = getTangent(startLatLon, candidates[i]).Dot(w);
            }

            return candidates[innerProducts.MaxIndex()];
        }

        //returns the unit vector tantgent to the path from v to w at v
        //
        private Vector3D getTangent(Vector3D v, Vector3D w)
        {
            return v.Cross(w.Cross(v)).Normalize();
        }

        private Vector3D getTangent(LatLon v, LatLon w)
        {
            return getTangent(v.ToVector3D(), w.ToVector3D());
        }

        private enum FindLatDirection
        {
            North,
            South
        }

        private double maxLat(LatLon u, LatLon v2)
        {
            return ToDegree(Acos(Cos(u.Lat) * Abs(unitVectorUDir(u.Lon).Dot(getTangent(u, v2)))));
        }

        private Vector3D unitVectorUDir(double lon)
        {
            double lonRad = ToRadian(lon);
            return new Vector3D(-Sin(lonRad), Cos(lonRad), 0);
        }

        private double getNextLat(double lat, FindLatDirection dir)
        {
            if (dir == FindLatDirection.North)
            {
                return Floor(lat + 1);
            }
            else
            {
                return Ceiling(lat - 1);
            }
        }
    }
}
