using QSP.Metar;
using QSP.UI.Forms;
using QSP.UI.ToLdgModule.Common.AirportInfo;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.MsgBox;
using QSP.UI.Utilities;

namespace QSP.UI.ToLdgModule.Common
{
    public class AutoWeatherSetter
    {
        private WeatherInfoControl wxControl;
        private AirportInfoControl airportControl;
        private string metar;

        public AutoWeatherSetter(WeatherInfoControl wxControl, AirportInfoControl airportControl)
        {
            this.wxControl = wxControl;
            this.airportControl = airportControl;
            DisableViewBtn();
        }

        public void Subscribe()
        {
            wxControl.GetMetarBtn.Click += (s, e) => GetMetarClicked(s, e);
            wxControl.ViewMetarBtn.Click += ViewMetarClicked;
            airportControl.airportTxtBox.TextChanged += (sender, e) =>
            {
                wxControl.picBox.Visible = false;
                DisableViewBtn();
            };
        }

        private void EnableViewBtn()
        {
            wxControl.ViewMetarBtn.Enabled = true;
            wxControl.ViewMetarBtn.BackColor = SystemColors.MenuHighlight;
        }

        private void DisableViewBtn()
        {
            wxControl.ViewMetarBtn.Enabled = false;
            wxControl.ViewMetarBtn.BackColor = Color.FromArgb(224, 224, 224);
        }

        private void EnableDnBtn()
        {
            var btn = wxControl.GetMetarBtn;

            btn.Enabled = true;
            btn.BackColor = Color.DarkSlateGray;
            btn.Text = "Import METAR";
        }

        private void DisableDnBtn()
        {
            var btn = wxControl.GetMetarBtn;

            btn.Enabled = false;
            btn.BackColor = Color.FromArgb(224, 224, 224);
            btn.Text = "Downloading ...";
        }

        private void ViewMetarClicked(object sender, EventArgs e)
        {
            var frm = new MetarForm();
            frm.icaoTxtBox.Text = airportControl.Icao;
            frm.icaoTxtBox.Enabled = false;
            frm.resultRichTxtBox.Text = metar ?? "";
            frm.ShowDialog();
        }

        // Get metar functions.
        private async Task GetMetarClicked(object sender, EventArgs e)
        {
            DisableDnBtn();
            var w = wxControl;
            w.picBox.Visible = false;
            w.picBox.Image = Properties.Resources.errorIcon;

            string icao = airportControl.Icao;
            metar = null;

            bool metarAcquired = await Task.Run(
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
                    MsgBoxHelper.ShowError(
                        @"Metar has been downloaded but the weather " +
                        "information cannot be filled automatically.",
                        "");
                }
                else
                {
                    w.picBox.Image = Properties.Resources.okIcon;
                }

                EnableViewBtn();
            }

            w.picBox.Visible = true;
            EnableDnBtn();
        }
    }
}
