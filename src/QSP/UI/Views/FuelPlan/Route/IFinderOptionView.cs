using QSP.UI.Models.FuelPlan;
using QSP.UI.Presenters.FuelPlan;
using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan.Route
{
    public interface IFinderOptionView : IMessageDisplay
    {
        bool IsOrigin { set; }
        string Icao { get; set; }

        // Can be "AUTO".
        IEnumerable<string> Runways { set; }

        // Can be "AUTO" or "NONE".
        IEnumerable<string> Procedures { set; }

        string SelectedRwy { get; set; }

        // There can be multiple or zero selected procedures. For example, when "AUTO" is selected,
        // every procedure is considered selected. When "NONE" is selected, the selection is empty.
        SelectedProcedures SelectedProcedures { get; set; }

        // Can be "NONE" or "AUTO".
        string SelectedProcedureText { get; set; }

        void ShowFilter(SidStarFileterPresenter presenter);
    }
}