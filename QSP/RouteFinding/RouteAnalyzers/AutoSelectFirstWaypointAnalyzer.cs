using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.AirwayStructure;
using static QSP.MathTools.Utilities;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Utilizes the SimpleRouteAnalyzer to analyze a route represented as a string.
    // 
    // Because we only know the ident of the first waypoint and there is likely to be multiple waypoints with the 
    // same ident, route parse can fail due to picking an incorrect first waypoint.
    // Therefore this class allows you to specify the approximate lat/lon of first waypoint, and it will choose 
    // base on this lat/lon. When there are multiple waypoints matching the ident of the first waypoint in route, 
    // the one closest to this lat/lon will be chosen first.
    //
    // It will try all waypoints with matching ident until the route is successfully parsed. 

    public class AutoSelectFirstWaypointAnalyzer
    {
        private static char[] Delimiters = { ' ', '\r', '\n', '\t' };

        private string route;
        private double preferredLat;
        private double preferredLon;
        private WaypointList wptList;

        public AutoSelectFirstWaypointAnalyzer(string route, double preferredLat, double preferredLon) :
               this(route, preferredLat, preferredLon, RouteFindingCore.WptList)
        { }

        public AutoSelectFirstWaypointAnalyzer(string route, double preferredLat, double preferredLon, WaypointList wptList)
        {
            this.route = route;
            this.preferredLat = preferredLat;
            this.preferredLon = preferredLon;
            this.wptList = wptList;
        }

        /// <exception cref="InvalidRouteException"></exception>
        /// <exception cref="WaypointNotFoundException"></exception>
        public Route Parse()
        {
            var firstWptCandidates = wptList.FindAllByID(firstWptID());

            if (firstWptCandidates.Count == 0)
            {
                throw new WaypointNotFoundException();
            }

            if (firstWptCandidates.Count > 1)
            {
                firstWptCandidates.Sort(CompareDistance());
            }

            foreach (var i in firstWptCandidates)
            {
                try
                {
                    return new SimpleRouteAnalyzer(route, i, wptList).Parse();
                }
                catch { }
            }
            throw new InvalidRouteException();
        }

        public Comparer<int> CompareDistance()
        {
            return new CompareDistanceHelper(wptList, preferredLat, preferredLon);
        }

        private class CompareDistanceHelper : Comparer<int>
        {
            private WaypointList wptList;
            private double preferredLat;
            private double preferredLon;

            public CompareDistanceHelper(WaypointList wptList, double preferredLat, double preferredLon)
            {
                this.wptList = wptList;
                this.preferredLat = preferredLat;
                this.preferredLon = preferredLon;
            }

            public override int Compare(int x, int y)
            {
                return GreatCircleDistance(preferredLat, preferredLon, wptList[x].Lat, wptList[x].Lon)
                       .CompareTo(
                       GreatCircleDistance(preferredLat, preferredLon, wptList[y].Lat, wptList[y].Lon));
            }
        }

        private string firstWptID()
        {
            int index = -1;

            for (int i = 0; i < route.Length; i++)
            {
                if (Delimiters.Contains(route[i]) == false)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0 || index == route.Length - 1)
            {
                throw new ArgumentException("Invalid route string.");
            }

            int endIndex = route.IndexOfAny(Delimiters, index + 1);

            if (endIndex < 0)
            {
                throw new ArgumentException("Invalid route string.");
            }

            return route.Substring(index, endIndex - index);
        }

    }
}
