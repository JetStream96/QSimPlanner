using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.UI.UserControls
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
