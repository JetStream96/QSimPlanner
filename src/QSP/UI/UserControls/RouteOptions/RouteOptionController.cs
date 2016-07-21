using QSP.Common.Options;
using QSP.GoogleMap;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.UI.Controllers;
using QSP.UI.Controls;
using QSP.UI.Factories;
using QSP.UI.Utilities;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.UserControls.RouteOptions
{
    public partial class RouteOptionController
    {
        private AppOptions appSettings;
        private WaypointList wptList;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private ISelectedProcedureProvider origController;
        private ISelectedProcedureProvider destController;
        private Func<AvgWindCalculator> windCalcGetter;
        private Label routeDisLbl;
        private DistanceDisplayStyle displayStyle;
        private Func<string> routeTxtGetter;
        private Action<string> routeTxtSetter;
        private IClickable findRouteBtn;
        private IClickable analyzeRouteBtn;
        private IClickable exportBtn;
        private IClickable showMapBtn;

        public RouteGroup Route { get; private set; }

        public RouteOptionController(
            AppOptions appSettings,
            WaypointList wptList,
            AirportManager airportList,
            TrackInUseCollection tracksInUse,
            ISelectedProcedureProvider origController,
            ISelectedProcedureProvider destController,
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
            this.appSettings = appSettings;
            this.wptList = wptList;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
            this.origController = origController;
            this.destController = destController;
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
            var sid = origController.GetSelectedProcedures();
            var star = destController.GetSelectedProcedures();

            var finder = new RouteFinderFacade(
                wptList,
                airportList,
                appSettings.NavDataLocation,
                null,
                windCalcGetter());

            var result = finder.FindRoute(
                origController.Icao, origController.Rwy, sid,
                destController.Icao, destController.Rwy, star);

            Route = new RouteGroup(result, tracksInUse);
            var expanded = Route.Expanded;

            routeTxtSetter(expanded.ToString(false, false));
            UpdateRouteDistanceLbl(routeDisLbl, expanded, displayStyle);
        }

        private void ExportRouteFiles(object sender, EventArgs e)
        {
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
                        airportList,
                        wptList),
                    tracksInUse);

                var expanded = Route.Expanded;

                routeTxtSetter(expanded.ToString(false, false));
                UpdateRouteDistanceLbl(routeDisLbl, expanded, displayStyle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (Route == null)
            {
                MsgBoxHelper.ShowWarning(
                    "Please find or analyze a route first.");
                return;
            }

            var wb = new WebBrowser();
            wb.Size = new Size(1200, 800);

            var GoogleMapDrawRoute = RouteDrawing.MapDrawString(
                Route.Expanded, wb.Size.Width - 20, wb.Size.Height - 30);

            wb.DocumentText = GoogleMapDrawRoute.ToString();

            using (var frm = FormFactory.GetForm(wb.Size))
            {
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Controls.Add(wb);
                frm.ShowDialog();
            }
        }
    }
}
