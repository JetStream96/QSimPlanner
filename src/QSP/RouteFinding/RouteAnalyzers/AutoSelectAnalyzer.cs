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
        private double preferredLat;
        private double preferredLon;
        private WaypointList wptList;

        public AutoSelectAnalyzer(
            string[] route,
            double preferredLat,
            double preferredLon,
            WaypointList wptList)
        {
            this.route = route;
            this.preferredLat = preferredLat;
            this.preferredLon = preferredLon;
            this.wptList = wptList;
        }

        /// <exception cref="InvalidRouteException"></exception>
        /// <exception cref="WaypointNotFoundException"></exception>
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
                firstWptCandidates.Sort(CompareDistance());
            }

            foreach (var i in firstWptCandidates)
            {
                try
                {
                    return new BasicRouteAnalyzer(route, wptList, i).Analyze();
                }
                catch { }
            }

            // TODO: Error message for user?
            throw new InvalidRouteException();
        }

        public Comparer<int> CompareDistance()
        {
            return Comparer<int>.Create((x, y) =>
            {
                var disX = wptList[x].Distance(preferredLat, preferredLon);
                var disY = wptList[y].Distance(preferredLat, preferredLon);

                return disX.CompareTo(disY);
            });
        }
    }
}
