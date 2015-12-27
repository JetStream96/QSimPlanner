using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using static QSP.RouteFinding.TerminalProcedures.Sid.SidHandler;
using static QSP.RouteFinding.Utilities;
using QSP.LibraryExtension;
using static QSP.RouteFinding.TerminalProcedures.Sid.Utilities;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // Collection of SidEntry for a particular airport.
    public class SidCollection
    {
        private List<SidEntry> _sids;

        public ReadOnlyCollection<SidEntry> SidList
        {
            get
            {
                return new ReadOnlyCollection<SidEntry>(_sids);
            }
        }

        public SidCollection(List<SidEntry> sids)
        {
            _sids = sids;
        }

        /// <summary>
        /// Get the SID for the specified runway or transition.
        /// </summary>
        /// <param name="sid">The identifier of the SID, without transition. e.g. AJ1M, or JFK1. NOT PORTE7.AVE</param>
        /// <param name="rwyOrTransition">Either the name of RWY or Transition. e.g. 27L, or AVE.</param>
        public SidEntry GetSid(string sid, string rwyOrTransition)
        {
            foreach (var i in _sids)
            {
                if (i.Name == sid && i.RunwayOrTransition == rwyOrTransition)
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the common part of the SID.
        /// </summary>
        public SidEntry GetSid(string sid)
        {
            foreach (var i in _sids)
            {
                if (i.Name == sid && i.Type == EntryType.Common)
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Find all SID available for the runway. Two SIDs only different in transitions are regarded as different. 
        /// If non is available a empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetSidList(string rwy)
        {
            var noTrans = new List<string>();
            var trans = new List<TerminalProcedureName>();

            foreach (var i in _sids)
            {
                if (i.Type == EntryType.Transition)
                {
                    trans.Add(new TerminalProcedureName(i.Name, i.RunwayOrTransition));
                }
                else
                {
                    noTrans.Add(i.Name);
                }
            }

            // Remove duplicates, from runway specific part and common part
            noTrans = noTrans.Distinct().ToList();

            //  SIDs which have transition(s), should not appear as one without transition.
            for (int i = noTrans.Count-1; i >=0; i--)
            {
                foreach (var j in trans)
                {
                    if (noTrans[i] == j.ProcedureName)
                    {
                        noTrans.RemoveAt(i);
                    }
                }
            }

            // Merge the two lists
            foreach (var k in trans)
            {
                noTrans.Add(k.ProcedureName);
            }
            return noTrans;
        }

        public List<Waypoint> SidWaypoints(string sid, string rwy, Waypoint origRwy)
        {
            var sidTrans = Utilities.SplitSidStarTransition(sid);

            var rwySpecificPart = GetSid(sidTrans.ProcedureName, rwy);
            var commonPart = GetSid(sidTrans.ProcedureName);

            var wpts = new List<Waypoint>();
            wpts.Add(origRwy);

            // May be null.
            if (rwySpecificPart != null)
            {
                wpts.AddRange(rwySpecificPart.Waypoints);
            }

            if (commonPart != null)
            {
                // The last wpt of runway specific part should be the same as the first one of the common part.
                wpts.TryRemoveLast();
                wpts.AddRange(commonPart.Waypoints);
            }

            if (sidTrans.TransitionName != "")
            {
                // There is transition
                var transitionPart = GetSid(sidTrans.ProcedureName, sidTrans.TransitionName);

                if (transitionPart == null)
                {
                    throw new SidNotFoundException();
                }

                // The last wpt of (runway specific + common part) should be the same as the first one of the transition part.
                wpts.TryRemoveLast();
                wpts.AddRange(transitionPart.Waypoints);
            }
            else if (rwySpecificPart == null && commonPart == null)
            {
                // No transition, both part are null
                throw new SidNotFoundException();
            }
            return wpts;
        }

        /// <summary>
        /// Returns total distance of the SID and the last wpt, regardless whether the last wpt is in wptList.
        /// If there isn't any waypoint in the SID (e.g. a vector after takeoff), this returns a distance of 0.0   
        /// and the origin runway (e.g. KLAX25L).
        /// <param name="rwy">The runway identifier. e.g. 25R </param>
        /// <param name="origRwy">The waypoint representing the origin runway.</param>
        /// </summary>
        /// <exception cref="SidNotFoundException"></exception>
        public SidInfo GetSidInfo(string sid, string rwy, Waypoint origRwy)
        {
            var wpts = SidWaypoints(sid, rwy, origRwy);
            return new SidInfo(GetTotalDistance(wpts), wpts.Last());
        }
    }
}
