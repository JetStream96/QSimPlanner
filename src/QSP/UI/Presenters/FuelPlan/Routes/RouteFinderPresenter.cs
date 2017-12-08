using QSP.FuelCalculation.FuelData;
using QSP.RouteFinding.Routes;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public class RouteFinderPresenter
    {
        private IRouteFinderView view;
        private FinderOptionModel model;

        public RouteGroup Route => view.ActionMenuPresenter.Route;

        //TODO:
        public double ZfwTon => 0;
        public string OrigIcao => "";
        public string DestIcao => "";
        public FuelDataItem SelectedFuelData => null;

        public RouteFinderPresenter(IRouteFinderView view, FinderOptionModel model)
        {
            this.view = view;
            this.model = model;
        }
        
    }
}
