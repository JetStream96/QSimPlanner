using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;

namespace QSP.RouteFinding.RouteAnalyzers.Extractors
{
    // Given a route as a LinkedList<string>, Extract() method removes the 
    // nodes that are:
    // (1) origin ICAO
    // (2) SID name
    // (3) The last waypoint of SID, if the waypoint is not in wptList.
    //
    // Also returns a route containing the departure runway and 
    // SID (if SID exists).
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

        public SidExtractor(
            LinkedList<string> route,
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
                var last = origRoute.Last.Value;
                last.AirwayToNext = sidName;

                if (Math.Abs(sid.TotalDistance) > 1E-8)
                {
                    // SID has at least one waypoint.                    
                    last.DistanceToNext = sid.TotalDistance;
                    origRoute.AddLastWaypoint(sid.LastWaypoint);

                    if (route.First.Value == sid.LastWaypoint.ID &&
                        wptList.FindAllByWaypoint(sid.LastWaypoint).Count == 0)
                    {
                        route.RemoveFirst();
                    }
                }
            }
        }

        private bool tryGetSid(
            string sidName, Waypoint rwyWpt, out SidInfo result)
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
