using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static QSP.RouteFinding.TerminalProcedures.Utilities;
using static QSP.RouteFinding.Utilities;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // A immutable collection of StarEntry for a particular airport.
    public class StarCollection
    {
        private List<StarEntry> _stars;

        public ReadOnlyCollection<StarEntry> StarList
        {
            get
            {
                return new ReadOnlyCollection<StarEntry>(_stars);
            }
        }

        public StarCollection(List<StarEntry> stars)
        {
            _stars = stars;
        }

        /// <summary>
        /// Get the STAR for the specified runway or transition.
        /// </summary>
        /// <param name="star">The identifier of the STAR, without transition. e.g. GOLDN6. NOT GOLDN6.ENI</param>
        /// <param name="rwyOrTransition">Either the name of RWY or Transition. e.g. 01L, or ENI.</param>
        public StarEntry GetStar(string star, string rwyOrTransition)
        {
            foreach (var i in _stars)
            {
                if (i.Name == star && i.RunwayOrTransition == rwyOrTransition)
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the common part of the STAR.
        /// </summary>
        public StarEntry GetStar(string star)
        {
            foreach (var i in _stars)
            {
                if (i.Name == star && i.Type == EntryType.Common)
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Find all STARs available for the runway. Two STARs only different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetStarList(string rwy)
        {
            return new ProcedureSelector<StarEntry>(_stars, rwy).GetProcedureList();
        }

        public ReadOnlyCollection<Waypoint> StarWaypoints(string star, string rwy, Waypoint destRwy)
        {
            var starTrans = SplitSidStarTransition(star);

            var rwySpecificPart = GetStar(starTrans.ProcedureName, rwy);
            var commonPart = GetStar(starTrans.ProcedureName);

            var wpts = new List<Waypoint>();
            bool addedTrans = addTransitionIfNeeded(starTrans, wpts);

            if (commonPart != null)
            {
                // The last wpt of runway specific part should be the same as the first one of the common part.
                wpts.TryRemoveLast();
                wpts.AddRange(commonPart.Waypoints);
            }

            if (rwySpecificPart != null)
            {
                wpts.TryRemoveLast();
                wpts.AddRange(rwySpecificPart.Waypoints);
            }
            else
            {
                if (commonPart == null && addedTrans == false)
                {
                    throw new StarNotFoundException();
                }
            }

            wpts.Add(destRwy);

            return new ReadOnlyCollection<Waypoint>(wpts);
        }

        private bool addTransitionIfNeeded(TerminalProcedureName starTrans, List<Waypoint> wpts)
        {
            if (starTrans.TransitionName != "")
            {
                // There is transition
                var transitionPart = GetStar(starTrans.ProcedureName, starTrans.TransitionName);

                if (transitionPart == null)
                {
                    throw new StarNotFoundException();
                }

                wpts.AddRange(transitionPart.Waypoints);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns total distance of the STAR and the last wpt, regardless whether the last wpt is in wptList.
        /// If there isn't any waypoint in the STAR, this returns a distance of 0.0 and the dest runway (e.g. KLAX25L).
        /// <param name="rwy">The runway identifier. e.g. 25R </param>
        /// <param name="destRwy">The waypoint representing the destination runway.</param>
        /// </summary>
        /// <exception cref="StarNotFoundException"></exception>
        public StarInfo GetStarInfo(string star, string rwy, Waypoint destRwy)
        {
            var starWpts = StarWaypoints(star, rwy, destRwy);
            return new StarInfo(GetTotalDistance(starWpts), starWpts.First());
        }
    }
}
