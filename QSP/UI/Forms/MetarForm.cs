using QSP.AviationTools;
using QSP.Metar;
using System;
using System.Net;
using static QSP.UI.Utilities;

namespace QSP
{
    public partial class MetarForm
    {
        //e.g. "Takeoff"
        //TODO:
        public string FromFormName = "";

        public string Metar { get; private set; }
        public string Taf { get; private set; }

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            //result_box.Text = "Downloading METAR/TAF..."
            downloadMetarTafAndShow();
        }

        private void downloadMetarTafAndShow()
        {
            if (getTafCheckBox.Checked)
            {
                resultRichTxtBox.Text = MetarDownloader.TryGetMetarTaf(icaoTxtBox.Text);
            }
            else
            {
                resultRichTxtBox.Text = MetarDownloader.TryGetMetar(icaoTxtBox.Text);
            }            
        }

        private void MetarForm_Load(object sender, EventArgs e)
        {
            PicBox.Hide();
            getMetarCheckBox.Checked = true;
            getTafCheckBox.Checked = false;
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
                        if (!char.IsDigit(metar[i + j]))
                        {
                            meetCondition = false;
                            break;
                        }
                    }

                    if (meetCondition)
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
        
        private void sendBtn_Click(object sender, EventArgs e)
        {
            var frm = MainFormInstance();

            if (string.IsNullOrEmpty(Metar))
            {
                downloadMetarTafAndShow();
            }

            string str_wind = WindInfo(Metar);
            string str_temp = TempInfo(Metar);
            string str_press = PressInfo(Metar);
            string winddir = null;
            string windspd = null;
            string oat = null;
            string altimeter = null;
            string hPa_inHg = null;
            //string usr_message;

            if (str_wind == "NA" || str_temp == "NA" || str_press == "NA")
            {
                //usr_message = "Failed to send weather.";
                PicBox.Image = Properties.Resources.deleteIconLarge;
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
                windspd = Convert.ToString(Math.Round(Convert.ToDouble(str_wind.Substring(3, 2)) / Constants.KT_MPS));
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
            //else if (FromFormName == "LDG")
            //{
            //    frm.windspd_LDG.Text = windspd;
            //    frm.winddir_LDG.Text = winddir;
            //    frm.temp_c_f_LDG.Text = "°C";
            //    frm.oat_LDG.Text = oat;
            //    frm.hpa_inhg_LDG.Text = hPa_inHg;
            //    frm.altimeter_LDG.Text = altimeter;
            //    //usr_message = "Weather successfully sent for landing calculation.";
            //}

            //complete message
            PicBox.Image = Properties.Resources.checkIconLarge;
            PicBox.Show();

        }

        public MetarForm()
        {
            Load += MetarForm_Load;
            InitializeComponent();
        }

    }
}
