using System;
using System.Drawing;
using System.Threading.Tasks;
using QSP.Metar;
using QSP.UI.UserControls.TakeoffLanding.Common.AirportInfo;
using QSP.UI.Util;
using QSP.UI.Views;

namespace QSP.UI.UserControls.TakeoffLanding.Common
{
    public class AutoWeatherSetter
    {
        private readonly WeatherInfoControl wxControl;
        private readonly AirportInfoControl airportControl;
        private readonly MetarCache metarCache;
        private string metar;

        public AutoWeatherSetter(
            WeatherInfoControl wxControl,
            AirportInfoControl airportControl,
            MetarCache metarCache)
        {
            this.wxControl = wxControl;
            this.airportControl = airportControl;
            this.metarCache = metarCache;
            DisableViewBtn();
        }

        public void Subscribe()
        {
            wxControl.GetMetarBtn.Click += (s, e) => GetMetarClicked(s, e);
            wxControl.ViewMetarBtn.Click += ViewMetarClicked;
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

        // Gets metar from MetarCache if it exists in the cache.
        // Otherwise, download the metar from internet.
        // this.metar is set if successful.
        // icao should be all capital letters.
        private async Task<bool> GetMetar(string icao)
        {
            if (metarCache.Contains(icao))
            {
                this.metar = metarCache.GetItem(icao).Metar;
                return true;
            }

            return await Task.Run(
                () => MetarDownloader.TryGetMetar(icao, out metar));
        }

        // Get metar functions.
        public async Task GetMetarClicked(object sender, EventArgs e)
        {
            DisableDnBtn();
            var w = wxControl;
            w.picBox.Visible = false;
            w.picBox.Image = Properties.Resources.RedLight;

            string icao = airportControl.Icao;

            bool metarAcquired = await GetMetar(icao);

            // Because GetMetar method is asynchronous, it is neccessary to
            // check whether the currently entered ICAO code is still the 
            // same as before. If the ICAO changed, we do not need to update.
            if (metarAcquired && icao == airportControl.Icao)
            {
                if (!WeatherAutoFiller.Fill(
                    metar,
                    w.windDirTxtBox,
                    w.windSpdTxtBox,
                    w.oatTxtBox,
                    w.tempUnitComboBox,
                    w.pressTxtBox,
                    w.pressUnitComboBox,
                    w.surfCondComboBox))
                {
                    wxControl.ShowError(
                        @"Metar has been downloaded but the weather " +
                        "information cannot be filled automatically.",
                        "");
                }
                else
                {
                    w.picBox.Image = Properties.Resources.GreenLight;
                }

                EnableViewBtn();
            }

            w.picBox.Visible = true;
            EnableDnBtn();
        }
    }
}
