using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.AviationTools.Coordinates;
using QSP.Common.Options;
using QSP.FuelCalculation;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;
using QSP.UI.RoutePlanning;
using QSP.UI.Utilities;
using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.MainForm;
using static QSP.UI.Factories.FormFactory;
using static QSP.UI.Factories.ToolTipFactory;

namespace QSP.UI.UserControls
{
    public partial class FuelPlanningControl : UserControl
    {
        private AppOptions appSettings;
        private WaypointList wptList;
        private AirportManager airportList;
        private TrackInUseCollection tracksInUse;
        private ProcedureFilter procFilter;
        private CountryCodeManager countryCodes;

        private RouteFinderSelection origController;
        private RouteFinderSelection destController;
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        private WeightTextBoxController zfw;
        private WeightController weightControl;
        private AcConfigManager aircrafts;
        private IEnumerable<FuelData> fuelData;

        private RouteGroup mainRoute;

        public FuelPlanningControl()
        {
            InitializeComponent();
        }

        public void Init(
            AppOptions appSettings,
            WaypointList wptList,
            AirportManager airportList,
            TrackInUseCollection tracksInUse,
            ProcedureFilter procFilter,
            CountryCodeManager countryCodes,
            AcConfigManager aircrafts,
            IEnumerable<FuelData> fuelData)
        {
            this.appSettings = appSettings;
            this.wptList = wptList;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
            this.procFilter = procFilter;
            this.countryCodes = countryCodes;
            this.aircrafts = aircrafts;
            this.fuelData = fuelData;

            SetOrigDestControllers();
            SetWeightController();
            FillAircraftSelection();

            wtUnitComboBox.SelectedIndex = 0;
            SubscribeEventHandlers();

            if (acListComboBox.Items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }
        }

        private void SubscribeEventHandlers()
        {
            wtUnitComboBox.SelectedIndexChanged += WtUnitChanged;
            acListComboBox.SelectedIndexChanged += RefreshRegistrations;
            registrationComboBox.SelectedIndexChanged += RegistrationChanged;
            findRouteBtn.Click += FindRouteClick;
            exportBtn.Click += ExportRouteFiles;
        }

        private void FillAircraftSelection()
        {
            acListComboBox.Items.Clear();
            acListComboBox.Items.AddRange(
                aircrafts.Aircrafts
                .Select(c => c.Config.AC)
                .ToArray());
        }

        private void SetOrigDestControllers()
        {
            origController = new RouteFinderSelection(
               origTxtBox,
               true,
               origRwyComboBox,
               sidComboBox,
               filterSidBtn,
               appSettings,
               airportList,
               wptList,
               procFilter);

            destController = new RouteFinderSelection(
                destTxtBox,
                false,
                destRwyComboBox,
                starComboBox,
                filterStarBtn,
                appSettings,
                airportList,
                wptList,
                procFilter);

            origController.Subscribe();
            destController.Subscribe();
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

            mainRoute = new RouteGroup(result, tracksInUse);
            var route = mainRoute.Expanded;

            //PMDGrteFile = new PmdgProvider(route, airportList)
            //    .GetExportText();

            mainRouteRichTxtBox.Text = route.ToString(false, false);
            RouteDistanceDisplay.UpdateRouteDistanceLbl(routeDisLbl, route);
        }

        private void ExportRouteFiles(object sender, EventArgs e)
        {
            var cmds = appSettings.ExportCommands.Values;
            var writer = new FileExporter(
                mainRoute.Expanded, airportList, cmds);

            var reports = writer.Export();
            ShowReports(reports);
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

        private void WtUnitChanged(object sender, EventArgs e)
        {
            var unit = (WeightUnit)wtUnitComboBox.SelectedIndex;
            oew.Unit = unit;
            payload.Unit = unit;
            zfw.Unit = unit;
        }

        private void SetWeightController()
        {
            oew = new WeightTextBoxController(oewTxtBox, oewLbl);
            payload = new WeightTextBoxController(payloadTxtBox, payloadLbl);
            zfw = new WeightTextBoxController(zfwTxtBox, zfwLbl);

            weightControl = new WeightController(
                oew, payload, zfw, payloadTrackBar);
            weightControl.Enable();
        }

        private bool FuelProfileExists(string profileName)
        {
            return fuelData.Any(c => c.ProfileName == profileName);
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac =
                    aircrafts
                    .FindAircraft(acListComboBox.Text);

                var items = registrationComboBox.Items;

                items.Clear();

                items.AddRange(
                    ac
                    .Where(c => FuelProfileExists(c.Config.TOProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());

                if (items.Count > 0)
                {
                    registrationComboBox.SelectedIndex = 0;
                }
            }
        }

        private void RegistrationChanged(object sender, EventArgs e)
        {
            if (registrationComboBox.SelectedIndex < 0)
            {
                //RefreshWtColor();
                return;
            }

            weightControl.AircraftConfig =
                aircrafts.Find(registrationComboBox.Text).Config;
        }
    }
}
