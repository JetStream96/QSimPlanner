using QSP.LibraryExtension;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.UI.Views.MiscInfo;
using QSP.WindAloft;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Presenters.MiscInfo
{
    public class DescentForcastPresenter
    {
        private IDescentForcastView view;
        private Locator<IWxTableCollection> windTableLocator;
        private Func<string> destGetter;

        public AirportManager AirportList { get; set; }

        public DescentForcastPresenter(
            IDescentForcastView view,
            AirportManager airportList,
            Locator<IWxTableCollection> windTableLocator,
            Func<string> destGetter)
        {
            this.view = view;
            this.AirportList = airportList;
            this.windTableLocator = windTableLocator;
            this.destGetter = destGetter;
        }

        public async Task UpdateForcast()
        {
            if (windTableLocator.Instance is DefaultWindTableCollection)
            {
                view.Forcast = "\n\n\n       Wind aloft has not been downloaded.";
                return;
            }

            var dest = destGetter();

            try
            {
                view.Forcast = "\n\n\n           Refreshing ...";
                view.DestinationIcao = dest;

                view.Forcast =
                    await Task.Factory.StartNew(() => GenDesForcastString(dest));

                view.LastUpdateTime = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                Log(ex);
                view.Forcast = "\n\n\n       Unable to get descent forcast for " + dest;
            }
        }

        private string GenDesForcastString(string icao)
        {
            var airport = AirportList[icao];
            double[] flightLevels = { 60, 90, 120, 180, 240, 300, 340, 390, 440, 490 };
            var windTemp = DescendForcast.Generate(
                windTableLocator.Instance, airport.Lat, airport.Lon, flightLevels).ToList();

            var result = new StringBuilder("\n");

            for (int i = 0; i < flightLevels.Length; i++)
            {
                var (wind,temp) = windTemp[i];
                var flightLevel = flightLevels[i].ToString().PadLeft(3, '0');
                var direction = wind.DirectionString();
                int speed = Numbers.RoundToInt(wind.Speed);
                var tempInt = Numbers.RoundToInt(temp);

                result.AppendLine($"        FL{flightLevel}   {direction}/{speed} ({tempInt})");
            }

            return result.ToString();
        }
    }
}
