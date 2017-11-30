using CommonLibrary.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Views.FuelPlan.Route;
using System.Collections.Generic;
using System.Linq;

namespace QSP.UI.Presenters.FuelPlan
{
    // It's important to note that some procedures in the ProcedureFilter may not exist 
    // because user just changed the NavData version after starting the program. The implementation
    // of this presenter and its corresponding views must handle this situation.

    public class SidStarFilterPresenter
    {
        public ISidStarFilterView View { get; private set; }

        public bool IsSid { get; private set; }
        private string icao;
        private string rwy;
        private List<string> procedures;
        private ProcedureFilter procFilter;

        public SidStarFilterPresenter(
            ISidStarFilterView view,
            string icao,
            string rwy,
            List<string> procedures,
            bool isSid,
            ProcedureFilter procFilter)
        {
            this.View = view;
            this.icao = icao;
            this.rwy = rwy;
            this.procedures = procedures;
            this.IsSid = isSid;
            this.procFilter = procFilter;
        }

        public (bool isBlacklist, IEnumerable<ProcedureEntry> e) AllProcedures()
        {
            var entry = procFilter.TryGetEntry(icao, rwy, IsSid);
            if (entry != null)
            {
                var ticked = entry.Procedures.ToHashSet();
                var proc = procedures.Select(p =>
                      new ProcedureEntry() { Name = p, Ticked = ticked.Contains(p) });
                return (entry.IsBlackList, proc);
            }
            else
            {
                var proc= procedures.Select(p =>
                     new ProcedureEntry() { Name = p, Ticked = false });
                return (true, proc);
            }
        }

        private IEnumerable<string> GetSelectedProcedures() =>
            View.SelectedProcedures().Where(p => p.Ticked).Select(p => p.Name);

        /// <summary>
        /// Update the procedure filter.
        /// </summary>
        public void UpdateFilter()
        {
            procFilter[icao, rwy, IsSid] = new FilterEntry(
                View.IsBlacklist,
                GetSelectedProcedures().ToList(),
                IsSid);
        }
    }
}
