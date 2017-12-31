using QSP.UI.Models.FuelPlan;
using System;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public interface IRouteFinderRowView : IRefreshForNavDataChange
    {
        event EventHandler IcaoChanged;
        IFinderOptionView OptionView { get; }

        /// <summary>
        /// This value represents whether the user is able to select waypoint
        /// as origin or destination.
        /// </summary>
        bool WaypointOptionEnabled { get; }

        /// <summary>
        /// Returns true if airport is currently selected.
        /// Returns false if waypoint is currently selected.
        /// </summary>
        bool IsAirport { get; }

        /// <summary>
        /// The index of waypoint selected by the user, in WaypointList.
        /// Returns null if WaypointOptionEnabled is false, 
        /// IsAirport is true, or selected waypoint is not found.
        /// </summary>
        int? SelectedWaypointIndex { get; }

        string WaypointIdent { get; }
    }
}
