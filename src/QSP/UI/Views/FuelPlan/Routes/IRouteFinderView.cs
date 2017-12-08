using QSP.UI.Presenters.FuelPlan.Routes;
using QSP.UI.Views.FuelPlan.Routes.Actions;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public interface IRouteFinderView :ISupportActionContextMenu
    {
        ActionContextMenuPresenter ActionMenuPresenter { get; }
    }
}