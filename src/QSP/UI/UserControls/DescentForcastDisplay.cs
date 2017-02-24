using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.WindAloft;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MathTools.Numbers;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.UserControls
{
    public partial class DescentForcastDisplay : UserControl
    {
        private Locator<IWindTableCollection> windTableLocator;
        private Func<string> destGetter;

        public AirportManager AirportList { get; set; }

        public DescentForcastDisplay()
        {
            InitializeComponent();
        }

        public void Init(
            AirportManager airportList,
            Locator<IWindTableCollection> windTableLocator,
            Func<string> destGetter)
        {
            this.AirportList = airportList;
            this.windTableLocator = windTableLocator;
            destIcaoLbl.Text = "";
            desForcastLastUpdatedLbl.Text = "";
            this.destGetter = destGetter;

            updateDesForcastBtn.Click += (s, e) => UpdateDesForcast();
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
            double[] flightLevels = { 60, 90, 120, 180, 240, 300, 340, 390, 440, 490 };
            var winds = DescendForcast.Generate(
                windTableLocator.Instance, airport.Lat, airport.Lon, flightLevels).ToList();

            var result = new StringBuilder("\n");

            for (int i = 0; i < flightLevels.Length; i++)
            {
                var flightLevel = flightLevels[i].ToString().PadLeft(3, '0');
                var direction = winds[i].DirectionString();
                int speed = RoundToInt(winds[i].Speed);

                result.AppendLine($"        FL{flightLevel}   {direction}/{speed}");
            }

            return result.ToString();
        }
    }
}
