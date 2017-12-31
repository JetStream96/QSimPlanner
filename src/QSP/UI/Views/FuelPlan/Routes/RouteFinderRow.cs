using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Data.Interfaces;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Presenters.FuelPlan.Routes;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static CommonLibrary.LibraryExtension.IEnumerables;

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
        
        public int? SelectedWaypointIndex
        {
            get
            {
                if (!WaypointOptionEnabled || IsAirport) return null;

                try
                {
                    var latLon = ExtractLatLon(wptComboBox.Text);
                    var wptList = model.WptList();
                    return wptList.FindAllById(identTxtBox.Text)
                        .MinBy(i => wptList[i].Distance(latLon));
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool IsAirport => typeComboBox.SelectedIndex == 0;

        public string WaypointIdent => identTxtBox.Text;

        // Gets the lat and lon.
        // Inpute sample: "LAT/22.55201 LON/-121.3554"
        private static LatLon ExtractLatLon(string s)
        {
            var matchLat = Regex.Match(s, @"LAT/-?([\d.]+) ");
            double lat = double.Parse(matchLat.Groups[1].Value);

            var matchLon = Regex.Match(s, @"LON/(-?[\d.]+)");
            double lon = double.Parse(matchLon.Groups[1].Value);

            return new LatLon(lat, lon);
        }


        // TODO: Is the IMessageDisplay really needed?
        public void Init(IFinderOptionModel model)
        {
            this.model = model;
            var p = new FinderOptionPresenter(OptionControl, model);
            OptionControl.Init(model);
            typeComboBox.SelectedIndex = 0;
            fromToLbl.Text = model.IsDepartureAirport ? "From" : "To";
            OptionControl.Presenter.IcaoChanged += (s, e) => IcaoChanged?.Invoke(s, e);
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
