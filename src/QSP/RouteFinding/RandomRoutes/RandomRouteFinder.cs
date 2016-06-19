using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static QSP.RouteFinding.Constants;
using static System.Math;

namespace QSP.RouteFinding.RandomRoutes
{
    public class RandomRouteFinder
    {
        private static readonly double MaxAngleRadian =
            MaxLegDis / EarthRadiusNm;

        private LatLonSearcher<Waypoint> searcher;

        public RandomRouteFinder(
            IEnumerable<Waypoint> candidates, int gridSize, int polarRegSize)
        {
            searcher = new LatLonSearcher<Waypoint>(gridSize, polarRegSize);

            foreach (var i in candidates)
            {
                searcher.Add(i);
            }
        }

        public List<Waypoint> Find(Waypoint start, Waypoint end)
        {
            var route = new List<Waypoint> { start };
            var current = start;

            while (current.DistanceFrom(end) > MaxLegDis)
            {
                var list = getCandidates(current, end);
                var optimal = chooseCandidates(list, current, end);
                route.Add(optimal);
                current = optimal;
            }

            route.Add(end);
            return route;
        }
        
        private Waypoint chooseCandidates(
            List<Waypoint> candidates, Waypoint start, Waypoint end)
        {
            return candidates
                .Where(w => w.Equals(start) == false)
                .MinBy(getSelector(start, end));
        }

        private Func<Waypoint, double> getSelector(
            Waypoint start, Waypoint end)
        {
            return (w) =>
            {
                var dis = w.DistanceFrom(start);
                var total = dis + w.DistanceFrom(end);
                return total - 0.01 * dis;
            };
        }

        private List<Waypoint> getCandidates(Waypoint start, Waypoint end)
        {
            var startVector = start.LatLon.ToVector3D();
            var endVector = end.LatLon.ToVector3D();
            var tangent = getTangent(startVector, endVector);
            var maxDisVector = (startVector + Tan(MaxAngleRadian) * tangent)
                .Normalize();

            var midPoint = (startVector + maxDisVector) * 0.5;

            var pt = midPoint.ToLatLon();
            var smallRegion = searcher.Find(
                pt.Lat, pt.Lon, MaxLegDis * 0.5);

            if (smallRegion.Count > 0)
            {
                return smallRegion;
            }

            return searcher.Find(start.Lat, start.Lon, MaxLegDis);
        }

        // Returns the unit vector tantgent to the path 
        // from v to w at v
        private static Vector3D getTangent(Vector3D v, Vector3D w)
        {
            return v.Cross(w.Cross(v)).Normalize();
        }
    }
}
