using System;
using QSP.RouteFinding.Containers;
using static QSP.LibraryExtension.Arrays;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding
{

    // Designed to analyze a route consisting of a series of strings, containing only waypoints and airways.
    // For more sophisticated analyzer, use RouteAnalyzer class.
    //
    // 1. Input: The string, consisting of waypoint (WPT), airway (AWY), and/or direct (DCT) sysbols.
    //           These should be seperated by at least one of the following char/strings:
    //           (1) space
    //           (2) Tab
    //           (3) LF
    //           (4) CR
    //
    // 2. Not case-sensitive to input string. They get converted to upper case before parsing.
    //
    // 3. Format: WPT AWY WPT ... WPT
    //    (1) Any AWY can be replaced by DCT. The route will be a direct between the two waypoints.
    //    (2) Any DCT can be omitted.
    //
    // 4. If the format is wrong, an InvalidIdentifierException will be thrown with an message describing the place where the problem occurs.
    //
    // 5. It's not allowed to direct from one waypoint to another which is more than 500 nm away.
    //    Otherwise an exception will be thrown indicating that the latter waypoint is not a valid waypoint.
    //
    // 6. It's highly recommended to specify preferred lat/lon in the constructor. When there are multiple waypoints matching the 
    //    ident of the first waypoint in route, the one closest to this lat/lon will be used. 

    public class SimpleRouteAnalyzer
    {
        //DCT is ignored altogether, before parsing.
        private static string[] Delimiters = { " ", "\r", "\n", "\t" };
        private string[] routeInput;
        private int lastWpt;
        //if this is null, it indicates that the last element in the input array is a wpt
        private string lastAwy;

        private double prefLat;
        private double prefLon;

        public SimpleRouteAnalyzer(string route) : this(route, 0, 0)
        {
        }

        public SimpleRouteAnalyzer(string route, double prefLat, double prefLon)
        {
            routeInput = route.ToUpper().Split(Delimiters, StringSplitOptions.RemoveEmptyEntries).RemoveElements("DCT");
            this.prefLat = prefLat;
            this.prefLon = prefLon;
        }

        public Route Parse()
        {
            Route result = new Route();

            lastWpt = -1;
            lastAwy = null;

            for (int i = 0; i < routeInput.Length; i++)
            {
                if (lastAwy == null)
                {
                    //this one may be awy or wpt

                    if (tryParseAwy(routeInput[i]) == false && tryParseWpt(routeInput[i], result) == false)
                    {
                        throw new InvalidIdentifierException(string.Format("{0} is not a valid waypoint or airway identifier.", routeInput[i]));
                    }
                }
                else
                {
                    //this one must be wpt
                    if (!tryParseWpt(routeInput[i], result))
                    {
                        throw new InvalidIdentifierException(string.Format("Cannot find waypoint {0} on airway {1}", routeInput[i], lastAwy));
                    }
                }
            }
            return result;
        }

        private bool tryParseWpt(string ident, Route rte)
        {
            if (lastAwy == null)
            {
                var indices = RouteFindingCore.WptList.FindAllByID(ident);

                if (indices == null || indices.Count == 0)
                {
                    return false;
                }
                else if (indices.Count == 1)
                {
                    lastWpt = indices[0];
                }
                else
                {
                    if (lastWpt == -1)
                    {
                        lastWpt = Tracks.Common.Utilities.ChooseSubsequentWpt(prefLat, prefLon, indices);
                    }
                    else
                    {
                        var wpt = RouteFindingCore.WptList[lastWpt];
                        lastWpt = Tracks.Common.Utilities.ChooseSubsequentWpt(wpt.Lat, wpt.Lon, indices);
                    }
                }
                rte.AppendWaypoint(RouteFindingCore.WptList[lastWpt]);
            }
            else
            {
                var intermediateWpt = new AirwayNodeFinder(lastWpt, lastAwy, ident).FindWaypoints();

                if (intermediateWpt == null)
                {
                    return false;
                }

                foreach (var i in intermediateWpt)
                {
                    rte.AppendWaypoint(i, lastAwy);
                }
                lastAwy = null;
            }
            return true;
        }

        private bool tryParseAwy(string airway)
        {
            if (lastWpt == -1)
            {
                return false;
            }

            var wptList = RouteFindingCore.WptList;

            foreach (var i in wptList.EdgesFrom(lastWpt))
            {
                if (wptList.GetEdge(i).value.Airway == airway)
                {
                    lastAwy = airway;
                    return true;
                }
            }
            return false;
        }
    }

}
