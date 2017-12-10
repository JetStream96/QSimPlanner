using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.Tracks;
using QSP.UI.Views.FuelPlan.Routes;
using System.Collections.Generic;

namespace QSP.UI.Controllers
{
    public class DestinationSidSelection : ISelectedProcedureProvider
    {
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptions;
        private IFinderOptionView option;

        public DestinationSidSelection(AirwayNetwork airwayNetwork,
            Locator<AppOptions> appOptions,
            IFinderOptionView option)
        {
            this.airwayNetwork = airwayNetwork;
            this.appOptions = appOptions;
            this.option = option;
        }

        public string Icao => option.Icao;

        public string Rwy => option.SelectedRwy;

        public IEnumerable<string> GetSelectedProcedures()
        {
            return SidHandlerFactory.GetHandler(
                Icao,
                appOptions.Instance.NavDataLocation,
                airwayNetwork.WptList,
                airwayNetwork.WptList.GetEditor(),
                airwayNetwork.AirportList)
                .GetSidList(Rwy);
        }
    }
}
