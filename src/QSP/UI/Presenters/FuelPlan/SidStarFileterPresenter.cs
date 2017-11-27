using CommonLibrary.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Views.FuelPlan.Route;
using System.Collections.Generic;
using System.Linq;

namespace QSP.UI.Presenters.FuelPlan
{
    public class SidStarFileterPresenter
    {
        public ISidStarFilterView View { get; private set; }

        public bool IsSid { get; private set; }
        private string icao;
        private string rwy;
        private List<string> procedures;
        private ProcedureFilter procFilter;

        public SidStarFileterPresenter(
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
            var entry = procFilter.TryGetEntry(icao, rwy);
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
            procFilter[icao, rwy] = new FilterEntry(
                View.IsBlacklist,
                GetSelectedProcedures().ToList());
        }
    }
}
