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
using QSP.UI.Util;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QSP.UI.Views;
using static QSP.UI.Util.RouteDistanceDisplay;

namespace QSP.UI.UserControls.RouteActions
{
    public class RouteActionController
    {
        private Locator<AppOptions> appOptionsLocator;
        private AirwayNetwork airwayNetwork;
        private ISelectedProcedureProvider origController;
        private ISelectedProcedureProvider destController;
        private Locator<CountryCodeCollection> checkedCodesLocator;
        private Func<AvgWindCalculator> windCalcGetter;
        private Label routeDisLbl;
        private DistanceDisplayStyle displayStyle;
        private Func<string> routeTxtGetter;
        private Action<string> routeTxtSetter;
        private IClickable findRouteBtn;
        private IClickable analyzeRouteBtn;
        private IClickable exportBtn;
        private IClickable showMapBtn;
        private IClickable showMapBrowserBtn;
        private Form parentForm;

        private AppOptions AppSettings => appOptionsLocator.Instance;
        private AirportManager AirportList => airwayNetwork.AirportList;

        public RouteGroup Route { get; private set; }

        public RouteActionController(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
            Locator<CountryCodeCollection> checkedCodesLocator,
            Func<AvgWindCalculator> windCalcGetter,
            Label routeDisLbl,
            DistanceDisplayStyle displayStyle,
            Func<string> routeTxtGetter,
            Action<string> routeTxtSetter,
            IClickable findRouteBtn,
            IClickable analyzeRouteBtn,
            IClickable exportBtn,
            IClickable showMapBtn,
            IClickable showMapBrowserBtn,
            Form parentForm)
        {
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.origController = origController;
            this.destController = destController;
            this.checkedCodesLocator = checkedCodesLocator;
            this.windCalcGetter = windCalcGetter;
            this.routeDisLbl = routeDisLbl;
            this.displayStyle = displayStyle;
            this.routeTxtGetter = routeTxtGetter;
            this.routeTxtSetter = routeTxtSetter;
            this.findRouteBtn = findRouteBtn;
            this.analyzeRouteBtn = analyzeRouteBtn;
            this.exportBtn = exportBtn;
            this.showMapBtn = showMapBtn;
            this.showMapBrowserBtn = showMapBrowserBtn;
            this.parentForm = parentForm;
        }

        public void Subscribe()
        {
            findRouteBtn.Click += FindRouteClick;
            analyzeRouteBtn.Click += AnalyzeRouteClick;
            exportBtn.Click += ExportRouteFiles;
            showMapBtn.Click += ShowMapClick;
            showMapBrowserBtn.Click += ShowMapBrowserClick;
        }

        private void FindRouteClick(object sender, EventArgs e)
        {
            try
            {
                FindRoute();
            }
            catch (Exception ex)
            {
                parentForm.ShowWarning(ex.Message);
            }
        }

        // @Throws
        private void FindRoute()
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
            var selected = AppSettings.ShowTrackIdOnly ?
                Route.Folded : Route.Expanded;
            var showDct = !AppSettings.HideDctInRoute;
            routeTxtSetter(selected.ToString(showDct));
            UpdateRouteDistanceLbl(routeDisLbl, Route.Expanded, displayStyle);
        }

        private void ExportRouteFiles(object sender, EventArgs e)
        {
            if (Route == null)
            {
                parentForm.ShowInfo("Please find or analyze a route first.");
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
                parentForm.ShowWarning(ex.Message);
                return;
            }

            ShowReports(reports.ToList());
        }

        private void AnalyzeRouteClick(object sender, EventArgs e)
        {
            try
            {
                var input = routeTxtGetter().ToUpper();

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
                parentForm.ShowWarning(ex.Message);
            }
        }

        private void ShowReports(List<FileExporter.Status> reports)
        {
            if (!reports.Any())
            {
                parentForm.ShowInfo(
                    "No route file to export. Please set export settings in options page.");
            }
            else
            {
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
                    parentForm.ShowWarning(msg.ToString());
                }
                else
                {
                    parentForm.ShowInfo(msg.ToString());
                }
            }
        }

        private void ShowMapClick(object sender, EventArgs e)
        {
            if (Route == null)
            {
                MsgBoxHelper.ShowInfo(parentForm, "Please find a route first.");
                return;
            }

            ShowMapHelper.ShowMap(Route.Expanded, parentForm.Size, parentForm);
        }

        private void ShowMapBrowserClick(object sender, EventArgs e)
        {
            if (Route == null)
            {
                MsgBoxHelper.ShowInfo(parentForm, "Please find a route first.");
                return;
            }

            ShowMapHelper.ShowMap(Route.Expanded, parentForm.Size, parentForm, true, true);
        }
    }
}
