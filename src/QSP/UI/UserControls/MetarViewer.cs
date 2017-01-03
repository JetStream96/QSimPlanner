using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.WindAloft;

namespace QSP.UI.UserControls
{
    public partial class MetarViewer : UserControl
    {
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
            
            metarLastUpdatedLbl.Text = "";
            downloadAllBtn.Click += (s, e) => UpdateAllMetarTaf();
            button1.Click += (s, e) => downloadMetarBtnClick();
        }

        private async Task downloadMetarBtnClick()
        {
            var icao = metarToFindTxtBox.Text.Trim().ToUpper();
            matarTafRichTxtBox.Text = await Task.Factory.StartNew(() =>
                MetarDownloader.TryGetMetarTaf(icao));
            SetUpdateTime();
        }

        private void SetUpdateTime()
        {
            metarLastUpdatedLbl.Text = $"Last Updated : {DateTime.Now}";
        }

        private async Task UpdateAllMetarTaf()
        {
            downloadAllBtn.Enabled = false;

            var allTasks = new[] { OrigTask(), DestTask() }.Concat(AltnTask());
            var result = await Task.WhenAll(allTasks);

            matarTafRichTxtBox.Text = string.Join("\n\n", result);
            SetUpdateTime();
            downloadAllBtn.Enabled = true;
        }

        private Task<string> OrigTask()
        {
            return Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetarTaf(origGetter()));
        }

        private Task<string> DestTask()
        {
            return Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetarTaf(destGetter()));
        }

        private IEnumerable<Task<string>> AltnTask()
        {
            return altnGetter().Select(
                i => Task.Factory.StartNew(
                    () => MetarDownloader.TryGetMetarTaf(i)));
        }
    }
}
