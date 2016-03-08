using QSP.AviationTools;
using QSP.Metar;
using System;
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

            // TODO: Save this state.
            getTafCheckBox.Checked = false;
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            var frm = MainFormInstance();

            if (string.IsNullOrEmpty(Metar))
            {
                downloadMetarTafAndShow();
            }

            string wind = ParaExtractor.WindInfo(Metar);
            string temp = ParaExtractor.TempInfo(Metar);
            string press = ParaExtractor.PressInfo(Metar);
            string winddir = null;
            string windspd = null;
            string oat = null;
            string altimeter = null;
            bool isHpa;
            //string usr_message;

            if (wind == "NA" || temp == "NA" || press == "NA")
            {
                //usr_message = "Failed to send weather.";
                PicBox.Image = Properties.Resources.deleteIconLarge;
                PicBox.Show();
                return;
            }

            oat = temp.Substring(0, temp.IndexOf("/"));
            winddir = wind.Substring(0, 3);

            if (wind[wind.Length - 1] == 'T')
            {
                windspd = wind.Substring(3, 2);
            }
            else
            {
                windspd = Math.Round(
                    Convert.ToDouble(wind.Substring(3, 2)) / Constants.KT_MPS)
                    .ToString();
            }

            if (press[0] == 'Q')
            {
                isHpa = true;
                altimeter = press.Substring(1, 4);
            }
            else
            {
                isHpa = false;
                altimeter = press.Substring(1, 2) + "." + press.Substring(3);
            }

            if (FromFormName == "Takeoff")
            {
                frm.windspd.Text = windspd;
                frm.winddir.Text = winddir;
                frm.temp_c_f.Text = "°C";
                frm.OAT.Text = oat;
                frm.hpa_inHg.SelectedIndex = isHpa ? 0 : 1;
                frm.altimeter.Text = altimeter;
            }

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
