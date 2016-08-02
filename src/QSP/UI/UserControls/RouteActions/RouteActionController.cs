using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.UI.Controllers;
using QSP.UI.Controls;
using QSP.UI.Utilities;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.UserControls.RouteActions
{
    public partial class RouteActionController
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

        private AppOptions appSettings
        {
            get { return appOptionsLocator.Instance; }
        }

        private AirportManager airportList
        {
            get { return airwayNetwork.AirportList; }
        }

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
            IClickable showMapBtn)
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
        }

        public void Subscribe()
        {
            findRouteBtn.Click += FindRouteClick;
            analyzeRouteBtn.Click += AnalyzeRouteClick;
            exportBtn.Click += ExportRouteFiles;
            showMapBtn.Click += ShowMapClick;
        }

        private void FindRouteClick(object sender, EventArgs e)
        {
            try
            {
                FindRoute();
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowWarning(ex.Message);
            }
        }

        // Can throw exceptions.
        private void FindRoute()
        {
            var orig = origController.Icao;
            var dest = destController.Icao;

            if (airportList.Find(orig) == null)
            {
                throw new ArgumentException(
                    "Cannot find origin airport in Nav Data.");
            }

            if (airportList.Find(dest) == null)
            {
                throw new ArgumentException(
                    "Cannot find destination airport in Nav Data.");
            }

            var sid = origController.GetSelectedProcedures();
            var star = destController.GetSelectedProcedures();

            var finder = new RouteFinderFacade(
                airwayNetwork.WptList,
                airwayNetwork.AirportList,
                appSettings.NavDataLocation,
                checkedCodesLocator.Instance,
                windCalcGetter());

            var result = finder.FindRoute(
                orig, origController.Rwy, sid,
                dest, destController.Rwy, star);

            Route = new RouteGroup(result, airwayNetwork.TracksInUse);
            ShowRouteTxt();
        }

        private void ShowRouteTxt()
        {
            var routeToShow = appSettings.ShowTrackIdOnly ?
                Route.Folded : Route.Expanded;

            var showDct = !appSettings.HideDctInRoute;
            routeTxtSetter(routeToShow.ToString(false, false, showDct));
            UpdateRouteDistanceLbl(routeDisLbl, Route.Expanded, displayStyle);
        }

        private void ExportRouteFiles(object sender, EventArgs e)
        {
            if (Route == null)
            {
                MsgBoxHelper.ShowWarning(
                    "Please find or analyze a route first.");
                return;
            }

            var cmds = appSettings.ExportCommands.Values;
            var writer = new FileExporter(
                Route.Expanded, airportList, cmds);

            var reports = writer.Export();
            ShowReports(reports);
        }

        private void AnalyzeRouteClick(object sender, EventArgs e)
        {
            //TODO: Need better exception message for AUTO, RAND commands
            try
            {
                var input = routeTxtGetter().ToUpper();

                Route = new RouteGroup(
                    RouteAnalyzerFacade.AnalyzeWithCommands(
                        input,
                        origController.Icao,
                        origController.Rwy,
                        destController.Icao,
                        destController.Rwy,
                        appSettings.NavDataLocation,
                        airwayNetwork.AirportList,
                        airwayNetwork.WptList),
                    airwayNetwork.TracksInUse);

                ShowRouteTxt();
            }
            catch (Exception ex)
            {
                MsgBoxHelper.ShowWarning(ex.Message);
            }
        }

        private static void ShowReports(
            IEnumerable<FileExporter.Status> reports)
        {
            if (reports.Count() == 0)
            {
                MessageBox.Show(
                    "No route file to be exported. " +
                    "Please select select export settings in options page.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var msg = new StringBuilder();
                var success = reports.Where(r => r.Successful);

                if (success.Count() > 0)
                {
                    msg.AppendLine(
                        $"{success.Count()} company route(s) exported:");

                    foreach (var i in success)
                    {
                        msg.AppendLine(i.FilePath);
                    }
                }

                var errors = reports.Where(r => r.Successful == false);

                if (errors.Count() > 0)
                {
                    msg.AppendLine("\n\n" +
                        $"Failed to export {errors.Count()} file(s) into:");

                    foreach (var j in errors)
                    {
                        msg.AppendLine(j.FilePath);
                    }
                }

                var icon =
                    errors.Count() > 0 ?
                    MessageBoxIcon.Warning :
                    MessageBoxIcon.Information;

                MessageBox.Show(
                    msg.ToString(),
                    "",
                    MessageBoxButtons.OK,
                    icon);
            }
        }

        private void ShowMapClick(object sender, EventArgs e)
        {
            ShowMapHelper.ShowMap(Route);
        }
    }
}
