using QSP.Metar;
using QSP.Utilities;
using System;
using static QSP.UI.FormInstanceGetter;

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
                    LoggerInstance.WriteToLog(ex);
                }
            }

            try
            {
                Metar = MetarDownloader.GetMetar(icaoTxtBox.Text);
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
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

        private void sendBtnClick(object sender, EventArgs e)
        {
            var frm = MainFormInstance();

            if (string.IsNullOrEmpty(Metar))
            {
                downloadMetarTafAndShow();
            }

            var wind = ParaExtractor.GetWind(Metar);
            int? temp = ParaExtractor.GetTemp(Metar);
            var press = ParaExtractor.GetPressure(Metar);
            
            if (wind == null ||
                temp == null ||
                press == null)
            {
                PicBox.Image = Properties.Resources.deleteIconLarge;
                PicBox.Show();
                return;
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
