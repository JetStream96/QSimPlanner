using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.TerminalProcedures
{
    // 'Procedure' refers to a SID or a STAR in the following paragraph.
    //
    // Find all procedures available for the runway.
    //
    // The rules are:
    // (1) An entry which is runway-specific and matches the given 
    //     runway is selected.
    // (2) An entry with a procedure that does not have a runway-specific 
    //     part in the list, is usable regardless of runway.
    //     Therefore it's selected.
    // (3) If the procedure of an entry has runway-specific part, but 
    //     none of the runway-specific parts matches the given 
    //     runway, it is not selected.
    // (4) An entry which is a transition is selected if and only if
    //     at least one entry with the same ProcedureName is selected 
    //     by the criterions above.
    // (5) If a procedure has transitions then it cannot appear as 
    //     one without transition. 
    //     e.g. If the selected procedures are SID1, SID1.TRANS1, 
    //     then the final selected items should only contain SID1.TRANS1.
    //

    public class ProcedureSelector<T> where T : IProcedureEntry
    {
        private IEnumerable<T> procedureEntries;
        private string rwy;

        public ProcedureSelector(IEnumerable<T> procedureEntries, string rwy)
        {
            this.procedureEntries = procedureEntries;
            this.rwy = rwy;
        }

        /// <summary>
        /// Find all procedure available for the runway. Two procedures 
        /// only different in transitions are regarded as different. 
        /// If none is available an empty list is returned.
        /// </summary>
        public List<string> GetProcedureList()
        {
            var groups = procedureEntries.GroupBy(p => p.Name);

            // Rule (1) - (4) satisfied
            var filterd =
                groups.Where(g =>
                IsRunwaySpecific(g) == false || ContainDesiredRwy(g));

            // Rule (5)
            var names = filterd.SelectMany(f => FullNames(f));

            return names.ToList();
        }

        private IEnumerable<string> FullNames(IGrouping<string, T> group)
        {
            string name = null;
            bool hasTransition = false;

            foreach (var i in group)
            {
                if (i.Type == EntryType.Transition)
                {
                    hasTransition = true;
                    yield return i.Name + "." + i.RunwayOrTransition;
                }
                else
                {
                    name = i.Name;
                }
            }

            if (hasTransition == false)
            {
                yield return name;
            }
        }

        private bool ContainDesiredRwy(IGrouping<string, T> group)
        {
            return group.Any(
                x => x.Type == EntryType.RwySpecific &&
                x.RunwayOrTransition == rwy);
        }

        private bool IsRunwaySpecific(IGrouping<string, T> group)
        {
            return group.Any(i => i.Type == EntryType.RwySpecific);
        }
    }
}
