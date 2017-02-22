using System.Drawing;

namespace QSP.UI.Forms
{
    public class WindDownloadStatus
    {
        public string Text { get; private set; }
        public Image Image { get; private set; }

        public WindDownloadStatus(string Text, Image Image)
        {
            this.Text = Text;
            this.Image = Image;
        }

        public static WindDownloadStatus FinishedDownload
        {
            get
            {
                return new WindDownloadStatus(
                    "Lastest wind ready", Properties.Resources.GreenLight);
            }
        }

        public static WindDownloadStatus Downloading
        {
            get
            {
                return new WindDownloadStatus(
                    "Downloading lastest wind ...", null);
            }
        }

        public static WindDownloadStatus FailedToDownload
        {
            get
            {
                return new WindDownloadStatus("Failed to download wind data",
                    Properties.Resources.RedLight);
            }
        }

        public static WindDownloadStatus WaitingManualDownload
        {
            get
            {
                return new WindDownloadStatus(
                    "Wind data not downloaded.",
                    Properties.Resources.YellowLight);
            }
        }

        public static WindDownloadStatus LoadingFromFile
        {
            get
            {
                return new WindDownloadStatus("Loading from file ...", null);
            }
        }
    }
}
