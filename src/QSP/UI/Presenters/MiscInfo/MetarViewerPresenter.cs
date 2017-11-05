using QSP.Metar;
using QSP.UI.Models.MiscInfo;
using QSP.UI.Views.MiscInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSP.UI.Presenters.MiscInfo
{
    public class MetarViewerPresenter
    {
        private IMetarViewerView view;
        private Func<string> origGetter;
        private Func<string> destGetter;
        private Func<IEnumerable<string>> altnGetter;

        public MetarViewerPresenter(IMetarViewerView view,
            Func<string> origGetter,
            Func<string> destGetter,
            Func<IEnumerable<string>> altnGetter)
        {
            this.view = view;
            this.origGetter = origGetter;
            this.destGetter = destGetter;
            this.altnGetter = altnGetter;
        }

        public async Task UpdateMetarTaf()
        {
            var icaoCode = view.Icao;
            view.Status = MetarViewStatus.Processing;
            view.MetarText = "";
            var result = await Task.Factory.StartNew(() => MetarDownloader.GetMetarTaf(icaoCode));

            if (result == null)
            {
                view.Status = MetarViewStatus.Failed;
                view.LastUpdateTime = "";
                return;
            }

            if (view.Icao == icaoCode)
            {
                view.MetarText = result;
                view.LastUpdateTime = DateTime.Now.ToString();
                view.Status = MetarViewStatus.OK;
            }
        }

        public async Task UpdateAllMetarTaf()
        {
            view.MetarText = "";
            view.LastUpdateTime = "";

            var allTasks = new[] { origGetter(), destGetter() }.Concat(altnGetter())
                .Select(i => Task.Factory.StartNew(() => MetarDownloader.TryGetMetarTaf(i)));
            var result = await Task.WhenAll(allTasks);

            var lineSep = new string('-', 80);
            view.MetarText = string.Join($"\n{lineSep}\n\n", result);
            view.LastUpdateTime = DateTime.Now.ToString();
        }
    }
}

