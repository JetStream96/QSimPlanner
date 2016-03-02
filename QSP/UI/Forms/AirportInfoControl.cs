using System;
using System.Windows.Forms;
using QSP.RouteFinding.Airports;
using QSP.AviationTools;

namespace QSP.UI.Forms
{
    public partial class AirportInfoControl : UserControl
    {
        public AirportCollection Airports { get; set; }

        public AirportInfoControl()
        {
            InitializeComponent();
            initializeControls();
        }

        private void initializeControls()
        {
            airportNameLbl.Text = "";
            lengthUnitComboBox.SelectedIndex = 0; // Meter
            setSlopeRange(-2.0, 2.0);
        }

        private void setSlopeRange(double min, double max)
        {
            slopeComboBox.Items.Clear();

            int start = (int)Math.Round(min * 10);
            int end = (int)Math.Round(max * 10);

            for (int i = start; i <= end; i++)
            {
                slopeComboBox.Items.Add((i * 0.1).ToString("0.0"));
            }

            slopeComboBox.SelectedIndex = 20;
        }

        private void setSlope(double slope)
        {
            slope = Math.Round(slope, 1);
            var items = slopeComboBox.Items;

            if (slope + 0.05 < (double)items[0] ||
                slope - 0.05 > (double)items[items.Count - 1])
            {
                slope = Math.Abs(slope);
                setSlopeRange(-slope, slope);
            }

            slopeComboBox.SelectedIndex = (int)Math.Round((slope - (double)items[0]) * 10);
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
                airportNameLbl.Text = takeoffAirport.Name.PadLeft(24, ' ');

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
                rwyHeadingTxtBox.Text = takeoffAirport.Rwys[index].Heading;

                int elevationOppositeRwyFt = takeoffAirport.RwyElevationFt(
                    CoversionTools.RwyIdentOppositeDir(rwyComboBox.Text));

                setSlope((elevationOppositeRwyFt - elevationFt) / lengthFt * 100);
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

                lengthTxtBox.Text = ((int)len).ToString();
            }
        }
    }
}
