using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Presenters.FuelPlan.Routes;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public partial class AdvancedToolRow : UserControl, IAdvancedRouteRowView
    {
        public AdvancedToolRow()
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

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isAirport = typeComboBox.SelectedIndex == 0;
            finderOptionControl.Enabled = isAirport;
            identTxtBox.Enabled = !isAirport;
            wptComboBox.Enabled = !isAirport;
        }
    }
}
