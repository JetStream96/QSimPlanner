using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Airports;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.Lists;
using static QSP.MathTools.Utilities;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.TerminalProcedures.Star;
using static QSP.RouteFinding.Tracks.Common.Utilities;

namespace QSP.RouteFinding.RouteAnalyzers
{
    // Utilizes BasicRouteAnalyzer, with the additional functionality of reading airports and SIDs/STARs.
    //
    // 1. Input: The string array, consisting of airport icao code (ICAO), airway (AWY), 
    //    and/or waypoint (WPT) sysbols.
    //           
    // 2. All characters should be capital.
    //
    // 3. Format: {ICAO, SID, WPT, AWY, WPT, ... , WPT, STAR, ICAO}
    //    (1) First ICAO must be identical to origin icao code. Last ICAO must be identical to dest icao code.
    //    (2) Both ICAO can be omitted.
    //    (3) If an airway is DCT, it should be omitted. The route will be a direct between the two waypoints.
    //    (4) SID/STAR can be omitted. The route will be a direct from/to airport.
    //
    // 4. All cases of SID/STAR are handled. These cases are handled by SidHandler and StarHandler.
    //    The last waypoint of SID and first one of STAR can be omitted.
    //
    // 5. If the format is wrong, an InvalidIdentifierException will be thrown with an message describing 
    //    the place where the problem occurs.
    //
    // 6. It's not allowed to direct from one waypoint to another which is more than 500 nm away.
    //    Otherwise an WaypointTooFarException will be thrown.
    //

    public class StandardRouteAnalyzer
    {
        private WaypointList wptList;
        private AirportManager airportList;
        private SidCollection sids;
        private StarCollection stars;

        private string origIcao;
        private string origRwy;
        private string destIcao;
        private string destRwy;
        private string[] route;

        private Route origPart;
        private Route destPart;

        int mainRouteStartIndex;
        int mainRouteEndIndex;

        Waypoint origRwyWpt;
        Waypoint destRwyWpt;

        public StandardRouteAnalyzer(string[] route,
                                     string origIcao,
                                     string origRwy,
                                     string destIcao,
                                     string destRwy,
                                     AirportManager airportList,
                                     WaypointList wptList,
                                     SidCollection sids,
                                     StarCollection stars)
        {
            this.route = route;
            this.origIcao = origIcao;
            this.origRwy = origRwy;
            this.destIcao = destIcao;
            this.destRwy = destRwy;
            this.airportList = airportList;
            this.wptList = wptList;
            this.sids = sids;
            this.stars = stars;
        }

        private void setRwyWpts()
        {
            origRwyWpt = new Waypoint(origIcao + origRwy, airportList.RwyLatLon(origIcao, origRwy));
            destRwyWpt = new Waypoint(destIcao + destRwy, airportList.RwyLatLon(destIcao, destRwy));
        }

        public Route Analyze()
        {
            setRwyWpts();
            var mainRoute = getMainRoute();

            if (mainRoute.Length == 0)
            {
                origPart.AppendRoute(destPart, "DCT");
                return origPart;
            }
            else
            {
                int chosenIndex = ChooseSubsequentWpt(origRwyWpt.Lat,
                                                      origRwyWpt.Lon,
                                                      wptList.FindAllByID(mainRoute[0]),
                                                      wptList);

                if (mainRoute.Length == 1)
                {
                    origPart.AppendWaypoint(wptList[chosenIndex], true);
                    origPart.AppendRoute(destPart, "DCT");
                    return origPart;
                }
                else
                {
                    var mainPart = new BasicRouteAnalyzer(mainRoute, wptList, chosenIndex).Analyze();
                    mergeRoutes(origPart, mainPart);
                    mergeRoutes(origPart, destPart);
                    return origPart;
                }
            }
        }

        private string[] getMainRoute()
        {
            return route.SubArray(mainRouteStartIndex, mainRouteEndIndex - mainRouteStartIndex + 1);
        }

        private bool tryGetSid(string sidName, Waypoint origRwyWpt, out SidInfo result)
        {
            try
            {
                result = sids.GetSidInfo(sidName, origRwy, origRwyWpt);
                return true;
            }
            catch
            {
                // no SID in route
                result = null;
                return false;
            }
        }

        private void createOrigRoute()
        {
            int sidPossibleIndex = route[0] == origIcao ? 1 : 0;
            string sidName = route[sidPossibleIndex];
            origPart = new Route();
            origPart.AppendWaypoint(origRwyWpt);
            SidInfo sid;

            if (tryGetSid(sidName, origRwyWpt, out sid))
            {
                if (Math.Abs(sid.TotalDistance) > 1E-8)
                {
                    // SID has at least one waypoint.
                    origPart.Last.AirwayToNext = sidName;
                    origPart.Last.DistanceToNext = sid.TotalDistance;
                    origPart.AppendWaypoint(sid.LastWaypoint);
                }

                if (route.Length <= sidPossibleIndex + 1 ||
                    sid.LastWaypoint.ID != route[sidPossibleIndex + 1])
                {
                    route[sidPossibleIndex] = sid.LastWaypoint.ID;
                    mainRouteStartIndex = sidPossibleIndex;
                }
                else
                {
                    mainRouteStartIndex = sidPossibleIndex + 1;
                }
            }
            else
            {
                // no SID in route
                mainRouteStartIndex = sidPossibleIndex;
            }
        }

        private void mergeRoutes(Route original, Route RouteToMerge)
        {
            if (original.Last.Equals(RouteToMerge.First))
            {
                original.ConnectRoute(RouteToMerge);
            }
            else
            {
                original.AppendRoute(RouteToMerge, "DCT");
            }
        }

        private bool tryGetStar(string StarName, Waypoint destRwyWpt, out StarInfo result)
        {
            try
            {
                result = stars.GetStarInfo(StarName, destRwy, destRwyWpt);
                return true;
            }
            catch
            {
                // no Star in route
                result = null;
                return false;
            }
        }

        private void createDestRoute()
        {
            int starPossibleIndex = route.Last() == destIcao ? route.Length - 2 : route.Length - 1;
            string starName = route[starPossibleIndex];
            destPart = new Route();
            StarInfo star;

            if (tryGetStar(starName, destRwyWpt, out star))
            {
                if (Math.Abs(star.TotalDistance) > 1E-8)
                {
                    // STAR has at least one waypoint.
                    destPart.AppendWaypoint(star.FirstWaypoint, starName, star.TotalDistance);
                }

                if (starPossibleIndex == 0 ||
                    star.FirstWaypoint.ID != route[starPossibleIndex - 1])
                {
                    route[starPossibleIndex] = star.FirstWaypoint.ID;
                    mainRouteEndIndex = starPossibleIndex;
                }
                else
                {
                    mainRouteEndIndex = starPossibleIndex - 1;
                }
            }
            else
            {
                // no STAR in route
                mainRouteEndIndex = starPossibleIndex;
            }
            destPart.AppendWaypoint(destRwyWpt);
        }
    }
}
