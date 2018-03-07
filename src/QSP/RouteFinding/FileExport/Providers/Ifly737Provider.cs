using System;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class Ifly737Provider
    {
        private Route route;
        private AirportManager airports;

        public Ifly737Provider(Route route, AirportManager airports)
        {
            if (route.Count < 2) throw new ArgumentException();

            this.route = route;
            this.airports = airports;
        }

        public string GetExportText()
        {
            throw new NotImplementedException();
        }
    }
}