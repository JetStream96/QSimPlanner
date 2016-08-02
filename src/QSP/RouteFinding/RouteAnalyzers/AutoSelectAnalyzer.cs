using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Automatically select the first waypoint when there are many with 
    // the same ident. Utilizes the SimpleRouteAnalyzer to analyze a route 
    // represented as a string.
    // 
    // Because we only know the ident of the first waypoint and there is 
    // likely to be multiple waypoints with the same ident, route parse 
    // can fail due to picking an incorrect first waypoint.
    //
    // Therefore this class allows you to specify the approximate lat/lon 
    // of first waypoint, and it will choose base on this lat/lon. 
    // When there are multiple waypoints matching the ident of the first 
    // waypoint in route, the one closest to this lat/lon will be chosen first.
    //
    // It will try all waypoints with matching ident until the route is 
    // successfully parsed. 

    public class AutoSelectAnalyzer
    {
        private string[] route;
        private ICoordinate orig;
        private ICoordinate dest;
        private WaypointList wptList;

        public AutoSelectAnalyzer(
            string[] route,
            ICoordinate orig,
            ICoordinate dest,
            WaypointList wptList)
        {
            this.route = route;
            this.orig = orig;
            this.dest = dest;
            this.wptList = wptList;
        }

        // Can throws exception.
        public Route Analyze()
        {
            if (route.Length == 0)
            {
                return new Route();
            }

            var firstWptCandidates = wptList.FindAllById(route[0]);

            if (firstWptCandidates.Count == 0)
            {
                return new BasicRouteAnalyzer(route, wptList, -1).Analyze();
            }

            if (firstWptCandidates.Count > 1)
            {
                firstWptCandidates.Sort(DistanceSum());
            }

            Exception firstException = null;

            foreach (var i in firstWptCandidates)
            {
                try
                {
                    return new BasicRouteAnalyzer(route, wptList, i).Analyze();
                }
                catch (Exception ex)
                {
                    if (firstException == null) firstException = ex;
                }
            }
            
            throw firstException;
        }

        public Comparer<int> DistanceSum()
        {
            return Comparer<int>.Create((x, y) =>
            {
                var ptX = wptList[x];
                var ptY = wptList[y];

                var disX = ptX.Distance(orig) + ptX.Distance(dest); 
                var disY = ptY.Distance(orig) + ptY.Distance(dest);

                return disX.CompareTo(disY);
            });
        }
    }
}
