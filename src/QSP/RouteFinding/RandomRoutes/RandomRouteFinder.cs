using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data;
using QSP.AviationTools.Coordinates;
using static QSP.MathTools.Vectors.Vector3DExtension;
using static QSP.AviationTools.Constants;

namespace QSP.RouteFinding.RandomRoutes
{
    public class RandomRouteFinder
    {
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



            return route;
        }

        public List<Waypoint> getCandidates(Waypoint start, Waypoint end)
        {
            var midPoint =
                (start.LatLon.ToVector3D() + end.LatLon.ToVector3D()) *0.5;

            var pt = midPoint.ToLatLon();
            var smallRegion = searcher.Find(
                pt.Lat, pt.Lon, Constants.MaxLegDis * 0.5);

            if (smallRegion.Count > 0)
            {
                return smallRegion;
            }

            return searcher.Find(start.Lat, start.Lon, Constants.MaxLegDis);
        }
    }
}
