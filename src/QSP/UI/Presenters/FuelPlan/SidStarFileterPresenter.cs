using CommonLibrary.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures;
using System.Collections.Generic;
using System.Linq;
using QSP.UI.Views.FuelPlan.Route;

namespace QSP.UI.Presenters.FuelPlan
{
    public class SidStarFileterPresenter
    {
        private ISidStarFilterView view;

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
            this.view = view;
            this.icao = icao;
            this.rwy = rwy;
            this.procedures = procedures;
            this.IsSid = isSid;
            this.procFilter = procFilter;
        }

        public void InitView()
        {
            var entry = procFilter.TryGetEntry(icao, rwy);
            if (entry != null)
            {
                view.IsBlacklist = entry.IsBlackList;
                var ticked = entry.Procedures.ToHashSet();
                view.Procedures = procedures.Select(
                    p => new ProcedureEntry() { Name = p, Ticked = ticked.Contains(p) });
            }
            else
            {
                view.Procedures = procedures.Select(p =>
                    new ProcedureEntry() { Name = p, Ticked = false });
            }
        }

        private IEnumerable<string> GetSelectedProcedures() =>
            view.Procedures.Where(p => p.Ticked).Select(p => p.Name);

        /// <summary>
        /// Update the procedure filter.
        /// </summary>
        public void UpdateFilter()
        {
            procFilter[icao, rwy] = new FilterEntry(
                view.IsBlacklist,
                GetSelectedProcedures().ToList());
        }
    }
}
