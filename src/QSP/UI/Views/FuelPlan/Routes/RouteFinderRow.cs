using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Presenters.FuelPlan.Routes;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan.Routes
{
    public partial class RouteFinderRow : UserControl, IRouteFinderRowView
    {
        private IFinderOptionModel model;

        public event EventHandler IcaoChanged;

        public IFinderOptionView OptionView => OptionControl;

        public RouteFinderRow()
        {
            InitializeComponent();
        }

        public string Icao => OptionControl.Icao;
        public string Rwy => OptionControl.SelectedRwy;

        public bool WaypointOptionEnabled
        {
            get => typeLayoutPanel.Visible;

            set
            {
                if (!value) typeComboBox.SelectedIndex = 0;
                typeLayoutPanel.Visible = value;
                wptLayoutPanel.Visible = value;
            }
        }

        // TODO: Is the IMessageDisplay really needed?
        public void Init(IFinderOptionModel model, IMessageDisplay display)
        {
            this.model = model;
            var p = new FinderOptionPresenter(OptionControl, model);
            OptionControl.Init(model, display);
            typeComboBox.SelectedIndex = 0;
            fromToLbl.Text = model.IsDepartureAirport ? "From" : "To";
        }

        public void OnNavDataChange()
        {
            OptionControl.Presenter.OnNavDataChange();
            RefreshWptListCBox();
        }

        private void RefreshWptListCBox()
        {
            var items = wptComboBox.Items;
            items.Clear();
            var wptList = model.WptList();
            var indices = wptList.FindAllById(identTxtBox.Text);
            if (indices.Count == 0) return;

            items.AddRange(indices.Select(i =>
            {
                var wpt = wptList[i];
                return "LAT/" + wpt.Lat.ToString("F4") +
                  "  LON/" + wpt.Lon.ToString("F4");
            }).ToArray());

            wptComboBox.SelectedIndex = 0;
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isAirport = typeComboBox.SelectedIndex == 0;
            OptionControl.Enabled = isAirport;
            identTxtBox.Enabled = !isAirport;
            wptComboBox.Enabled = !isAirport;
        }

        private void identTxtBox_TextChanged(object sender, EventArgs e)
        {
            RefreshWptListCBox();
        }
    }
}
