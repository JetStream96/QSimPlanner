using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using System;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Designed to analyze a route consisting of a series of strings, containing only waypoints, 
    // lat/lon entries and airways. For a more sophisticated analyzer, use StandardRouteAnalyzer class.
    //
    // 1. Input: An array of strings, consisting of waypoint (WPT), and airway (AWY) sysbols.
    //
    // 2. All characters should be capital. 
    //
    // 3. Format: {WPT, AWY, WPT, ... , WPT}
    //    (1) If an airway is DCT, it should be omitted.
    //    (2) A WPT can also be represented with lat/lon (COORD), which has to be in decimal 
    //        representation (e.g. N32.665W122.1265).
    //
    // 4. If the format is wrong, an InvalidIdentifierException will be thrown with an message 
    //    describing the place where the problem occurs.
    //
    // 5. It's necessary to specify the index of first waypoint in WptList. 
    //    If the first entry is lat/lon (not in wptList), specifiy a negative index.
    //    If the ident of specified waypoint is different from the first word in route input string, 
    //    an ArgumentException will be thrown.
    //    

    public class BasicRouteAnalyzer
    {
        private WaypointList wptList;

        private string[] routeInput;
        private int lastWpt;         // Index in WptList. -1 if the last wpt is lat/lon.
        private string lastAwy;      // If this is null, the last element is a wpt.
        private Route rte;           // Returning value

        /// <param name="firstWaypointIndex">Use a negative value if the first waypoint is a lat/lon.</param>
        /// <exception cref="ArgumentException"></exception>
        public BasicRouteAnalyzer(string[] routeInput, WaypointList wptList, int firstWaypointIndex)
        {
            if (routeInput.Length == 0)
            {
                throw new ArgumentException("Route input should have at least 1 elements.");
            }

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

            if (wptList.WaypointExists(index) == false)
            {
                throw new ArgumentException("Wrong first waypoint index.");
            }

            var wpt = wptList[index];

            if (routeInput[0] != wpt.ID)
            {
                throw new ArgumentException("The first waypoint of the route does not match the specified index in WptList.");
            }
            lastWpt = index;
            rte.AddLastWaypoint(wpt);
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
                        throw new InvalidIdentifierException(
                            string.Format("{0} is not a valid waypoint or airway identifier", routeInput[i]));
                    }
                }
                else
                {
                    //this one must be wpt
                    if (tryParseWpt(routeInput[i]) == false)
                    {
                        throw new InvalidIdentifierException(
                            string.Format("Cannot find waypoint {0} on airway {1}", routeInput[i], lastAwy));
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
                return tryappendWpt(new Waypoint(coord.ToDecimalFormat(), coord.Lat, coord.Lon));
            }
            return false;
        }

        private bool tryappendWpt(Waypoint wpt)
        {
            rte.AddLastWaypoint(wpt, "DCT", true);
            return true;
        }

        private bool tryParseWpt(string ident)
        {
            if (lastAwy == null)
            {
                // Must be DCT.
                return tryDirectWpt(ident);
            }
            else
            {
                // Connected via airway
                return tryConnectAirway(ident);
            }
        }

        private bool tryConnectAirway(string ident)
        {
            var intermediateWpt = new AirwayNodeFinder(lastWpt, lastAwy, ident, wptList).FindWaypoints();

            if (intermediateWpt == null)
            {
                return false;
            }

            foreach (var i in intermediateWpt)
            {
                rte.AddLastWaypoint(i, lastAwy, true);
            }

            lastWpt = wptList.FindByWaypoint(intermediateWpt.Last());
            lastAwy = null;
            return true;
        }

        private bool tryDirectWpt(string ident)
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
                var wpt = rte.Last.Waypoint; 
                lastWpt = Tracks.Common.Utilities.ChooseSubsequentWpt(wpt.Lat, wpt.Lon, indices, wptList);
            }

            return tryappendWpt(wptList[lastWpt]);
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
