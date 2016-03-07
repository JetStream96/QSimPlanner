using QSP.AviationTools;
using QSP.RouteFinding.Airports;
using System;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.Common.AirportInfo
{
    public partial class AirportInfoControl : UserControl
    {
        private SlopeComboBoxController slopeController;

        public AirportManager Airports { get; set; }

        public AirportInfoControl()
        {
            InitializeComponent();
            initializeControls();
        }

        private void initializeControls()
        {
            airportNameLbl.Text = "";
            slopeController = new SlopeComboBoxController(-2.0, 2.0);
            updateSlopeItems();

            lengthUnitComboBox.Items.Clear();
            lengthUnitComboBox.Items.AddRange(new object[] { "M", "FT" });
            lengthUnitComboBox.SelectedIndex = 0; // Meter
        }

        private void updateSlopeItems()
        {
            slopeComboBox.Items.Clear();

            foreach (var i in slopeController.items)
            {
                slopeComboBox.Items.Add(i.ToString("0.0"));
            }

            slopeComboBox.SelectedIndex = slopeController.NearestIndex(0.0);
        }

        private void setSlope(double slope)
        {
            if (slopeController.ResizeRequired(slope))
            {
                double slopeAbs = Math.Abs(slope);
                slopeController.SetItems(-slopeAbs, slopeAbs);
                updateSlopeItems();
            }

            slopeComboBox.SelectedIndex = slopeController.NearestIndex(slope);
        }

        private void airportTxtBox_TextChanged(object sender, EventArgs e)
        {
            airportNameLbl.Text = "";
            rwyComboBox.Items.Clear();
            rwyComboBox.Enabled = false;

            var icao = airportTxtBox.Text; // TODO: trim this

            if (icao.Length != 4)
            {
                return;
            }

            var takeoffAirport = Airports.Find(icao);

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

        private void setLength(int lengthFt)
        {
            switch (lengthUnitComboBox.SelectedIndex)
            {
                case 0: // meter
                    lengthTxtBox.Text = ((int)(lengthFt * Constants.FT_M_ratio)).ToString();
                    break;

                case 1: // ft
                    lengthTxtBox.Text = lengthFt.ToString();
                    break;
            }
        }

        private void rwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rwyComboBox.Items.Count > 0)
            {
                var takeoffAirport = Airports.Find(airportTxtBox.Text);
                int index = rwyComboBox.SelectedIndex;

                int elevationFt = takeoffAirport.Rwys[index].Elevation;
                int lengthFt = takeoffAirport.Rwys[index].Length;

                setLength(lengthFt);
                elevationTxtBox.Text = elevationFt.ToString();
                rwyHeadingTxtBox.Text = takeoffAirport.Rwys[index].Heading.ToString().PadLeft(3, '0');

                int elevationOppositeRwyFt = takeoffAirport.RwyElevationFt(
                    CoversionTools.RwyIdentOppositeDir(rwyComboBox.Text));

                setSlope((elevationOppositeRwyFt - elevationFt) * 100.0 / lengthFt);
            }
        }

        public virtual void getMetarBtn_Click(object sender, EventArgs e)
        {
        }

        private void lengthUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            double len;

            if (double.TryParse(lengthTxtBox.Text, out len))
            {
                if (lengthUnitComboBox.SelectedIndex == 0)
                {
                    // ft -> m
                    len *= Constants.FT_M_ratio;
                }
                else
                {
                    // m -> ft
                    len *= Constants.M_FT_ratio;
                }

                lengthTxtBox.Text = ((int)Math.Round(len)).ToString();
            }
        }
    }
}
