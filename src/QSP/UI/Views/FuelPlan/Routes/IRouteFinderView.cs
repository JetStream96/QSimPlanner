using System;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Presenters.FuelPlan.Routes;
using QSP.UI.Views.FuelPlan.Routes.Actions;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public interface IRouteFinderView : ISupportActionContextMenu, IRefreshForNavDataChange
    {
        event EventHandler OrigIcaoChanged;
        event EventHandler DestIcaoChanged;
        ActionContextMenuPresenter ActionMenuPresenter { get; }
        string OrigIcao { get; }
        string DestIcao { get; }
    }
}