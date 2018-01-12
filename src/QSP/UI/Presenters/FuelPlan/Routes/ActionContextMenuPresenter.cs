using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Finder;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Tracks;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes.Actions;
using QSP.WindAloft;
using System;
using System.Linq;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Models.FuelPlan.Routes;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public class ActionContextMenuPresenter : IRefreshForNavDataChange
    {
        private ISupportActionContextMenu view;
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private ISelectedProcedureProvider origProvider;
        private ISelectedProcedureProvider destProvider;
        private Locator<CountryCodeCollection> checkedCodesLocator;
        private Func<AvgWindCalculator> windCalcGetter;

        public string DestIcao => destProvider.Icao;
        private AppOptions AppOptions => appOptionsLocator.Instance;
        private AirportManager AirportList => airwayNetwork.AirportList;

        public RouteGroup Route { get; private set; }

        public ActionContextMenuPresenter(
            ISupportActionContextMenu view,
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ISelectedProcedureProvider origProvider,
            ISelectedProcedureProvider destProvider,
            Locator<CountryCodeCollection> checkedCodesLocator,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.view = view;
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.origProvider = origProvider;
            this.destProvider = destProvider;
            this.checkedCodesLocator = checkedCodesLocator;
            this.windCalcGetter = windCalcGetter;
        }

        public void FindRoute()
        {
            try
            {
                FindRoutePrivate();
            }
            catch (Exception ex)
            {
                view.ShowMessage(ex.Message, MessageLevel.Warning);
            }
        }

        /// <exception cref="ArgumentException"></exception>
        private void EnsureAirportExists(string icao)
        {
            if (AirportList[icao] == null)
            {
                var msg = ActionContextMenuHelper.NonExistingAirportMsg(icao);
                throw new ArgumentException(msg);
            }
        }

        /// <exception cref="Exception"></exception>
        private void FindRoutePrivate()
        {
            var orig = origProvider.Icao;
            var dest = destProvider.Icao;

            EnsureAirportExists(orig);
            EnsureAirportExists(dest);

            var sid = origProvider.GetSelectedProcedures().ToList();
            var star = destProvider.GetSelectedProcedures().ToList();

            var finder = new RouteFinderFacade(
                airwayNetwork.WptList,
                airwayNetwork.AirportList,
                AppOptions.NavDataLocation,
                checkedCodesLocator.Instance,
                windCalcGetter());

            Route = new RouteGroup(
                finder.FindRoute(
                    orig, origProvider.Rwy, sid,
                    dest, destProvider.Rwy, star),
                airwayNetwork.TracksInUse);

            view.ShowRouteTxt(Route, AppOptions);
        }

        public void ExportRouteFiles()
        {
            ActionContextMenuHelper.ExportRouteFiles(
                view,
                Route,
                AppOptions.ExportCommands.Values,
                AirportList);
        }

        public void AnalyzeRoute()
        {
            try
            {
                var orig = origProvider.Icao;
                var dest = destProvider.Icao;

                EnsureAirportExists(orig);
                EnsureAirportExists(dest);

                var input = view.Route.ToUpper();

                var result = RouteAnalyzerFacade.AnalyzeWithCommands(
                        input,
                        orig,
                        origProvider.Rwy,
                        dest,
                        destProvider.Rwy,
                        AppOptions.NavDataLocation,
                        airwayNetwork.AirportList,
                        airwayNetwork.WptList);

                Route = new RouteGroup(result, airwayNetwork.TracksInUse);
                view.ShowRouteTxt(Route, AppOptions);
            }
            catch (Exception ex)
            {
                view.ShowMessage(ex.Message, MessageLevel.Warning);
            }
        }

        public void ShowMap() => view.ShowMap(Route);

        public void ShowMapBrowser() => view.ShowMapBrowser(Route);

        public void OnNavDataChange()
        {
            Route = null;
        }
    }
}