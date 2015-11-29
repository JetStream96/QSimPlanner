using System;
using System.Net;
using QSP.AviationTools;
using static QSP.UI.Utilities;

namespace QSP
{

    public partial class METAR
    {

        //e.g. "Takeoff"
        public string FromFormName = "";

        string result_metar;

        string result_taf;
        private void download_btn_Click(object sender, EventArgs e)
        {
            //result_box.Text = "Downloading METAR/TAF..."
            DownloadMetarTafAndShow();
        }

        public void DownloadMetarTafAndShow()
        {
            result_metar = "";
            result_taf = "";
            string icao = icao_code.Text.Replace(" ", "");
            try
            {
                using (var client = new WebClient())
                {
                    result_metar = client.DownloadString("http://weather.noaa.gov/pub/data/observations/metar/stations/" + icao + ".TXT");
                    if (TAF_download.Checked )
                    {
                        result_taf = client.DownloadString("http://weather.noaa.gov/pub/data/forecasts/taf/stations/" + icao + ".TXT");
                    }
                }
            }
            catch
            {
                result_box.Text = "Downloading Metar/TAF failed.";
                return;
            }

            result_box.Text = result_metar + Environment.NewLine + Environment.NewLine + result_taf;
        }

        public static  string GetMetarTAF(string icao)
        {

            string metar = "";
            string taf = "";
            icao = icao.Replace(" ", "");
            icao = icao.ToUpper();

            using (var client = new WebClient())
            {

                try
                {
                    metar = client.DownloadString("http://weather.noaa.gov/pub/data/observations/metar/stations/" + icao + ".TXT");
                }
                catch
                {
                    metar = "Downloading Metar failed.";
                }

                try
                {
                    taf = client.DownloadString("http://weather.noaa.gov/pub/data/forecasts/taf/stations/" + icao + ".TXT");
                }
                catch
                {
                    taf = "Downloading TAF failed.";
                }

            }

            return metar + Environment.NewLine + taf + Environment.NewLine;

        }

