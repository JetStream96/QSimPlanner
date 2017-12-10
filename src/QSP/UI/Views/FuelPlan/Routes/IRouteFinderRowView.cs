using System;
using QSP.UI.Models.FuelPlan;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public interface IRouteFinderRowView : IRefreshForNavDataChange
    {
        event EventHandler IcaoChanged;
    }
}
