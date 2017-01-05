using QSP.AviationTools;
using QSP.RouteFinding.Airports;
using System;
using System.Windows.Forms;
using QSP.UI.Factories;
using static QSP.MathTools.Doubles;

namespace QSP.UI.ToLdgModule.Common.AirportInfo
{
    public partial class AirportInfoControl : UserControl
    {
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
            lengthUnitComboBox.Items.AddRange(new string[] { "M", "FT" });
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

            if (airportIcao.Length != 4 || Airports == null)
            {
                return;
            }

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

        private void airportTxtBox_TextChanged(object sender, EventArgs e)
        {
            RefreshAirportInfo();
        }

        private void SetLength(int lengthFt)
        {
            switch (lengthUnitComboBox.SelectedIndex)
            {
                case 0: // meter
                    int len = RoundToInt(lengthFt * Constants.FtMeterRatio);
                    lengthTxtBox.Text = len.ToString();
                    break;

                case 1: // ft
                    lengthTxtBox.Text = lengthFt.ToString();
                    break;
            }
        }

        private void rwyComboBoxIndexChanged(object sender, EventArgs e)
        {
            if (rwyComboBox.Items.Count > 0)
            {
                var takeoffAirport = Airports[Icao];
                int index = rwyComboBox.SelectedIndex;

                int elevationFt = takeoffAirport.Rwys[index].Elevation;
                int lengthFt = takeoffAirport.Rwys[index].LengthFt;

                SetLength(lengthFt);
                elevationTxtBox.Text = elevationFt.ToString();

                var heading = takeoffAirport.Rwys[index].Heading;
                rwyHeadingTxtBox.Text = heading.PadLeft(3, '0');

                int elevationOppositeRwyFt = takeoffAirport.RwyElevationFt(
                    RwyIdentConversion.RwyIdentOppositeDir(rwyComboBox.Text));

                SetSlope((elevationOppositeRwyFt - elevationFt) * 100.0 / lengthFt);
            }
        }
        
        private void lengthUnitSelectedChanged(object sender, EventArgs e)
        {
            double len;

            if (double.TryParse(lengthTxtBox.Text, out len))
            {
                if (lengthUnitComboBox.SelectedIndex == 0)
                {
                    // ft -> m
                    len *= Constants.FtMeterRatio;
                }
                else
                {
                    // m -> ft
                    len *= Constants.MeterFtRatio;
                }

                lengthTxtBox.Text = ((int)Math.Round(len)).ToString();
            }
        }
    }
}
