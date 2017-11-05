using QSP.UI.Models.MiscInfo;
using QSP.UI.Presenters.MiscInfo;
using QSP.UI.Util;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.Views.MiscInfo
{
    public partial class MetarViewer : UserControl, IMetarViewerView
    {
        private MetarViewerPresenter presenter;
        private Image processingImage;
        public MetarViewer()
        {
            InitializeComponent();
        }

        public void Init(MetarViewerPresenter presenter)
        {
            this.presenter = presenter;

            SetImages();
            lastUpdatedLbl.Text = "";
            downloadAllBtn.Click += (s, e) => UpdateAllMetarTaf();
            IcaoTxtBox.TextChanged += (s, e) => presenter.UpdateMetarTaf();
            metarTafRichTxtBox.ContentsResized += (s, e) =>
            {
                metarTafRichTxtBox.Height = e.NewRectangle.Height + 10;
            };
        }

        private void SetImages()
        {
            processingImage = ImageUtil.Resize(Properties.Resources.processing, statusPicBox.Size);
        }

        public string Icao => IcaoTxtBox.Text.Trim().ToUpper();

        public string LastUpdateTime
        {
            set { lastUpdatedLbl.Text = $"Last Updated : {value}"; }
        }

        public MetarViewStatus Status
        {
            set
            {
                switch (value)
                {
                    case MetarViewStatus.Processing:
                        statusPicBox.Image = processingImage;
                        break;

                    case MetarViewStatus.OK:
                        statusPicBox.SetImageHighQuality(Properties.Resources.GreenLight);
                        break;

                    case MetarViewStatus.Failed:
                        statusPicBox.SetImageHighQuality(Properties.Resources.RedLight);
                        break;

                    default:
                        throw new ArgumentException();
                }
            }
        }

        public string MetarText { set { metarTafRichTxtBox.Text = value; } }

        private void SetUpdateTime()
        {
            lastUpdatedLbl.Text = $"Last Updated : {DateTime.Now}";
        }

        private async Task UpdateAllMetarTaf()
        {
            downloadAllBtn.Enabled = false;
            await presenter.UpdateAllMetarTaf();
            downloadAllBtn.Enabled = true;
        }
    }
}
