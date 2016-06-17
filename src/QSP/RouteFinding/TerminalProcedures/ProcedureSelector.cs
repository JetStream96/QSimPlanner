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
    // (3) An entry with a procedure that have a runway-specific part in 
    //     the list, but none of the runway-specific parts matches the 
    //     given runway, is not selected.
    // (4) An entry which is a transition is selected if and only if
    //     at least one entry with the same procedure name (ProcedureName) 
    //     is selected by the criterions above.
    // (5) If a procedure has transitions then it cannot appear as 
    //     one without transition. 
    //     e.g. If the selected procedures are SID1, SID1.TRANS1, 
    //     then the final selected items should only contain SID1.TRANS1.
    //

    public class ProcedureSelector<T> where T : IProcedureEntry
    {
        private List<T> procedureEntries;
        private string rwy;

        public ProcedureSelector(List<T> procedureEntries, string rwy)
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
            List<string> noTrans;
            List<TerminalProcedureName> trans;

            classifyProcedures(out noTrans, out trans);

            // Remove transitions that are not available for the runway.
            removeTransitionWrongRunway(noTrans, trans);

            // Procedures which have transition(s), should not appear as 
            // one without transition.
            removeProcedureWithoutTransition(noTrans, trans);

            // Merge the two lists
            foreach (var k in trans)
            {
                noTrans.Add(k.ProcedureName + '.' + k.TransitionName);
            }
            return noTrans;
        }

        private void classifyProcedures(
            out List<string> noTrans, out List<TerminalProcedureName> trans)
        {
            noTrans = new List<string>();
            trans = new List<TerminalProcedureName>();

            foreach (var i in procedureEntries)
            {
                if (i.Type == EntryType.Transition)
                {
                    trans.Add(
                        new TerminalProcedureName(
                            i.Name, i.RunwayOrTransition));
                }
                else if (
                    (i.Type == EntryType.RwySpecific &&
                    i.RunwayOrTransition == rwy) ||
                    (i.Type == EntryType.Common &&
                    runwaySpecificPartExists(i.Name) == false))
                {
                    // RwySpecific or common parts which are available 
                    // for the particular runway.
                    noTrans.Add(i.Name);
                }
            }

            // Remove duplicates, from runway specific part and common part
            noTrans = noTrans.Distinct().ToList();
        }

        private bool runwaySpecificPartExists(string procedureName)
        {
            return procedureEntries.Exists(i =>
            i.Type == EntryType.RwySpecific &&
            i.Name == procedureName);
        }

        private static void removeTransitionWrongRunway(
            List<string> noTrans, List<TerminalProcedureName> trans)
        {
            for (int i = trans.Count - 1; i >= 0; i--)
            {
                bool removeEntry = true;

                foreach (var j in noTrans)
                {
                    if (j == trans[i].ProcedureName)
                    {
                        removeEntry = false;
                        break;
                    }
                }

                if (removeEntry)
                {
                    trans.RemoveAt(i);
                }
            }
        }

        private static void removeProcedureWithoutTransition(
            List<string> noTrans, List<TerminalProcedureName> trans)
        {
            for (int i = noTrans.Count - 1; i >= 0; i--)
            {
                foreach (var j in trans)
                {
                    if (noTrans[i] == j.ProcedureName)
                    {
                        noTrans.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
