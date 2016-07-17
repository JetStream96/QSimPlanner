using QSP.Metar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QSP.UI.UserControls
{
    public class MetarMonitor
    {
        private Control display;
        private string orig;
        private string dest;
        private IEnumerable<string> altn;

        private string origTxt;
        private string destTxt;
        private IEnumerable<string> altnTxt;
        
        public MetarMonitor(Control display)
        {
            this.display = display;
            origTxt = "";
            destTxt = "";
            altnTxt = new string[0];
        }
        public async Task UpdateOrig(string newOrig)
        {
            if (newOrig != orig)
            {
                ShowUpdating();

                orig = newOrig;
                origTxt = await Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetarTaf(orig));

                UpdateDisplay();
            }
        }

        public async Task UpdateDest(string newDest)
        {
            if (newDest != dest)
            {
                ShowUpdating();

                dest = newDest;
                destTxt = await Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetarTaf(dest));

                UpdateDisplay();
            }
        }

        public async Task UpdateAltn(IEnumerable<string> newAltn)
        {
            if (newAltn.SequenceEqual(altn) == false)
            {
                ShowUpdating();

                altn = newAltn;
                var tasks = altn.Select(
                    i => Task.Factory.StartNew(
                        () => MetarDownloader.TryGetMetarTaf(i)));

                await Task.WhenAll(tasks);

                UpdateDisplay();
            }
        }

        private void ShowUpdating()
        {
            display.Text = "Updating ...";
        }

        private void UpdateDisplay()
        {
            var altnJoined = string.Join("\n\n", altnTxt);
            display.Text = string.Join("\n\n", origTxt, destTxt, altnJoined);
        }

    }
}
