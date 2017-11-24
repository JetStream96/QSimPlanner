using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.TerminalProcedures.Sid;
using QSP.RouteFinding.Tracks;
using QSP.UI.Presenters.FuelPlan.Route;
using System.Collections.Generic;

namespace QSP.UI.Controllers
{
    public class DestinationSidSelection : ISelectedProcedureProvider
    {
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptions;
        private FinderOptionPresenter presenter;

        public DestinationSidSelection(AirwayNetwork airwayNetwork,
            Locator<AppOptions> appOptions,
            FinderOptionPresenter presenter)
        {
            this.airwayNetwork = airwayNetwork;
            this.appOptions = appOptions;
            this.presenter = presenter;
        }

        public string Icao => presenter.Icao;

        public string Rwy => presenter.Rwy;

        public List<string> GetSelectedProcedures()
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
