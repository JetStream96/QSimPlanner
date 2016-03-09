using QSP.AviationTools;
using QSP.Metar;
using System;
using static QSP.UI.Utilities;
using QSP.MathTools;

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

            var wind = ParaExtractor.GetWind(Metar);
            int temp = ParaExtractor.GetTemp(Metar);
            string press = ParaExtractor.PressInfo(Metar);
            string altimeter = null;
            bool isHpa;

            if (wind == null || temp == int.MinValue || press == "NA")
            {
                //usr_message = "Failed to send weather.";
                PicBox.Image = Properties.Resources.deleteIconLarge;
                PicBox.Show();
                return;
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
                frm.windspd.Text = ((int)Math.Round(wind.Speed)).ToString();
                frm.winddir.Text = (((int)wind.Direction - 1).Mod(360) + 1).ToString();
                frm.temp_c_f.Text = "°C";
                frm.OAT.Text = temp.ToString();
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
