using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.RouteFinding.Airports;
using QSP.AviationTools;

namespace QSP.UI.Forms
{
    public partial class AirportInfoControl : UserControl
    {
        public AirportCollection airports { get; set; }

        public AirportInfoControl()
        {
            InitializeComponent();
            initializeControls();
        }

        private void initializeControls()
        {
            airportNameLbl.Text = "";
            lengthUnitComboBox.SelectedIndex = 0; // Meter
            setSlopeComboBox();
        }

        private void setSlopeComboBox()
        {
            slopeComboBox.Items.Clear();

            for (int i = -20; i <= 20; i++)
            {
                slopeComboBox.Items.Add((i * 0.1).ToString("0.0"));
            }

            slopeComboBox.SelectedIndex = 20;
        }

        private void setSlope()
        {

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

            var takeoffAirport = airports.Find(icao);

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

        private static int rwyElevFt(Airport airport, string ident)
        {
            for (int j = 0; j < airport.Rwys.Count; j++)
            {
                if (airport.Rwys[j].RwyIdent == ident)
                {
                    return airport.Rwys[j].Elevation;  
                }
            }

            throw new ArgumentException("Not found.");
        }

        private void rwyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rwyComboBox.Items.Count > 0)
            {
                var takeoffAirport = airports.Find(airportTxtBox.Text);
                int index = rwyComboBox.SelectedIndex;

                int elevationFt = takeoffAirport.Rwys[index].Elevation;
                int lengthFt = takeoffAirport.Rwys[index].Length;

                setLength(lengthFt);
                elevationTxtBox.Text = elevationFt.ToString();
                rwyHeadingTxtBox.Text = takeoffAirport.Rwys[index].Heading;
                
                int elevationOppositeRwyFt = rwyElevFt(
                    takeoffAirport,
                    CoversionTools.RwyIdentOppositeDir(rwyComboBox.Text));
                
                
                    Slope.Text = Convert.ToString(Math.Round((double)(elevationOppositeRwyFt - elevationFt) / lengthFt * 100 * 10) / 10);
                
            }
        }
    }
}
