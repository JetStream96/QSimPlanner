using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public class AdvancedToolRowPresenter
    {
        private IAdvancedRouteRowView view;

        public AdvancedToolRowPresenter(
            IAdvancedRouteRowView view,
            FinderOptionPresenter finderPresenter,
            IMessageDisplay display,
            bool isDeparture)
        {
            this.view = view;
        }
    }
}
