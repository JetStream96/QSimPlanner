using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.Metar;
using QSP.UI.Util;

namespace QSP.UI.UserControls
{
    public partial class MetarViewer : UserControl
    {
        private Image processingImage;

        private Func<string> origGetter;
        private Func<string> destGetter;
        private Func<IEnumerable<string>> altnGetter;

        public MetarViewer()
        {
            InitializeComponent();
        }

        public void Init(
           Func<string> origGetter,
           Func<string> destGetter,
           Func<IEnumerable<string>> altnGetter)
        {
            this.origGetter = origGetter;
            this.destGetter = destGetter;
            this.altnGetter = altnGetter;

            SetImages();
            metarLastUpdatedLbl.Text = "";
            downloadAllBtn.Click += (s, e) => UpdateAllMetarTaf();
            IcaoTxtBox.TextChanged += (s, e) => DownloadMetarTaf();
            metarTafRichTxtBox.ContentsResized += (s, e) =>
            {
                metarTafRichTxtBox.Height = e.NewRectangle.Height + 10;
            };
        }

        private void SetImages()
        {
            processingImage = ImageUtil.Resize(Properties.Resources.processing, statusPicBox.Size);
        }

        private string Icao => IcaoTxtBox.Text.Trim().ToUpper();

        private async Task DownloadMetarTaf()
        {
            var icaoCode = Icao;
            statusPicBox.Image = processingImage;
            metarTafRichTxtBox.Text = "";
            var result = await Task.Factory.StartNew(() => MetarDownloader.GetMetarTaf(icaoCode));

            if (result == null)
            {
                statusPicBox.SetImageHighQuality(Properties.Resources.RedLight);
                metarLastUpdatedLbl.Text = "";
                return;
            }

            if (Icao == icaoCode)
            {
                metarTafRichTxtBox.Text = result;
                SetUpdateTime();
                statusPicBox.SetImageHighQuality(Properties.Resources.GreenLight);
            }
        }

        private void SetUpdateTime()
        {
            metarLastUpdatedLbl.Text = $"Last Updated : {DateTime.Now}";
        }

        private async Task UpdateAllMetarTaf()
        {
            downloadAllBtn.Enabled = false;
            metarLastUpdatedLbl.Text = "";

            var allTasks = new[] { origGetter(), destGetter() }.Concat(altnGetter())
                .Select(i => Task.Factory.StartNew(() => MetarDownloader.TryGetMetarTaf(i)));
            var result = await Task.WhenAll(allTasks);

            var lineSep = new string('-', 80);
            metarTafRichTxtBox.Text = string.Join($"\n{lineSep}\n\n", result);
            SetUpdateTime();
            downloadAllBtn.Enabled = true;
        }
    }
}
