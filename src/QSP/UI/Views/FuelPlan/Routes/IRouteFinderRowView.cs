using System;
using QSP.UI.Models.FuelPlan;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public interface IRouteFinderRowView : IRefreshForNavDataChange
    {
        event EventHandler IcaoChanged;
        IFinderOptionView OptionView { get; }
        bool WaypointOptionEnabled { get; }
        int? SelectedWaypointIndex { get; }
    }
}
