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

        public async void UpdateAll()
        {
            var orig = OrigTask();
            var dest = DestTask();
            var altn = AltnTask().ToArray();

            int taskCount = altn.Length + 2;
            var allTasks = new Task<string>[taskCount];
            allTasks[0] = orig;
            allTasks[1] = dest;

            for (int i = 2; i < allTasks.Length; i++)
            {
                allTasks[i] = altn[i - 2];
            }

            var result = await Task.WhenAll(allTasks);

            origTxt = result[0];
            destTxt = result[1];
            altnTxt = result.Skip(2);
        }

        public async void UpdateOrig(string newOrig)
        {
            if (newOrig != orig)
            {
                ShowUpdating();

                orig = newOrig;
                origTxt = await OrigTask();

                UpdateDisplay();
            }
        }

        private Task<string> OrigTask()
        {
            return Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetarTaf(orig));
        }

        public async void UpdateDest(string newDest)
        {
            if (newDest != dest)
            {
                ShowUpdating();

                dest = newDest;
                destTxt = await DestTask();

                UpdateDisplay();
            }
        }

        private Task<string> DestTask()
        {
            return Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetarTaf(dest));
        }

        public async void UpdateAltn(IEnumerable<string> newAltn)
        {
            if (newAltn.SequenceEqual(altn) == false)
            {
                ShowUpdating();

                altn = newAltn;
                var tasks = AltnTask();
                altnTxt= await Task.WhenAll(tasks);

                UpdateDisplay();
            }
        }

        private IEnumerable<Task<string>> AltnTask()
        {
            return altn.Select(
                i => Task.Factory.StartNew(
                    () => MetarDownloader.TryGetMetarTaf(i)));
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
