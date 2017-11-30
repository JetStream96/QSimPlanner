using System;
using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan.Route
{
    public interface ISidStarFilterView
    {
        bool IsBlacklist { get; set; }

        IEnumerable<ProcedureEntry> SelectedProcedures();
        
        /// <summary>
        /// Fires when the user completes the selection. E.g. when the selection form closes.
        /// </summary>
        event EventHandler SelectionComplete;
    }

    public struct ProcedureEntry
    {
        public string Name;
        public bool Ticked;
    }
}