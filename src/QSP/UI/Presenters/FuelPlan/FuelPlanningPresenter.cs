using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.UI.UserControls;
using QSP.UI.Views.FuelPlan;

namespace QSP.UI.Presenters.FuelPlan
{
    public class FuelPlanningPresenter
    {
        private IFuelPlanningView view;

        public FuelPlanningPresenter(IFuelPlanningView view)
        {
            this.view = view;
        }

    }
}
