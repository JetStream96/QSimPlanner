using System;
using QSP.FuelCalculation.Calculations;
using QSP.FuelCalculation.FuelDataNew;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;

namespace QSP.FuelCalculation.Results
{
    public class FuelReportGenerator
    {
        private readonly AirportManager airportList;
        private readonly ICrzAltProvider altProvider;
        private readonly IWindTableCollection windTable;
        private readonly Route route;
        private readonly FuelParameters para;
        private readonly double maxAlt;
        
        public FuelReportGenerator(
            AirportManager airportList,
            ICrzAltProvider altProvider,
            IWindTableCollection windTable,
            Route route,
            FuelParameters para,
            double maxAlt = 41000.0)
        {
            this.airportList = airportList;
            this.altProvider = altProvider;
            this.windTable = windTable;
            this.route = route;
            this.para = para;
            this.maxAlt = maxAlt;
        }

        public FuelReport Generate()
        {

        }
    }
}