        public static string GetMetar(string icao)
        {
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString("http://weather.noaa.gov/pub/data/observations/metar/stations/" + icao.ToUpper() + ".TXT");
                }
            }
            catch
            {
                return "Downloading Metar failed.";
            }
        }

        private void METAR_Load(object sender, EventArgs e)
        {
            PicBox.Hide();
            metar_download.Checked = true;
            TAF_download.Checked = false;
        }

        public string WindInfo(string metar)
        {
            //will return "02013KT", or "14007MPS" if found 
            //return "NA" if nothing is found
            bool meetCondition = false;

            try
            {
                if (metar.IndexOf("VRB") != -1)
                {
                    return "36000KT";
                }

                for (int i = 0; i <= metar.Length - 10; i++)
                {
                    meetCondition = true;
                    for (int j = 0; j <= 4; j++)
                    {
                        if (!char.IsDigit(metar[i + j]) )
                        {
                            meetCondition = false;
                            break; 
                        }
                    }

                    if (meetCondition )
                    {
                        if (metar[i + 5] == 'G' && char.IsDigit(metar[i + 6]) && char.IsDigit(metar[i + 7]))
                        {
                            if (metar[i + 8] == 'K' && metar[i + 9] == 'T')
                            {
                                return metar.Substring(i, 5) + metar.Substring(i + 8, 2);
                            }
                            else if (metar[i + 8] == 'M' && metar[i + 9] == 'P' && metar[i + 10] == 'S')
                            {
                                return metar.Substring(i, 5) + metar.Substring(i + 8, 3);
                            }
                        }
                        else
                        {
                            if (metar[i + 5] == 'K' && metar[i + 6] == 'T')
                            {
                                return metar.Substring(i, 7);
                            }
                            else if (metar[i + 5] == 'M' && metar[i + 6] == 'P' & metar[i + 7] == 'S')
                            {
                                return metar.Substring(i, 8);
                            }
                        }
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }

        public string TempInfo(string metar)
        {
            //input can be 12/09, 01/M01, M04/M06,etc
            //return "12/09", "-4/-6", or NA
            try
            {
                for (int i = 0; i <= metar.Length - 8; i++)
                {

                    if (metar[i] == ' ' && char.IsDigit(metar[i + 1]) && char.IsDigit(metar[i + 2]) &&
                        metar[i + 3] == '/' && char.IsDigit(metar[i + 4]) && char.IsDigit(metar[i + 5]))
                    {
                        return metar.Substring(i + 1, 5);

                    }
                    else if (metar[i] == ' ' && char.IsDigit(metar[i + 1]) && char.IsDigit(metar[i + 2]) && metar[i + 3] == '/' &&
                        metar[i + 4] == 'M' && char.IsDigit(metar[i + 5]) && char.IsDigit(metar[i + 6]))
                    {
                        return metar.Substring(i + 1, 6).Replace('M', '-');

                    }
                    else if (metar[i] == ' ' && metar[i + 1] == 'M' && char.IsDigit(metar[i + 2]) == true &&
                      char.IsDigit(metar[i + 3]) && metar[i + 4] == '/' && metar[i + 5] == 'M' &&
                     char.IsDigit(metar[i + 6]) && char.IsDigit(metar[i + 7]))
                    {
                        return metar.Substring(i + 1, 7).Replace('M', '-');
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }

        public string PressInfo(string metar)
        {
            //will return, e.g. Q1013, A3000
            //return "NA" if nothing is found
            bool meetCondition = false;
            try
            {
                for (int i = 0; i <= metar.Length - 5; i++)
                {
                    meetCondition = true;
                    if (metar[i] == 'Q' || metar[i] == 'A')
                    {
                        for (int j = 1; j <= 4; j++)
                        {
                            if (!char.IsDigit(metar[i + j]))
                            {
                                meetCondition = false;
                                break;
                            }
                        }

                        if (meetCondition == true)
                        {
                            return metar.Substring(i, 5);
                        }
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }

        public bool IsNumeric(char c)
        {
            if (c >= 48 & c <= 57)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void send_btn_Click(object sender, EventArgs e)
        {
            var frm = MainFormInstance();

            if (string.IsNullOrEmpty( result_metar))
            {
                DownloadMetarTafAndShow();
            }

            string str_wind = WindInfo(result_metar);
            string str_temp = TempInfo(result_metar);
            string str_press = PressInfo(result_metar);
            string winddir = null;
            string windspd = null;
            string oat = null;
            string altimeter = null;
            string hPa_inHg = null;
            //string usr_message;

            if (str_wind == "NA" || str_temp == "NA" || str_press == "NA")
            {
                //usr_message = "Failed to send weather.";
                PicBox.Image = Properties .Resources.deleteIconLarge;
                PicBox.Show();
                return;
            }

            oat = str_temp.Substring(0, str_temp.IndexOf("/"));
            winddir = str_wind.Substring(0, 3);

            if (str_wind[str_wind.Length - 1] == 'T')
            {
                windspd = str_wind.Substring(3, 2);
            }
            else
            {
                windspd = Convert.ToString(Math.Round(Convert.ToDouble(str_wind.Substring(3, 2)) / AviationConstants.KT_MPS));
            }

            if (str_press[0] == 'Q')
            {
                hPa_inHg = "hPa";
                altimeter = str_press.Substring(1, 4);
            }
            else
            {
                hPa_inHg = "inHg";
                altimeter = str_press.Substring(1, 2) + "." + str_press.Substring(3);
            }

            if (FromFormName == "Takeoff")
            {
                frm.windspd.Text = windspd;
                frm.winddir.Text = winddir;
                frm.temp_c_f.Text = "°C";
                frm.OAT.Text = oat;
                frm.hpa_inHg.Text = hPa_inHg;
                frm.altimeter.Text = altimeter;
                //usr_message = "Weather successfully sent for takeoff calculation.";
            }
            else if (FromFormName == "LDG")
            {
                frm.windspd_LDG.Text = windspd;
                frm.winddir_LDG.Text = winddir;
                frm.temp_c_f_LDG.Text = "°C";
                frm.oat_LDG.Text = oat;
                frm.hpa_inhg_LDG.Text = hPa_inHg;
                frm.altimeter_LDG.Text = altimeter;
                //usr_message = "Weather successfully sent for landing calculation.";
            }

            //complete message
            PicBox.Image =Properties .Resources.checkIconLarge;
            PicBox.Show();

        }

        public METAR()
        {
            Load += METAR_Load;
            InitializeComponent();
        }

    }
}
