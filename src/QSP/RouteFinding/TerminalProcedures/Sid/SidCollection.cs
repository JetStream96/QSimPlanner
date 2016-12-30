using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // An immutable collection of SidEntry for a particular airport.
    public sealed class SidCollection
    {
        public IEnumerable<SidEntry> SidList { get; private set; }

        public SidCollection(IEnumerable<SidEntry> sids)
        {
            SidList = sids;
        }

        /// <summary>
        /// Get the SID for the specified runway or transition.
        /// </summary>
        /// <param name="sid">The identifier of the SID, without 
        /// transition. e.g. AJ1M, or JFK1. NOT PORTE7.AVE</param>
        /// <param name="rwyOrTransition">Either the name of RWY or 
        /// Transition. e.g. 27L, or AVE.</param>
        public SidEntry GetSid(string sid, string rwyOrTransition)
        {
            return SidList.FirstOrDefault(
                i => i.Name == sid && i.RunwayOrTransition == rwyOrTransition);
        }

        /// <summary>
        /// Get the common part of the SID.
        /// </summary>
        public SidEntry GetSid(string sid)
        {
            return SidList.FirstOrDefault(i => i.Name == sid && i.Type == EntryType.Common);
        }

        /// <summary>
        /// Find all SID available for the runway. Two SIDs which are 
        /// only different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetSidList(string rwy)
        {
            return new ProcedureSelector<SidEntry>(SidList, rwy)
                .GetProcedureList();
        }

        public SidWaypoints SidWaypoints(
            string sid, string rwy, Waypoint origRwy)
        {
            var sidTrans = new TerminalProcedureName(sid);

            var rwySpecificPart = GetSid(sidTrans.ProcedureName, rwy);
            var commonPart = GetSid(sidTrans.ProcedureName);

            bool endsWithVector = false;
            var wpts = new List<Waypoint>();
            wpts.Add(origRwy);

            // May be null.
            if (rwySpecificPart != null)
            {
                wpts.AddRange(rwySpecificPart.Waypoints);
                endsWithVector = rwySpecificPart.EndWithVector;
            }

            if (commonPart != null)
            {
                // The last wpt of runway specific part should be 
                // the same as the first one of the common part.
                wpts.TryRemoveLast();
                wpts.AddRange(commonPart.Waypoints);
                endsWithVector = commonPart.EndWithVector;
            }

            AddTransitionIfNeeded(
                sidTrans,
                rwySpecificPart,
                ref endsWithVector,
                commonPart,
                wpts);

            return new SidWaypoints(wpts, endsWithVector);
        }

        private void AddTransitionIfNeeded(
            TerminalProcedureName sidTrans,
            SidEntry rwySpecificPart,
            ref bool endsWithVector,
            SidEntry commonPart,
            List<Waypoint> wpts)
        {
            if (sidTrans.TransitionName != "")
            {
                // There is transition
                var transitionPart = GetSid(sidTrans.ProcedureName, sidTrans.TransitionName);

                if (transitionPart == null)
                {
                    throw new SidNotFoundException();
                }

                // The last wpt of (runway specific + common part) should 
                // be the same as the first one of the transition part.
                wpts.TryRemoveLast();
                wpts.AddRange(transitionPart.Waypoints);
                endsWithVector = transitionPart.EndWithVector;
            }
            else if (rwySpecificPart == null && commonPart == null)
            {
                // No transition, both part are null
                throw new SidNotFoundException();
            }
        }

        /// <summary>
        /// Returns total distance of the SID and the last wpt, regardless 
        /// whether the last wpt is in wptList.
        /// If there isn't any waypoint in the SID 
        /// (e.g. a vector after takeoff), this returns a distance of 0.0   
        /// and the origin runway (e.g. KLAX25L).
        /// <param name="rwy">The runway identifier. e.g. 25R </param>
        /// <param name="origRwy">The waypoint representing the 
        /// origin runway.</param>
        /// </summary>
        /// <exception cref="SidNotFoundException"></exception>
        public SidInfo GetSidInfo(string sid, string rwy, Waypoint origRwy)
        {
            var sidWpts = SidWaypoints(sid, rwy, origRwy);
            var wpts = sidWpts.Waypoints;
            return new SidInfo(wpts.TotalDistance(), wpts.Last(), sidWpts.EndsWithVector);
        }
    }
}
