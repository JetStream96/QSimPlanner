using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using System;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;
using static QSP.MathTools.Utilities;

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
    //    Otherwise a WaypointTooFarException is thrown.
    //
    // 6. It's necessary to specify the index of first waypoint in WptList. 
    //    If the first entry is lat/lon (not in wptList), specifiy a negative index.
    //    If the ident of specified waypoint is different from the first word in route input string, an ArgumentException will be thrown.
    //    

    public class BasicRouteAnalyzer
    {
        private WaypointList wptList;

        private string[] routeInput;
        private int lastWpt;         // Index in WptList. -1 if the last wpt is lat/lon.
        private string lastAwy;      // If this is null, it indicates that the last element in the input array is a wpt.
        private Route rte;           // Returning value

        /// <param name="firstWaypointIndex">Use a negative value if the first waypoint is a lat/lon.</param>
        /// <exception cref="ArgumentException"></exception>
        public BasicRouteAnalyzer(string[] routeInput, WaypointList wptList, int firstWaypointIndex)
        {
            this.wptList = wptList;
            rte = new Route();
            this.routeInput = routeInput;
            validateFirstWpt(firstWaypointIndex);
        }

        private void validateFirstWpt(int index)
        {
            if (index == -1)
            {
                if (tryParseCoord(routeInput[0]))
                {
                    return;
                }
                throw new ArgumentException("The first waypoint is not a valid lat/lon.");
            }

            var wpt = wptList[index];

            if (routeInput[0] != wpt.ID)
            {
                throw new ArgumentException("The first waypoint of the route does not match the specified index in WptList.");
            }
            lastWpt = index;
            rte.AppendWaypoint(wpt);
        }
        
        /// <exception cref="InvalidIdentifierException"></exception>
        /// <exception cref="WaypointTooFarException"></exception>
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
                        throw new InvalidIdentifierException(string.Format("{0} is not a valid waypoint or airway identifier", routeInput[i]));
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

        private bool tryParseCoord(string ident)
        {
            LatLon coord;

            if (FormatDecimal.TryReadFromDecimalFormat(ident, out coord))
            {
                lastWpt = -1;
                appendWpt(new Waypoint(coord.ToDecimalFormat(), coord.Lat, coord.Lon));
                return true;
            }
            return false;
        }

        private void appendWpt(Waypoint wpt)
        {
            if (rte.Count > 0)
            {
                var last = rte.Last.Waypoint;

                if (GreatCircleDistance(wpt.Lat, wpt.Lon, last.Lat, last.Lon) > Constants.MAX_LEG_DIS)
                {
                    throw new WaypointTooFarException(string.Format("Error: {0} is more than 500nm from the last waypoint, {1}",
                                                                    last.ID,
                                                                    wpt.ID));
                }
            }
            rte.AppendWaypoint(wpt, true);
        }

        private bool tryParseWpt(string ident)
        {
            if (lastAwy == null)
            {
                var indices = wptList.FindAllByID(ident);

                if (indices == null || indices.Count == 0)
                {
                    return tryParseCoord(ident);
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

                appendWpt(wptList[lastWpt]);
            }
            else
            {
                var intermediateWpt = new AirwayNodeFinder(lastWpt, lastAwy, ident, wptList).FindWaypoints();

                if (intermediateWpt == null)
                {
                    return false;
                }

                foreach (var i in intermediateWpt)
                {
                    rte.AppendWaypoint(i, lastAwy, true);
                }

                lastWpt = wptList.FindByWaypoint(intermediateWpt.Last());
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
