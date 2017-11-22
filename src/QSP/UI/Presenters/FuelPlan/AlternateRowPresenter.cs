using CommonLibrary.Attributes;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Presenters.FuelPlan.Route;
using QSP.UI.Views.FuelPlan;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;
using QSP.UI.Views.FuelPlan.Route;

namespace QSP.UI.Presenters.FuelPlan
{
    public class AlternateRowPresenter: IRefreshForOptionChange
    {
        public IAlternateRowView View { get; private set; }

        private Locator<AppOptions> appOptions;
        private ActionContextMenuPresenter contextMenuPresenter;
        private AirwayNetwork airwayNetwork;
        private ISelectedProcedureProvider destController;

        public FindAltnPresenter FindAltnPresenter(IFindAltnView altnView) =>
            new FindAltnPresenter(altnView, airwayNetwork.AirportList);

        public string DestIcao => destController.Icao;
        public RouteGroup Route => contextMenuPresenter.Route;

        [Throws]
        public List<string> GetAllProcedures()
        {
            var wptList = airwayNetwork.WptList;
            var handler = StarHandlerFactory.GetHandler(
                View.Icao,
                appOptions.Instance.NavDataLocation,
                wptList,
                new WaypointListEditor(wptList),
                airwayNetwork.AirportList);

            return handler.StarCollection.GetStarList(View.Rwy);
        }

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

            this.appOptions = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.destController = destController;
        }
        
        public void FindRoute() => contextMenuPresenter.FindRoute();
        public void ExportRouteFiles() => contextMenuPresenter.ExportRouteFiles();
        public void AnalyzeRoute() => contextMenuPresenter.AnalyzeRoute();
        public void ShowMap() => contextMenuPresenter.ShowMap();
        public void ShowMapBrowser() => contextMenuPresenter.ShowMapBrowser();

        public void UpdateRunways()
        {
            var airport = airwayNetwork.AirportList[View.Icao];
            if (airport == null) return;
            View.RunwayList = airport.Rwys.Select(r => r.RwyIdent);
        }

        public void RefreshForAirportListChange()
        {
            var rwy = View.Rwy;
            UpdateRunways();
            View.SetRwy(rwy);
        }

        public void RefreshForNavDataLocationChange()
        {
            RefreshForAirportListChange();
        }
    }
}
