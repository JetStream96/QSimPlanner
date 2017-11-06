using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.UI.Views.MiscInfo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSP.UI.Presenters.MiscInfo
{
    public class AirportMapPresenter
    {
        private IAirportMapView view;
        private string _orig;
        private string _dest;
        private IEnumerable<string> _altn;

        private AirportManager _airports;
        public AirportManager Airports
        {
            get { return _airports; }
            set
            {
                _airports = value;
                UpdateAirport();
            }
        }

        public AirportMapPresenter(IAirportMapView view, AirportManager airports)
        {
            this.view = view;
            this._airports = airports;
        }

        public bool BrowserEnabled
        {
            set => view.BrowserEnabled = value;
        }

        public string Orig
        {
            set
            {
                _orig = value;
                UpdateIcaoList();
            }
        }

        public string Dest
        {
            set
            {
                _dest = value;
                UpdateIcaoList();
            }
        }

        public IEnumerable<string> Altn
        {
            set
            {
                _altn = value;
                UpdateIcaoList();
            }
        }

        private void UpdateIcaoList()
        {
            view.IcaoList = new[] { _orig, _dest }
                    .Concat(_altn ?? new string[0])
                    .Where(s => !string.IsNullOrEmpty(s));
        }

        public async Task SetMetarText()
        {
            var icao = view.IcaoText;
            view.MetarText = "Updating ...";
            view.MetarText = await Task.Run(() => MetarDownloader.TryGetMetar(icao));
        }

        public void UpdateAirport()
        {
            view.Runways = new IRwyData[0];
            var icao = view.IcaoText;
            if (icao.Length != 4 || Airports == null) return;
            var airport = Airports[icao];

            if (airport != null && airport.Rwys.Count > 0)
            {
                SetMetarText();

                view.AirportName = airport.Name;
                view.LatLon = airport;
                view.ElevationFt = airport.Elevation;
                view.TransitionAltExist = airport.TransAvail;

                if (airport.TransAvail)
                {
                    view.TransitionAltLevel = (airport.TransAlt, airport.TransLvl);
                }

                view.Runways = airport.Rwys;
                view.ShowMap(airport);
            }
        }
    }
}
