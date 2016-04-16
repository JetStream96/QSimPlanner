using QSP.Metar;
using QSP.UI.ToLdgModule.Common.AirportInfo;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.Common
{
    public class AutoWeatherSetter
    {
        private WeatherInfoControl wxControl;
        private AirportInfoControl airportControl;
        private string metar;

        public AutoWeatherSetter(WeatherInfoControl wxControl, 
                                 AirportInfoControl airportControl)
        {
            this.wxControl = wxControl;
            this.airportControl = airportControl;
        }

        public void Subscribe()
        {
            wxControl.GetMetarBtn.Click += getMetarClicked;
            wxControl.ViewMetarBtn.Click += viewMetarClicked;
        }

        private void enableDnBtn()
        {
            var btn = wxControl.GetMetarBtn;

            btn.Enabled = true;
            btn.BackColor = Color.DarkSlateGray;
            btn.Text = "Import METAR";
        }

        private void disableDnBtn()
        {
            var btn = wxControl.GetMetarBtn;

            btn.Enabled = false;
            btn.BackColor = Color.Gray;
            btn.Text = "Downloading ...";
        }

        private void viewMetarClicked(object sender, EventArgs e)
        {
            var frm = new MetarForm();
            frm.icaoTxtBox.Text = airportControl.Icao;
            frm.icaoTxtBox.Enabled = false;
            frm.resultRichTxtBox.Text = metar ?? "";
            frm.sendBtn.Visible = false;
            frm.downloadBtn.Visible = false;
            frm.getTafCheckBox.Visible = false;
            frm.ShowDialog();
        }

        // Get metar functions.
        private async void getMetarClicked(object sender, EventArgs e)
        {
            disableDnBtn();
            wxControl.pictureBox1.Visible = false;

            string icao = airportControl.Icao;
            metar = null;

            bool metarAcquired =
                 await Task.Run(
                     () => MetarDownloader.TryGetMetar(icao, out metar));

            if (metarAcquired)
            {
                var w = wxControl;

                if (WeatherAutoFiller.Fill(
                    metar,
                    w.windDirTxtBox,
                    w.windSpdTxtBox,
                    w.oatTxtBox,
                    w.tempUnitComboBox,
                    w.pressTxtBox,
                    w.pressUnitComboBox) == false)
                {
                    MessageBox.Show(
                       @"Unable to fill the weather information automatically.");
                }
                enableDnBtn();
                w.pictureBox1.Visible = true;
            }
        }
    }
}
