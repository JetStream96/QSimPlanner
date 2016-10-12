using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // A immutable collection of StarEntry for a particular airport.
    public sealed class StarCollection
    {
        public IEnumerable<StarEntry> StarList { get; private set; }

        public StarCollection(IEnumerable<StarEntry> stars)
        {
            StarList = stars;
        }

        /// <summary>
        /// Get the STAR for the specified runway or transition.
        /// </summary>
        /// <param name="star">The identifier of the STAR, without transition.
        /// e.g. GOLDN6. NOT GOLDN6.ENI</param>
        /// <param name="rwyOrTransition">Either the name of RWY or Transition.
        /// e.g. 01L, or ENI.</param>
        public StarEntry GetStar(string star, string rwyOrTransition)
        {
            return StarList.FirstOrDefault(i =>
                i.Name == star && i.RunwayOrTransition == rwyOrTransition);
        }

        /// <summary>
        /// Get the common part of the STAR.
        /// </summary>
        public StarEntry GetStar(string star)
        {
            return StarList.FirstOrDefault(
                i => i.Name == star && i.Type == EntryType.Common);
        }

        /// <summary>
        /// Find all STARs available for the runway. Two STARs only 
        /// different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetStarList(string rwy)
        {
            return new ProcedureSelector<StarEntry>(StarList, rwy)
                .GetProcedureList();
        }

        public IReadOnlyList<Waypoint> StarWaypoints(
            string star, string rwy, Waypoint destRwy)
        {
            var starTrans = new TerminalProcedureName(star);

            var rwySpecificPart = GetStar(starTrans.ProcedureName, rwy);
            var commonPart = GetStar(starTrans.ProcedureName);

            var wpts = new List<Waypoint>();
            bool addedTrans = AddTransitionIfNeeded(starTrans, wpts);

            if (commonPart != null)
            {
                // The last wpt of runway specific part should be the 
                // same as the first one of the common part.
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

            return wpts;
        }

        private bool AddTransitionIfNeeded(
            TerminalProcedureName starTrans, List<Waypoint> wpts)
        {
            if (starTrans.TransitionName != "")
            {
                // There is transition
                var transitionPart = GetStar(
                    starTrans.ProcedureName, starTrans.TransitionName);

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
        /// Returns total distance of the STAR and the last wpt,
        /// regardless whether the last wpt is in wptList.
        /// If there isn't any waypoint in the STAR, this returns
        /// a distance of 0.0 and the dest runway (e.g. KLAX25L).
        /// <param name="rwy">The runway identifier. e.g. 25R </param>
        /// <param name="destRwy">The waypoint representing the 
        /// destination runway.</param>
        /// </summary>
        /// <exception cref="StarNotFoundException"></exception>
        public StarInfo GetStarInfo(string star, string rwy, Waypoint destRwy)
        {
            var starWpts = StarWaypoints(star, rwy, destRwy);
            return new StarInfo(starWpts.TotalDistance(), starWpts.First());
        }
    }
}
