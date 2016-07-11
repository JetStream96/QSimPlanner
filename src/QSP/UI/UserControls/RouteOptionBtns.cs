using QSP.Common.Options;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.UI.Controllers;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QSP.UI.UserControls
{
    public partial class RouteOptionBtns : UserControl
    {
        private AppOptions appSettings;
        private WaypointList wptList;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private RouteFinderSelection origController;
        private RouteFinderSelection destController;
        private Label routeDisLbl;
        private Func<string> routeTxtGetter;
        private Action<string> routeTxtSetter;

        public RouteGroup Route { get; private set; }

        public RouteOptionBtns()
        {
            InitializeComponent();
        }

        public void Init(
            AppOptions appSettings,
            WaypointList wptList,
            AirportManager airportList,
            TrackInUseCollection tracksInUse,
            RouteFinderSelection origController,
            RouteFinderSelection destController,
            Label routeDisLbl,
            Func<string> routeTxtGetter,
            Action<string> routeTxtSetter)
        {
            this.appSettings = appSettings;
            this.wptList = wptList;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
            this.origController = origController;
            this.destController = destController;
            this.routeDisLbl = routeDisLbl;
            this.routeTxtGetter = routeTxtGetter;
            this.routeTxtSetter = routeTxtSetter;
        }

        public void Subscribe()
        {
            findRouteBtn.Click += FindRouteClick;
            analyzeRouteBtn.Click += AnalyzeRouteClick;
            exportBtn.Click += ExportRouteFiles;
        }

        private void FindRouteClick(object sender, EventArgs e)
        {
            var sid = origController.GetSelectedProcedures();
            var star = destController.GetSelectedProcedures();

            // TODO: need to be integrated with fuel calculator
            //var windCalc = windTables == null ?
            //    null : new AvgWindCalculator(windTables, 460, 370.0);

            var finder = new RouteFinderFacade(
                wptList,
                airportList,
                appSettings.NavDataLocation,
                null,  //TODO: add this
                null); //TODO: add this as well

            var result = finder.FindRoute(
                origController.Icao, origController.Rwy, sid,
                destController.Icao, destController.Rwy, star);

            this.Route = new RouteGroup(result, tracksInUse);
            var route = this.Route.Expanded;

            routeTxtSetter(route.ToString(false, false));
            RouteDistanceDisplay.UpdateRouteDistanceLbl(routeDisLbl, route);
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

                this.Route =
                    new RouteGroup(
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

                var route = this.Route.Expanded;

                routeTxtSetter(route.ToString(false, false));
                RouteDistanceDisplay.UpdateRouteDistanceLbl(
                    routeDisLbl, route);
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
    }
}
