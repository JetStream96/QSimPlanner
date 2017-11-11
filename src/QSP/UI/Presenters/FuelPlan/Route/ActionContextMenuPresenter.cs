using CommonLibrary.Attributes;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.Finder;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Tracks;
using QSP.UI.Controllers;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Views.Route;
using QSP.UI.Views.Route.Actions;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QSP.UI.Presenters.FuelPlan.Route
{
    public class ActionContextMenuPresenter
    {
        private ISupportActionContextMenu view;
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private ISelectedProcedureProvider origController;
        private ISelectedProcedureProvider destController;
        private Locator<CountryCodeCollection> checkedCodesLocator;
        private Func<AvgWindCalculator> windCalcGetter;

        public FindAltnPresenter FindAltnPresenter(IFindAltnView altnView) =>
            new FindAltnPresenter(altnView, airwayNetwork.AirportList);

        public string DestIcao => destController.Icao;
        private AppOptions AppSettings => appOptionsLocator.Instance;
        private AirportManager AirportList => airwayNetwork.AirportList;

        public RouteGroup Route { get; private set; }

        public ActionContextMenuPresenter(
            ISupportActionContextMenu view,
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
            Locator<CountryCodeCollection> checkedCodesLocator,
            Func<AvgWindCalculator> windCalcGetter)
        {
            this.view = view;
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.origController = origController;
            this.destController = destController;
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
                view.ShowWarning(ex.Message);
            }
        }

        [Throws]
        private void FindRoutePrivate()
        {
            var orig = origController.Icao;
            var dest = destController.Icao;

            if (AirportList[orig] == null)
            {
                throw new ArgumentException("Cannot find origin airport in Nav Data.");
            }

            if (AirportList[dest] == null)
            {
                throw new ArgumentException("Cannot find destination airport in Nav Data.");
            }

            var sid = origController.GetSelectedProcedures();
            var star = destController.GetSelectedProcedures();

            var finder = new RouteFinderFacade(
                airwayNetwork.WptList,
                airwayNetwork.AirportList,
                AppSettings.NavDataLocation,
                checkedCodesLocator.Instance,
                windCalcGetter());

            Route = new RouteGroup(
                finder.FindRoute(
                    orig, origController.Rwy, sid,
                    dest, destController.Rwy, star),
                airwayNetwork.TracksInUse);

            ShowRouteTxt();
        }

        private void ShowRouteTxt()
        {
            var selected = AppSettings.ShowTrackIdOnly ? Route.Folded : Route.Expanded;
            var showDct = !AppSettings.HideDctInRoute;
            view.Route = selected.ToString(showDct);
            view.DistanceInfo = RouteDistanceDisplay.GetDisplay(Route.Expanded, Style.Long);
        }

        public void ExportRouteFiles()
        {
            if (Route == null)
            {
                view.ShowInfo("Please find or analyze a route first.");
                return;
            }

            var cmds = AppSettings.ExportCommands.Values;
            var writer = new FileExporter(Route.Expanded, AirportList, cmds);

            IEnumerable<FileExporter.Status> reports = null;

            try
            {
                reports = writer.Export();
            }
            catch (Exception ex)
            {
                view.ShowWarning(ex.Message);
                return;
            }

            ShowReports(reports.ToList());
        }

        public void AnalyzeRoute()
        {
            try
            {
                var input = view.Route.ToUpper();

                var result = RouteAnalyzerFacade.AnalyzeWithCommands(
                        input,
                        origController.Icao,
                        origController.Rwy,
                        destController.Icao,
                        destController.Rwy,
                        AppSettings.NavDataLocation,
                        airwayNetwork.AirportList,
                        airwayNetwork.WptList);

                Route = new RouteGroup(result, airwayNetwork.TracksInUse);
                ShowRouteTxt();
            }
            catch (Exception ex)
            {
                view.ShowWarning(ex.Message);
            }
        }

        private void ShowReports(List<FileExporter.Status> reports)
        {
            if (!reports.Any())
            {
                view.ShowInfo("No route file to export. Please set export settings in options page.");
                return;
            }

            var msg = new StringBuilder();
            var success = reports.Where(r => r.Successful).ToList();

            if (success.Any())
            {
                msg.AppendLine($"{success.Count} company route(s) exported:");

                foreach (var i in success)
                {
                    msg.AppendLine(i.FilePath);
                }
            }

            var errors = reports.Where(r => !r.Successful).ToList();

            if (errors.Any())
            {
                msg.AppendLine($"\n\nFailed to export {errors.Count} file(s) into:");

                foreach (var j in errors)
                {
                    msg.AppendLine(j.FilePath);
                }
            }

            if (errors.Any(e => e.MayBePermissionIssue))
            {
                msg.AppendLine("\nYou can try to run this application " +
                    "as administrator.");
            }

            if (errors.Any())
            {
                view.ShowWarning(msg.ToString());
            }
            else
            {
                view.ShowInfo(msg.ToString());
            }
        }

        public void ShowMap()
        {
            if (Route == null)
            {
                view.ShowInfo("Please find a route first.");
                return;
            }

            view.ShowMap(Route.Expanded);
        }

        public void ShowMapBrowser()
        {
            if (Route == null)
            {
                view.ShowInfo("Please find a route first.");
                return;
            }

            view.ShowMapBrowser(Route.Expanded);
        }
    }
}