using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class FsxProvider : IExportProvider
    {
        private Route route;
        private AirportManager airports;

        public FsxProvider(Route route, AirportManager airports)
        {
            this.route = route;
            this.airports = airports;
        }

        public string GetExportText()
        {
            throw new NotImplementedException();
        }

        public static string LatLonAlt(ICoordinate latLon, double altitudeFt)
        {
            return "";
        }
    }
}
