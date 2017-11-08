using QSP.UI.Views.FuelPlan;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternatePresenter
    {
        private IAlternateView view;

        public AlternatePresenter(IAlternateView view)
        {
            this.view = view;
        }


    }
}