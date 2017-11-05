using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.UI.Views.MiscInfo;

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
                FindAirport();
            }
        }

        public AirportMapPresenter(IAirportMapView view, AirportManager airports)
        {
            this.view = view;
            this._airports = airports;
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

        public async Task SetMetar()
        {
            view.MetarText = "Updating ...";
            view.MetarText = await Task.Factory.StartNew(
                () => MetarDownloader.TryGetMetar(view.IcaoText));
        }

        public void UpdateAirport()
        {
            
        }
    }
}
