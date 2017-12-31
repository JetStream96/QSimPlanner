using CommonLibrary.Attributes;
using QSP.RouteFinding.Finder;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public class RouteFinderPresenter
    {
        private IRouteFinderView view;
        private IRouteFinderModel model;

        public RouteGroup Route { get; private set; }

        public RouteFinderPresenter(IRouteFinderView view, IRouteFinderModel model)
        {
            this.view = view;
            this.model = model;
        }

        /// <summary>
        /// Finds a route and display. The route found is assigned to Route property.
        /// If a route cannot be found, error message is displayed.
        /// </summary>
        [NoThrow]
        public void FindRoute()
        {
            try
            {
                Route = ToRouteGroup(FindRoutePrivate());
                ShowRouteTxt();
            }
            catch (Exception e)
            {
                view.ShowMessage(e.Message, MessageLevel.Warning);
            }
        }

        /// <exception cref="Exception"></exception>
        public Route FindRoutePrivate()
        {
            var o = view.OrigRow;
            var d = view.DestRow;

            if (o.IsAirport)
            {
                return d.IsAirport ? GetRouteAirportToAirport() : GetRouteAirportToWaypoint();
            }

            return d.IsAirport ? GetRouteWaypointToAirport() : GetRouteWaypointToWaypoint();
        }

        /// <exception cref="InvalidUserInputException"></exception>
        private RouteFinderFacade GetRouteFinder()
        {
            var f = model.FuelPlanningModel;
            var a = f.AirwayNetwork;
            return new RouteFinderFacade(
                a.WptList,
                a.AirportList,
                f.AppOption.Instance.NavDataLocation,
                f.CheckedCountryCodes.Instance,
                model.WindCalc());
        }

        /// <exception cref="ArgumentException"></exception>
        private void WaypointThrowHelper(IRouteFinderRowView v)
        {
            if (!v.SelectedWaypointIndex.HasValue)
            {
                throw new ArgumentException(
                    ActionContextMenuHelper.NonExistingWptMsg(v.WaypointIdent));
            }
        }

        /// <exception cref="ArgumentException"></exception>
        private void EnsureOrigWptExists() => WaypointThrowHelper(view.OrigRow);

        /// <exception cref="ArgumentException"></exception>
        private void EnsureDestWptExists() => WaypointThrowHelper(view.DestRow);

        private void AirportThrowHelper(string icao)
        {
            if (model.FuelPlanningModel.AirwayNetwork.AirportList[icao] == null)
            {
                throw new ArgumentException(
                    ActionContextMenuHelper.NonExistingAirportMsg(icao));
            }
        }

        /// <exception cref="ArgumentException"></exception>
        private void EnsureOrigAirportExists() => AirportThrowHelper(view.OrigIcao);

        /// <exception cref="ArgumentException"></exception>
        private void EnsureDestAirportExists() => AirportThrowHelper(view.DestIcao);

        private int OrigWaypointIndex => view.OrigRow.SelectedWaypointIndex.Value;
        private int DestWaypointIndex => view.DestRow.SelectedWaypointIndex.Value;
        private IReadOnlyList<string> Sids => view.OrigRow.OptionView.SelectedProcedures.ToList();
        private IReadOnlyList<string> Stars => view.DestRow.OptionView.SelectedProcedures.ToList();

        private RouteGroup ToRouteGroup(Route r) => new RouteGroup(r, TracksInUse);

        private TrackInUseCollection TracksInUse =>
            model.FuelPlanningModel.AirwayNetwork.TracksInUse;

        private Route GetRouteWaypointToWaypoint()
        {
            EnsureOrigWptExists();
            EnsureDestWptExists();

            var fuelModel = model.FuelPlanningModel;

            var finder = new RouteFinder(
                fuelModel.AirwayNetwork.WptList,
                fuelModel.CheckedCountryCodes.Instance,
                model.WindCalc());

            return finder.FindRoute(OrigWaypointIndex, DestWaypointIndex);
        }

        /// <exception cref="Exception"></exception>
        private Route GetRouteWaypointToAirport()
        {
            EnsureOrigWptExists();
            EnsureDestAirportExists();

            return GetRouteFinder().FindRoute(
                OrigWaypointIndex,
                view.DestIcao,
                view.DestRwy,
                Stars.ToList());
        }

        /// <exception cref="Exception"></exception>
        private Route GetRouteAirportToWaypoint()
        {
            EnsureOrigAirportExists();
            EnsureDestWptExists();

            return GetRouteFinder().FindRoute(
                view.OrigIcao,
                view.OrigRwy,
                Sids.ToList(),
                DestWaypointIndex);
        }

        /// <exception cref="Exception"></exception>
        private Route GetRouteAirportToAirport()
        {
            EnsureOrigAirportExists();
            EnsureDestAirportExists();

            return GetRouteFinder().FindRoute(
                view.OrigIcao, view.OrigRwy, Sids,
                view.DestIcao, view.DestRwy, Stars);
        }

        [NoThrow]
        private void ShowRouteTxt()
        {
            var o = model.FuelPlanningModel.AppOption.Instance;
            view.ShowRouteTxt(Route, o);
        }

        public void ExportRouteFiles()
        {
            Debug.Assert(view.IsAirportToAirport());

            var o = model.FuelPlanningModel.AppOption.Instance;
            var airportList = model.FuelPlanningModel.AirwayNetwork.AirportList;
            var cmds = o.ExportCommands.Values;
            ActionContextMenuHelper.ExportRouteFiles(view, Route, cmds, airportList);
        }

        public void AnalyzeRoute()
        {
            Debug.Assert(view.IsAirportToAirport());

            try
            {
                var input = view.Route.ToUpper();
                var airwayNetwork = model.FuelPlanningModel.AirwayNetwork;

                var result = RouteAnalyzerFacade.AnalyzeWithCommands(
                        input,
                        view.OrigIcao,
                        view.OrigRwy,
                        view.DestIcao,
                        view.DestRwy,
                        model.FuelPlanningModel.AppOption.Instance.NavDataLocation,
                        airwayNetwork.AirportList,
                        airwayNetwork.WptList);

                Route = ToRouteGroup(result);
                ShowRouteTxt();
            }
            catch (Exception ex)
            {
                view.ShowMessage(ex.Message, MessageLevel.Warning);
            }
        }

        public void ShowMap() => view.ShowMap(Route);

        public void ShowMapBrowser() => view.ShowMapBrowser(Route);
    }
}
