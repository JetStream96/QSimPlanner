using QSP.Metar;
using System;
using static QSP.UI.FormInstanceGetter;
using QSP.Utilities;

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
            Metar = null;
            Taf = null;

            if (getTafCheckBox.Checked)
            {
                try
                {
                    Taf = MetarDownloader.GetTaf(icaoTxtBox.Text);
                }
                catch (Exception ex)
                {
                    ErrorLogger.WriteToLog(ex);
                }
            }

            try
            {
                Metar = MetarDownloader.GetMetar(icaoTxtBox.Text);
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteToLog(ex);
            }

            resultRichTxtBox.Text =
                Metar ?? "Downloading Metar failed." +
                "\n\n" +
                Taf ?? "Downloading TAF failed.";
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
            var press = ParaExtractor.GetPressure(Metar);

            if (wind == null ||
                temp == int.MinValue ||
                press == null)
            {
                PicBox.Image = Properties.Resources.deleteIconLarge;
                PicBox.Show();
                return;
            }

            //if (FromFormName == "Takeoff")
            //{
            //    frm.windspd.Text = ((int)Math.Round(wind.Speed)).ToString();
            //    frm.winddir.Text = (((int)wind.Direction - 1).Mod(360) + 1).ToString().PadLeft(3, '0');
            //    frm.temp_c_f.Text = "°C";
            //    frm.OAT.Text = temp.ToString();
            //    frm.hpa_inHg.SelectedIndex = (int)press.PressUnit;
            //    frm.altimeter.Text =
            //        press.PressUnit == PressureUnit.inHg ?
            //        Math.Round(press.Value, 2).ToString() :
            //        ((int)press.Value).ToString();
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
