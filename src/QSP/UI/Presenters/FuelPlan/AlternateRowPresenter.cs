using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Presenters.FuelPlan.Route;
using QSP.UI.Views.FuelPlan;
using QSP.UI.Views.Route;
using QSP.WindAloft;
using System;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternateRowPresenter
    {
        public IAlternateRowView View { get; private set; }

        private ActionContextMenuPresenter contextMenuPresenter;
        private AirwayNetwork airwayNetwork;
        private ISelectedProcedureProvider destController;

        public FindAltnPresenter FindAltnPresenter(IFindAltnView altnView) =>
            new FindAltnPresenter(altnView, airwayNetwork.AirportList);

        public string DestIcao => destController.Icao;
        public RouteGroup Route => contextMenuPresenter.Route;

        public AlternateRowPresenter(
            IAlternateRowView view,
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ISelectedProcedureProvider destController,
            Locator<CountryCodeCollection> checkedCodesLocator,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.View = view;

            contextMenuPresenter = new ActionContextMenuPresenter(
                view,
                appOptionsLocator,
                airwayNetwork,
                destController,
                view,
                checkedCodesLocator,
                windCalcGetter);

            this.airwayNetwork = airwayNetwork;
            this.destController = destController;
        }

        public void FindRoute() => contextMenuPresenter.FindRoute();
        public void ExportRouteFiles() => contextMenuPresenter.ExportRouteFiles();
        public void AnalyzeRoute() => contextMenuPresenter.AnalyzeRoute();
        public void ShowMap() => contextMenuPresenter.ShowMap();
        public void ShowMapBrowser() => contextMenuPresenter.ShowMapBrowser();
    }
}
