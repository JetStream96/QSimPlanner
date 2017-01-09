using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.UserControls
{
    public partial class MiscInfoControl : UserControl
    {
        private Locator<IWindTableCollection> windTableLocator;
        private Func<string> origGetter;
        private Func<string> destGetter;
        private Func<IEnumerable<string>> altnGetter;

        private AirportManager _airportList;
        public AirportManager AirportList
        {
            get { return _airportList; }

            set
            {
                _airportList = value;
                airportMapControl.Airports = _airportList;
            }
        }

        public MiscInfoControl()
        {
            InitializeComponent();
        }

        public void Init(
            AirportManager airportList,
            Locator<IWindTableCollection> windTableLocator,
            bool enableBrowser,
            Func<string> origGetter,
            Func<string> destGetter,
            Func<IEnumerable<string>> altnGetter)
        {
            this._airportList = airportList;
            airportMapControl.Init(airportList);
            this.windTableLocator = windTableLocator;
            airportMapControl.BrowserEnabled = enableBrowser;
            destIcaoLbl.Text = "";
            desForcastLastUpdatedLbl.Text = "";
            this.origGetter = origGetter;
            this.destGetter = destGetter;
            this.altnGetter = altnGetter;

            EnableTabControlAutosize();
            updateDesForcastBtn.Click += (s, e) => UpdateDesForcast();
            metarViewer.Init(origGetter, destGetter, altnGetter);
            TabControl1.SelectedIndex = 0;
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

        private async Task UpdateDesForcast()
        {
            if (windTableLocator.Instance is DefaultWindTableCollection)
            {
                desForcastRichTxtBox.Text = "\n\n\n       Wind aloft has not been downloaded.";
                return;
            }

            var dest = destGetter();

            try
            {
                desForcastRichTxtBox.Text = "\n\n\n           Refreshing ...";
                destIcaoLbl.Text = "Destination : " + dest;

                desForcastRichTxtBox.Text =
                    await Task.Factory.StartNew(() => GenDesForcastString(dest));

                desForcastLastUpdatedLbl.Text = $"Last Updated : {DateTime.Now}";
            }
            catch (Exception ex)
            {
                Log(ex);
                desForcastRichTxtBox.Text =
                    "\n\n\n       Unable to get descent forcast for " + dest;
            }
        }

        private string GenDesForcastString(string icao)
        {
            var airport = AirportList[icao];
            int[] FLs = { 60, 90, 120, 180, 240, 300, 340, 390, 440, 490 };
            var forcastGen = new DescendForcastGenerator(
                windTableLocator.Instance, airport.Lat, airport.Lon, FLs);

            Wind[] w = forcastGen.Generate();
            var result = new StringBuilder("\n");

            for (int i = 0; i < FLs.Length; i++)
            {
                var flightLevel = FLs[i].ToString().PadLeft(3, '0');
                var direction = w[i].DirectionString();
                int speed = RoundToInt(w[i].Speed);

                result.AppendLine($"        FL{flightLevel}   {direction}/{speed}");
            }

            return result.ToString();
        }

        private void EnableTabControlAutosize()
        {
            Control[] controls = { airportMapControl, metarViewer };
            EventHandler adjustHeight = (s, e) => TabControl1.Height = GetHeight();

            TabControl1.SelectedIndexChanged += adjustHeight;
            controls.ForEach(c => c.SizeChanged += adjustHeight);
        }

        private int GetHeight()
        {
            Control[] controls = { airportMapControl, metarViewer };
            const int minHeight = 800;
            int index = TabControl1.SelectedIndex;

            if (index == 2) return minHeight;
            return controls[index].Height + 100;
        }
    }
}
