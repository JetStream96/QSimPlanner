using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using System;
using QSP.RouteFinding.Routes.Toggler;
using static QSP.LibraryExtension.Arrays;

namespace QSP.RouteFinding.RouteAnalyzers
{

    ////// Designed to analyze a route consisting of a series of strings, containing only waypoints, lat/lon entries and airways.
    ////// For a more sophisticated analyzer, use StandardRouteAnalyzer class.
    //////
    ////// 1. Input: The string, consisting of waypoint (WPT), airway (AWY), and/or direct (DCT) sysbols.
    //////           These should be seperated by at least one of the following char/strings:
    //////           (1) space
    //////           (2) Tab
    //////           (3) LF
    //////           (4) CR
    //////
    ////// 2. Not case-sensitive to input string. They get converted to upper case before parsing.
    //////
    ////// 3. Format: WPT AWY WPT ... WPT
    //////    (1) Any AWY can be replaced by DCT. The route will be a direct between the two waypoints.
    //////    (2) Any DCT can be omitted.
    //////
    ////// 4. If the format is wrong, an InvalidIdentifierException will be thrown with an message describing the place where the problem occurs.
    //////
    ////// 5. It's not allowed to direct from one waypoint to another which is more than 500 nm away.
    //////    Otherwise an exception will be thrown indicating that the latter waypoint is not a valid waypoint.
    //////
    ////// 6. It's neccesay to specify the index of first waypoint in WptList. If the ident of specified waypoint is different from
    //////    the first word in route input string, an exception will be thrown.
    //////


    // Designed to analyze a route consisting of a series of strings, containing only waypoints, lat/lon entries and airways.
    // For a more sophisticated analyzer, use StandardRouteAnalyzer class.
    //
    // 1. Input: An array of strings, consisting of waypoint (WPT), lat/lon (COORD), and airway (AWY) sysbols.
    //
    // 2. All characters should be capital. 
    //
    // 3. Format: WPT AWY WPT ... WPT
    //    (1) If an airway is DCT, it should be omitted.
    //    (2) A lat/lon must be in decimal representation (e.g. N32.665W122.1265).
    //
    // 4. If the format is wrong, an InvalidIdentifierException will be thrown with an message describing the place where the problem occurs.
    //
    // 5. It's not allowed to direct from one waypoint to another which is more than 500 nm away.
    //    Otherwise an exception will be thrown indicating that the latter waypoint is not a valid waypoint.
    //
    // 6. It's necessary to specify the index of first waypoint in WptList. If the ident of specified waypoint is different from
    //    the first word in route input string, an exception will be thrown.
    //    If the first entry is lat/lon (not in wptList), specifiy a negative index.
    //

    public class BasicRouteAnalyzer
    {
        private static char[] Delimiters = { ' ', '\r', '\n', '\t' };
        private WaypointList wptList;

        private string[] routeInput;
        private int lastWpt;
        private string lastAwy; // If this is null, it indicates that the last element in the input array is a wpt.
        private Route rte;  // Returning value

        public BasicRouteAnalyzer(string[] route, WaypointList wptList, int firstWaypointIndex)
        {

        }

        public BasicRouteAnalyzer(string route, int firstWaypointIndex, WaypointList wptList)
        {
            this.wptList = wptList;
            rte = new Route();
            routeInput = route.ToUpper().Split(Delimiters, StringSplitOptions.RemoveEmptyEntries).RemoveElements("DCT");
            validateFirstWpt(firstWaypointIndex);
        }

        private void validateFirstWpt(int index)
        {
            var wpt = wptList[index];
            if (routeInput[0] != wpt.ID)
            {
                throw new ArgumentException("The first waypoint of the route does not match the specified index in WptList.");
            }
            lastWpt = index;
            rte.AppendWaypoint(wpt);
        }

        public Route Analyze()
        {
            lastAwy = null;

            for (int i = 1; i < routeInput.Length; i++)
            {
                if (lastAwy == null)
                {
                    //this one may be awy or wpt

                    if (tryParseAwy(routeInput[i]) == false && tryParseWpt(routeInput[i]) == false)
                    {
                        throw new InvalidIdentifierException(string.Format("{0} is not a valid waypoint or airway identifier.", routeInput[i]));
                    }
                }
                else
                {
                    //this one must be wpt
                    if (tryParseWpt(routeInput[i]) == false)
                    {
                        throw new InvalidIdentifierException(string.Format("Cannot find waypoint {0} on airway {1}", routeInput[i], lastAwy));
                    }
                }
            }
            return rte;
        }

        private bool tryParseWpt(string ident)
        {
            if (lastAwy == null)
            {
                var indices = wptList.FindAllByID(ident);

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
                    var wpt = wptList[lastWpt];
                    lastWpt = Tracks.Common.Utilities.ChooseSubsequentWpt(wpt.Lat, wpt.Lon, indices);
                }
                rte.AppendWaypoint(wptList[lastWpt], true);
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
                    rte.AppendWaypoint(i, lastAwy, true);
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
