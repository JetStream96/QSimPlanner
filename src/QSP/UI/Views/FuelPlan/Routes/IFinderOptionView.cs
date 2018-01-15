using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public interface IFinderOptionView : IMessageDisplay
    {
        bool IsOrigin { set; }

        /// <summary>
        /// ICAO code, all capital letters.
        /// </summary>
        string Icao { get; set; }

        /// <summary>
        /// Can be "AUTO".
        /// </summary>
        IEnumerable<string> Runways { set; }

        /// <summary>
        /// Can be "AUTO" or "NONE".
        /// </summary>
        IEnumerable<string> Procedures { set; }

        string SelectedRwy { get; set; }

        /// <summary>
        /// There can be multiple or zero selected procedures. For example, when "AUTO" 
        /// is selected, every procedure is considered selected. 
        /// When "NONE" is selected, the selection is empty.
        /// </summary>
        IEnumerable<string> SelectedProcedures { get; set; }

        /// <summary>
        /// Can be "NONE" or "AUTO".
        /// </summary>
        string SelectedProcedureText { get; set; }
    }
}