using QSP.LibraryExtension;
using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.UserControls
{
    public partial class MiscInfoControl : UserControl
    {
        private AirportManager airportList;
        private Locator<WindTableCollection> windTableLocator;
        private Func<string> origGetter;
        private Func<string> destGetter;
        private Func<IEnumerable<string>> altnGetter;

        public MiscInfoControl()
        {
            InitializeComponent();
        }

        public void Init(
            AirportManager airportList,
            Locator<WindTableCollection> windTableLocator,
            bool enableBrowser,
            Func<string> origGetter,
            Func<string> destGetter,
            Func<IEnumerable<string>> altnGetter)
        {
            this.airportList = airportList;
            airportMapControl.Initialize(airportList);
            this.windTableLocator = windTableLocator;
            airportMapControl.BrowserEnabled = enableBrowser;
            metarLastUpdatedLbl.Text = "";
            destIcaoLbl.Text = "";
            desForcastLastUpdatedLbl.Text = "";
            this.origGetter = origGetter;
            this.destGetter = destGetter;
            this.altnGetter = altnGetter;
        }

        public void SetOrig(string icao)
        {
            airportMapControl.Orig = icao;
        }

        public void SetDest(string icao)
        {
            airportMapControl.Dest = icao;
        }
        
        public void SetAltn(IEnumerable<string> icao)
        {
            airportMapControl.Altn = icao;
        }

        private void downloadMetarBtnClick(object sender, EventArgs e)
        {
            var icao = metarToFindTxtBox.Text.Trim().ToUpper();
            RichTextBox1.Text = MetarDownloader.TryGetMetarTaf(icao);
        }
        
        private async void UpdateAllMetarTaf(object sender, EventArgs e)
        {
            var orig = OrigTask();
            var dest = DestTask();
            var altn = AltnTask().ToList();

            int taskCount = altn.Count + 2;
            var allTasks = new Task<string>[taskCount];
            allTasks[0] = orig;
            allTasks[1] = dest;

            for (int i = 2; i < allTasks.Length; i++)
            {
                allTasks[i] = altn[i - 2];
            }

            var result = await Task.WhenAll(allTasks);

            metarRichTxtBox.Text = string.Join("\n\n", result);
            metarLastUpdatedLbl.Text = $"Last Updated : {DateTime.Now}";
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

        private async void updateDesForcast(object sender, EventArgs e)
        {
            var dest = destGetter();

            try
            {
                desForcastRichTxtBox.Text = "\n\n\n           Refreshing ...";
                destIcaoLbl.Text = "Destination : " + dest;

                desForcastRichTxtBox.Text =
                    await Task.Factory.StartNew(() =>
                    GenDesForcastString(dest));

                desForcastLastUpdatedLbl.Text = 
                    $"Last Updated : {DateTime.Now}";
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                desForcastRichTxtBox.Text =
                    "\n\n\n       Unable to get descend forcast for " + dest;
            }
        }
        
        private string GenDesForcastString(string icao)
        {
            var latlon = airportList.AirportLatlon(icao);
            int[] FLs = { 60, 90, 120, 180, 240, 300, 340, 390, 440, 490 };
            var forcastGen = new DescendForcastGenerator(
                windTableLocator.Instance, latlon.Lat, latlon.Lon, FLs);

            Wind[] w = forcastGen.Generate();
            var result = new StringBuilder();

            for (int i = 0; i < FLs.Length; i++)
            {
                var flightLevel = FLs[i].ToString().PadLeft(3, '0');
                var direction = w[i].DirectionString();
                int speed = RoundToInt(w[i].Speed);

                result.AppendLine(
                    $"        FL{flightLevel}   {direction}/{speed}");
            }

            return result.ToString();
        }
    }
}
