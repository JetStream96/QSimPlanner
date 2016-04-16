using System;
using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a LinkedList<string>, this class removes the nodes that are
    // origin ICAO, SID, or the last waypoint of SID which is not in wptList.
    //
    // Returns a route containing the departure runway and SID (if SID exists).
    //

    public class SidExtractor
    {
        private WaypointList wptList;
        private SidCollection sids;
        private Waypoint rwyWpt;
        private string icao;
        private string rwy;

        private LinkedList<string> route;
        private Route origRoute;

        public SidExtractor(LinkedList<string> route,
                            string icao,
                            string rwy,
                            Waypoint rwyWpt,
                            WaypointList wptList,
                            SidCollection sids)
        {
            this.route = route;
            this.icao = icao;
            this.rwy = rwy;
            this.rwyWpt = rwyWpt;
            this.wptList = wptList;
            this.sids = sids;
        }

        public Route Extract()
        {
            origRoute = new Route();
            origRoute.AddLastWaypoint(rwyWpt);

            if (route.Count > 0)
            {
                createOrigRoute();
            }
            return origRoute;
        }

        private void createOrigRoute()
        {
            if (route.First.Value == icao)
            {
                route.RemoveFirst();
            }

            string sidName = route.First.Value;
            SidInfo sid;

            if (tryGetSid(sidName, rwyWpt, out sid))
            {
                route.RemoveFirst();
                origRoute.Last.AirwayToNext = sidName;

                if (Math.Abs(sid.TotalDistance) > 1E-8)
                {
                    // SID has at least one waypoint.                    
                    origRoute.Last.DistanceToNext = sid.TotalDistance;
                    origRoute.AddLastWaypoint(sid.LastWaypoint);

                    if (route.First.Value == sid.LastWaypoint.ID &&
                        wptList.FindAllByWaypoint(sid.LastWaypoint).Count == 0)
                    {
                        // See rule (1) of 4. above.  
                        route.RemoveFirst();
                    }
                }
            }
        }

        private bool tryGetSid(string sidName, Waypoint rwyWpt, out SidInfo result)
        {
            try
            {
                result = sids.GetSidInfo(sidName, rwy, rwyWpt);
                return true;
            }
            catch
            {
                // no SID in route
                result = null;
                return false;
            }
        }
    }
}
