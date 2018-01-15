using System;
using System.Windows.Forms;
using QSP.AviationTools;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.UI.Views.Factories;

namespace QSP.UI.UserControls.TakeoffLanding.Common.AirportInfo
{
    public partial class AirportInfoControl : UserControl
    {
        public event EventHandler IcaoChanged;

        private SlopeComboBoxController slopeController;

        public AirportManager Airports { get; set; }

        public string Icao => airportTxtBox.Text.Trim().ToUpper();

        public AirportInfoControl()
        {
            InitializeComponent();
        }

        public void Init()
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            airportNameLbl.Text = "";
            slopeController = new SlopeComboBoxController(-2.0, 2.0);
            UpdateSlopeItems();

            lengthUnitComboBox.Items.Clear();
            lengthUnitComboBox.Items.AddRange(new[] { "M", "FT" });
            lengthUnitComboBox.SelectedIndex = 0; // Meter

            reqAirportBtn.SetToolTip("Use airport and runway from 'Fuel' page.");
        }

        private void UpdateSlopeItems()
        {
            slopeComboBox.Items.Clear();

            foreach (var i in slopeController.items)
            {
                slopeComboBox.Items.Add(i.ToString("0.0"));
            }

            slopeComboBox.SelectedIndex = slopeController.NearestIndex(0.0);
        }

        private void SetSlope(double slope)
        {
            if (slopeController.ResizeRequired(slope))
            {
                double slopeAbs = Math.Abs(slope);
                slopeController.SetItems(-slopeAbs, slopeAbs);
                UpdateSlopeItems();
            }

            slopeComboBox.SelectedIndex = slopeController.NearestIndex(slope);
        }

        public void RefreshAirportInfo()
        {
            airportNameLbl.Text = "";
            rwyComboBox.Items.Clear();
            rwyComboBox.Enabled = false;

            var airportIcao = Icao;
            if (airportIcao.Length != 4 || Airports == null) return;
            var takeoffAirport = Airports[airportIcao];

            if (takeoffAirport != null && takeoffAirport.Rwys.Count > 0)
            {
                airportNameLbl.Text = takeoffAirport.Name;

                foreach (var i in takeoffAirport.Rwys)
                {
                    rwyComboBox.Items.Add(i.RwyIdent);
                }

                rwyComboBox.SelectedIndex = 0;
                rwyComboBox.Enabled = true;
            }
        }

        private void AirportTxtBoxTextChanged(object sender, EventArgs e)
        {
            RefreshAirportInfo();
            IcaoChanged?.Invoke(sender, e);
        }

        private void SetLength(int lengthFt)
        {
            var factor = lengthUnitComboBox.SelectedIndex == 0 ? Constants.FtMeterRatio : 1.0;
            lengthTxtBox.Text = Numbers.RoundToInt(lengthFt * factor).ToString();
        }

        private void RwyComboBoxIndexChanged(object sender, EventArgs e)
        {
            if (rwyComboBox.Items.Count > 0)
            {
                var takeoffAirport = Airports[Icao];
                int index = rwyComboBox.SelectedIndex;

                int elevationFt = takeoffAirport.Rwys[index].ElevationFt;
                int lengthFt = takeoffAirport.Rwys[index].LengthFt;

                SetLength(lengthFt);
                elevationTxtBox.Text = elevationFt.ToString();

                var heading = takeoffAirport.Rwys[index].Heading;
                rwyHeadingTxtBox.Text = heading.PadLeft(3, '0');

                var slope = takeoffAirport.GetSlopePercent(rwyComboBox.Text);
                SetSlope(slope == null ? 0.0 : slope.Value);
            }
        }

        private void LengthUnitSelectedChanged(object sender, EventArgs e)
        {
            if (double.TryParse(lengthTxtBox.Text, out var len))
            {
                len *= lengthUnitComboBox.SelectedIndex == 0 ?
                    Constants.FtMeterRatio :
                    Constants.MeterFtRatio;

                lengthTxtBox.Text = Numbers.RoundToInt(len).ToString();
            }
        }
    }
}
