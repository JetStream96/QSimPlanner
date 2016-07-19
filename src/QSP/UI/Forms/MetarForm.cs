using QSP.Metar;
using QSP.Utilities;
using System;

namespace QSP.UI.Forms
{
    public partial class MetarForm
    {
        public string Metar { get; private set; }
        public string Taf { get; private set; }

        public MetarForm()
        {
            InitializeComponent();
        }

        private void DownloadMetarTafAndShow()
        {
            Metar = "";
            Taf = "";

            try
            {
                Taf = MetarDownloader.GetTaf(icaoTxtBox.Text);
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
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
                Metar ?? "Downloading Metar failed.\n\n" +
                Taf ?? "Downloading TAF failed.";
        }
    }
}
