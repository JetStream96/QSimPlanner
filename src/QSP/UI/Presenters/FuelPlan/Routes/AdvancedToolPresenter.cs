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
    public class AdvancedToolPresenter
    {
        private IAdvancedRouteView view;
        private FinderOptionModel model;

        public AdvancedToolPresenter(IAdvancedRouteView view, FinderOptionModel model)
        {
            this.view = view;
            this.model = model;
        }
        
    }
}
