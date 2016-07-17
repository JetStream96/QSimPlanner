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
using QSP.Metar;

namespace QSP.UI.UserControls
{
    public partial class MiscInfoControl : UserControl
    {
        public MiscInfoControl()
        {
            InitializeComponent();
        }

        public void Init(AirportManager airports)
        {
            airportMapControl.Initialize(airports);
        }

        private void downloadMetarBtnClick(object sender, EventArgs e)
        {
            var icao = metarToFindTxtBox.Text.Trim().ToUpper();
            RichTextBox1.Text = MetarDownloader.TryGetMetarTaf(icao);
        }

        private void UpdateMetarMonitor()
        {

        }

        private void UpdateTabPage()
        {

        }
    }
}
