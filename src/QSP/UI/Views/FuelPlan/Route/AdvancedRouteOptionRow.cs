using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Presenters.FuelPlan.Route;

namespace QSP.UI.Views.FuelPlan.Route
{
    public partial class AdvancedRouteOptionRow : UserControl, IAdvancedRouteOptionRow
    {
        public AdvancedRouteOptionRow()
        {
            InitializeComponent();
        }

        public void Init(

            FinderOptionPresenter finderPresenter,
            IMessageDisplay display,
            bool isDeparture)
        {
            finderOptionControl.Init(finderPresenter, display);
            finderOptionControl.IsOrigin = isDeparture;
        }
    }
}
