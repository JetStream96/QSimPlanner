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
using QSP.RouteFinding.RouteAnalyzers;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;
using QSP.UI.RoutePlanning;
using QSP.UI.Utilities;
using QSP.Utilities.Units;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.UI.Factories.FormFactory;
using static QSP.UI.Factories.ToolTipFactory;
using QSP.LibraryExtension;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Doubles;
using static QSP.Utilities.Units.Conversions;
using QSP.FuelCalculation.Calculators;
using QSP.Common;

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
        private WindTableCollection windTables;

        private RouteFinderSelection origController;
        private RouteFinderSelection destController;
        public WeightTextBoxController Oew { get; private set; }
        public WeightTextBoxController Payload { get; private set; }
        public WeightTextBoxController Zfw { get; private set; }
        public WeightTextBoxController MissedApproach { get; private set; }
        public WeightTextBoxController Extra { get; private set; }
        private WeightController weightControl;
        private AlternateController altnControl;
        private AcConfigManager aircrafts;
        private IEnumerable<FuelData> fuelData;

        private RouteGroup routeToDest;

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
            WindTableCollection windTables,
            AcConfigManager aircrafts,
            IEnumerable<FuelData> fuelData)
        {
            this.appSettings = appSettings;
            this.wptList = wptList;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
            this.procFilter = procFilter;
            this.countryCodes = countryCodes;
            this.windTables = windTables;
            this.aircrafts = aircrafts;
            this.fuelData = fuelData;

            SetAltnController();
            SetDefaultState();
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

        public WeightUnit WeightUnit
        {
            get
            {
                return (WeightUnit)wtUnitComboBox.SelectedIndex;
            }
        }

        private void SetAltnController()
        {
            var controlsBelow = new Control[] 
            {
                addAltnBtn,
                calculateBtn,
                fuelParaGroupBox,
                fuelReportGroupBox
            };

            altnControl = new AlternateController(
                controlsBelow,
                alternateGroupBox,
                appSettings,
                airportList,
                wptList);
        }

        private void SubscribeEventHandlers()
        {
            wtUnitComboBox.SelectedIndexChanged += WtUnitChanged;
            acListComboBox.SelectedIndexChanged += RefreshRegistrations;
            registrationComboBox.SelectedIndexChanged += RegistrationChanged;
            findRouteBtn.Click += FindRouteClick;
            analyzeRouteBtn.Click += AnalyzeRouteClick;
            exportBtn.Click += ExportRouteFiles;
            calculateBtn.Click += Calculate;
        }

        private void FillAircraftSelection()
        {
            acListComboBox.Items.Clear();
            acListComboBox.Items.AddRange(
                aircrafts.Aircrafts
                .Select(c => c.Config.AC)
                .ToArray());
        }

        private void SetDefaultState()
        {
            routeDisLbl.Text = "";
            FinalReserveTxtBox.Text = "30";
            ContPercentComboBox.Text = "5";
            extraFuelTxtBox.Text = "0";
            ApuTimeTxtBox.Text = "30";
            TaxiTimeTxtBox.Text = "20";
            HoldTimeTxtBox.Text = "0";
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

            routeToDest = new RouteGroup(result, tracksInUse);
            var route = routeToDest.Expanded;

            mainRouteRichTxtBox.Text = route.ToString(false, false);
            RouteDistanceDisplay.UpdateRouteDistanceLbl(routeDisLbl, route);
        }

        private void ExportRouteFiles(object sender, EventArgs e)
        {
            var cmds = appSettings.ExportCommands.Values;
            var writer = new FileExporter(
                routeToDest.Expanded, airportList, cmds);

            var reports = writer.Export();
            ShowReports(reports);
        }

        private void AnalyzeRouteClick(object sender, EventArgs e)
        {
            //TODO: Need better exception message for AUTO, RAND commands
            try
            {
                mainRouteRichTxtBox.Text = mainRouteRichTxtBox.Text.ToUpper();

                routeToDest =
                    new RouteGroup(
                        RouteAnalyzerFacade.AnalyzeWithCommands(
                            mainRouteRichTxtBox.Text,
                            origController.Icao,
                            origController.Rwy,
                            destController.Icao,
                            destController.Rwy,
                            appSettings.NavDataLocation,
                            airportList,
                            wptList),
                        tracksInUse);

                var route = routeToDest.Expanded;

                mainRouteRichTxtBox.Text = route.ToString(false, false);
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

        private void WtUnitChanged(object sender, EventArgs e)
        {
            var unit = WeightUnit;
            Oew.Unit = unit;
            Payload.Unit = unit;
            Zfw.Unit = unit;
            MissedApproach.Unit = unit;
            Extra.Unit = unit;
        }

        private void SetWeightController()
        {
            Oew = new WeightTextBoxController(oewTxtBox, oewLbl);
            Payload = new WeightTextBoxController(payloadTxtBox, payloadLbl);
            Zfw = new WeightTextBoxController(zfwTxtBox, zfwLbl);
            MissedApproach = new WeightTextBoxController(
                missedAppFuelTxtBox, missedAppLbl);
            Extra = new WeightTextBoxController(extraFuelTxtBox, extraFuelLbl);

            weightControl = new WeightController(
                Oew, Payload, Zfw, payloadTrackBar);
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

        /// <summary>
        /// Gets the fuel data of currently selected aircraft.
        /// Returns null if no aircraft exists in ComboBox.
        /// </summary>
        /// <returns></returns>
        public FuelDataItem GetFuelData()
        {
            if (registrationComboBox.SelectedIndex < 0)
            {
                return null;
            }

            var ac = aircrafts.Find(registrationComboBox.Text);
            var dataName = ac.Config.FuelProfile;
            return fuelData.First(d => d.ProfileName == dataName).Data;
        }

        private void Calculate(object sender, EventArgs e)
        {
            fuelReportTxtBox.ForeColor = Color.Black;
            fuelReportTxtBox.Text = "";

            var validator = new FuelParameterValidator(this);
            FuelParameters para = null;

            try
            {
                para = validator.Validate();
            }
            catch (InvalidUserInputException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            var data = GetFuelData();
            FuelReport fuelReport =
                    new FuelCalculatorWithWind(data, para, windTables)
                    .Compute(routeToDest.Expanded, new Route[] { });

            if (fuelReport.TotalFuelKG > data.MaxFuelKg)
            {
                MessageBox.Show(InsufficientFuelMsg(
                    fuelReport.TotalFuelKG, data.MaxFuelKg, WeightUnit));
                return;
            }

            string outputText = fuelReport.ToString(WeightUnit);

            fuelReportTxtBox.Text = "\n" + outputText.ShiftToRight(20);
            //formStateManagerFuel.Save();

            //send weights to takeoff/ldg calc form 
            //AC_Req = ACList.Text;
            //TOWT_Req_Unit = parameters.WtUnit;
            //TODO:        LDG_fuel_prediction_unit = Parameters.WtUnit();

            //TOWT_Req = Convert.ToInt32(parameters.Zfw + fuelCalcResult.TakeoffFuelKg * (parameters.WtUnit == //WeightUnit.KG ? 1.0 : KgLbRatio));
            //TODO:       LDG_ZFW = Convert.ToInt32(Parameters.Zfw);
            //TODO:  LDG_fuel_prediction = Convert.ToInt32(fuelCalcResult.LdgFuelKgPredict * (Parameters.WtUnit() == WeightUnit.KG ? 1.0 : KG_LB));
        }

        private static string InsufficientFuelMsg(
            double fuelReqKG, double fuelCapacityKG, WeightUnit unit)
        {
            int fuelReqInt, fuelCapacityInt;
            string wtUnit = WeightUnitToString(unit);

            if (unit == WeightUnit.KG)
            {
                fuelReqInt = RoundToInt(fuelReqKG);
                fuelCapacityInt = RoundToInt(fuelCapacityKG);
            }
            else // WeightUnit.LB
            {
                fuelReqInt = RoundToInt(fuelReqKG * KgLbRatio);
                fuelCapacityInt = RoundToInt(fuelCapacityKG * KgLbRatio);
            }

            return "Insufficient fuel\n" +
                $"Fuel required for this flight is {fuelReqKG} {wtUnit}. " +
                $"Maximum fuel tank capacity is {fuelCapacityKG} {wtUnit}.";
        }

        private void findAltnBtn_Click(object sender, EventArgs e)
        {
            altnControl.AddRow();
        }
    }
}
