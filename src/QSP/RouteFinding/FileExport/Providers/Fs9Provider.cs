using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Linq;
using System.Xml.Linq;
using static QSP.AviationTools.Coordinates.FormatDegreeMinuteSecond;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class Fs9Provider : IExportProvider
    {
        private Route route;
        private AirportManager airports;

        public Fs9Provider(Route route, AirportManager airports)
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
