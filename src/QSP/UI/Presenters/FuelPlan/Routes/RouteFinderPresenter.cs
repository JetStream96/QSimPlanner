using QSP.AviationTools.Coordinates;
using QSP.FuelCalculation.FuelData;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Finder;
using QSP.RouteFinding.Routes;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QSP.UI.Presenters.FuelPlan.Routes
{
    public class RouteFinderPresenter
    {
        private IRouteFinderView view;
        private IRouteFinderModel model;

        public RouteGroup Route => view.ActionMenuPresenter.Route;

        public RouteFinderPresenter(IRouteFinderView view, IRouteFinderModel model)
        {
            this.view = view;
            this.model = model;
        }

        public Route FindRoute()
        {
            var o = view.OrigRow;
            var d = view.DestRow;

            if (o.WaypointOptionEnabled)
            {
                return d.WaypointOptionEnabled ?
                    GetRouteWaypointToWaypoint() :
                    GetRouteWaypointToAirport();
            }

            return d.WaypointOptionEnabled ?
                    GetRouteAirportToWaypoint() :
                    GetRouteAirportToAirport();
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

        private int OrigWaypointIndex => view.OrigRow.SelectedWaypointIndex.Value;
        private int DestWaypointIndex => view.DestRow.SelectedWaypointIndex.Value;
        private IReadOnlyList<string> Sids => view.OrigRow.OptionView.SelectedProcedures.Strings;
        private IReadOnlyList<string> Stars => view.DestRow.OptionView.SelectedProcedures.Strings;

        private RouteGroup ToRouteGroup(Route r) =>
            new RouteGroup(r, model.FuelPlanningModel.AirwayNetwork.TracksInUse);

        private Route GetRouteWaypointToWaypoint()
        {
            var fuelModel = model.FuelPlanningModel;

            var finder = new RouteFinder(
                fuelModel.AirwayNetwork.WptList,
                fuelModel.CheckedCountryCodes.Instance,
                model.WindCalc());

            return finder.FindRoute(OrigWaypointIndex, DestWaypointIndex);
        }

        private Route GetRouteWaypointToAirport()
        {
            return GetRouteFinder().FindRoute(
                    OrigWaypointIndex,
                    view.DestIcao,
                    view.DestRwy,
                    Stars.ToList());
        }

        private Route GetRouteAirportToWaypoint()
        {
            return GetRouteFinder().FindRoute(
                     view.OrigIcao,
                     view.OrigRwy,
                     Sids.ToList(),
                     DestWaypointIndex);
        }

        private Route GetRouteAirportToAirport()
        {
            return GetRouteFinder().FindRoute(
                       view.OrigIcao, view.OrigRwy, Sids,
                       view.DestIcao, view.DestRwy, Stars);
        }
    }
}
