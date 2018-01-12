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
