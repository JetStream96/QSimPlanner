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
            disableViewBtn();
        }

        public void Subscribe()
        {
            wxControl.GetMetarBtn.Click += getMetarClicked;
            wxControl.ViewMetarBtn.Click += viewMetarClicked;
            airportControl.airportTxtBox.TextChanged += (sender, e) =>
            {
                wxControl.picBox.Visible = false;
                disableViewBtn();
            };
        }

        private void enableViewBtn()
        {
            wxControl.ViewMetarBtn.Enabled = true;
            wxControl.ViewMetarBtn.BackColor = SystemColors.MenuHighlight;
        }

        private void disableViewBtn()
        {
            wxControl.ViewMetarBtn.Enabled = false;
            wxControl.ViewMetarBtn.BackColor = Color.FromArgb(224,224,224);
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
            frm.ShowDialog();
        }

        // Get metar functions.
        private async void getMetarClicked(object sender, EventArgs e)
        {
            disableDnBtn();
            var w = wxControl;
            w.picBox.Visible = false;
            w.picBox.Image = Properties.Resources.deleteIconLarge;

            string icao = airportControl.Icao;
            metar = null;

            bool metarAcquired =
                 await Task.Run(
                     () => MetarDownloader.TryGetMetar(icao, out metar));

            if (metarAcquired)
            {
                if (WeatherAutoFiller.Fill(
                    metar,
                    w.windDirTxtBox,
                    w.windSpdTxtBox,
                    w.oatTxtBox,
                    w.tempUnitComboBox,
                    w.pressTxtBox,
                    w.pressUnitComboBox,
                    w.surfCondComboBox) == false)
                {
                    MessageBox.Show(
                       @"Metar has been downloaded but the weather " +
                       "information cannot be filled automatically.",
                       "",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                }
                else
                {
                    w.picBox.Image = Properties.Resources.checkIconLarge;
                }

                enableViewBtn();
            }

            w.picBox.Visible = true;
            enableDnBtn();
        }
    }
}
