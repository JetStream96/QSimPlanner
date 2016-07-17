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
        private MetarMonitor metarMonitor;

        public MiscInfoControl()
        {
            InitializeComponent();
        }

        public void Init(
            AirportManager airports,
            bool enableBrowser)
        {
            airportMapControl.Initialize(airports);
            airportMapControl.BrowserEnabled = enableBrowser;
            metarMonitor = new MetarMonitor(RichTextBox2);
        }

        public void SetOrig(string icao)
        {
            airportMapControl.Orig = icao;
            metarMonitor.UpdateOrig(icao);
        }

        public void SetDest(string icao)
        {
            airportMapControl.Dest = icao;
            metarMonitor.UpdateOrig(icao);
        }

        // TODO: use this.
        public void SetAltn(IEnumerable<string> icao)
        {
            airportMapControl.Altn = icao;
            metarMonitor.UpdateAltn(icao);
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

        private void updateAllMetarTaf(object sender, EventArgs e)
        {

        }
    }
}
