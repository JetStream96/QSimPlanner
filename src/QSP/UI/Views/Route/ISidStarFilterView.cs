using System;
using System.Collections.Generic;

namespace QSP.UI.Views.Route
{
    public interface ISidStarFilterView
    {
        bool IsBlacklist { get; set; }
        
        /// <summary>
        /// A list of procedures to show.
        /// </summary>
        IEnumerable<ProcedureEntry> Procedures { get; set; }

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
