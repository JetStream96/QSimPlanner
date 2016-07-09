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

        private void SubscribeEventHandlers()
        {
            wtUnitComboBox.SelectedIndexChanged += WtUnitChanged;
            acListComboBox.SelectedIndexChanged += RefreshRegistrations;
            registrationComboBox.SelectedIndexChanged += RegistrationChanged;
            findRouteBtn.Click += FindRouteClick;
            analyzeRouteBtn.Click += AnalyzeRouteClick;
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

        private void SetDefaultState()
        {
            routeDisLbl.Text = "";
            finalReserveTxtBox.Text = "30";
            contPercentComboBox.Text = "5";
            extraFuelTxtBox.Text = "0";
            apuTimeTxtBox.Text = "30";
            taxiTimeTxtBox.Text = "20";
            holdTimeTxtBox.Text = "0";
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

        private void AnalyzeRouteClick(object sender, EventArgs e)
        {
            //TODO: Need better exception message for AUTO, RAND commands
            try
            {
                mainRouteRichTxtBox.Text = mainRouteRichTxtBox.Text.ToUpper();

                mainRoute =
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

                var route = mainRoute.Expanded;

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

        private FuelDataItem GetFuelData()
        {
            if(registrationComboBox.SelectedIndex < 0)
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

            var parameters = new FuelCalculationParameters();
            parameters.FillInDefaultValueIfLeftBlank();

            try
            {
                parameters.ImportValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            var data = GetFuelData();
            FuelReportResult fuelCalcResult = null;

            try
            {
                fuelCalcResult = ComputeFuelIteration(parameters, data, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            if (fuelCalcResult.TotalFuelKG > data.MaxFuelKg)
            {
                MessageBox.Show(InsufficientFuelMsg(fuelCalcResult.TotalFuelKG, data.MaxFuelKg, parameters.WtUnit));
                return;
            }

            string outputText = fuelCalcResult.ToString(parameters.WtUnit);

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

        private static string InsufficientFuelMsg(double fuelReqKG, double fuelCapacityKG, WeightUnit unit)
        {
            if (unit == WeightUnit.KG)
            {
                return "Insufficient fuel" + Environment.NewLine + "Fuel required for this flight is " + fuelReqKG + " KG. Maximum fuel tank capacity is " + fuelCapacityKG + " KG.";
            }
            else
            {
                return "Insufficient fuel" + Environment.NewLine + "Fuel required for this flight is " + Math.Round(fuelReqKG * KgLbRatio) + " LB. Maximum fuel tank capacity is " + Math.Round(fuelCapacityKG * KgLbRatio) + " LB.";
            }
        }

        // TODO: Shouldn't be here. Extract to another class.
        public FuelReportResult ComputeFuelIteration(
            FuelCalculationParameters para, 
            FuelDataItem data, 
            uint precisionLevel)
        {
            //presisionLevel = 0, 1, 2, ... 
            //smaller num = less precise
            //0 = disregard wind completely, 1 is good enough

            var FuelCalc = new FuelCalculator(para, data);
            var optCrz = data.OptCrzTable;
            var speedProfile = data.SpeedProfile;

            //calculate altn first
            double fuelTon = 0;
            double avgWeightTon = 0;
            double crzAltFt = 0;
            int tailwind = 0;
            double tas = 0;

            for (uint i = 0; i <= precisionLevel; i++)
            {
                FuelCalc.ReCompute();
                var result = FuelCalc.GetBriefResult();
                fuelTon = result.FuelToAltnTon;
                avgWeightTon = result.LandWeightTonAltn + fuelTon / 2;
                crzAltFt = optCrz.ActualCrzAltFt(avgWeightTon, para.DisToAltn);
                tas = speedProfile.CruiseTasKnots(crzAltFt);
                tailwind = ComputeTailWind(TailWindCalcOptions.DestToAltn, Convert.ToInt32(tas), Convert.ToInt32(crzAltFt / 100));
                para.AvgWindToAltn = tailwind;

                Debug.WriteLine("TO ALTN, CRZ ALT {0} FT, TAS {1} KTS, TAILWIND {2} KTS", crzAltFt, tas, tailwind);
            }

            for (uint i = 0; i <= precisionLevel; i++)
            {
                FuelCalc.ReCompute();
                var result = FuelCalc.GetBriefResult();
                fuelTon = result.FuelToDestTon;
                avgWeightTon = result.LandWeightTonDest + fuelTon / 2;
                crzAltFt = optCrz.ActualCrzAltFt(avgWeightTon, para.DisToDest);
                tas = speedProfile.CruiseTasKnots(crzAltFt);
                tailwind = ComputeTailWind(TailWindCalcOptions.OrigToDest, Convert.ToInt32(tas), Convert.ToInt32(crzAltFt / 100));
                para.AvgWindToDest = tailwind;

                Debug.WriteLine("TO DEST, CRZ ALT {0} FT, TAS {1} KTS, TAILWIND {2} KTS", crzAltFt, tas, tailwind);
            }

            FuelCalc.ReCompute();
            return FuelCalc.GetFullResult();
        }

        private int ComputeTailWind(TailWindCalcOptions para, int tas, int Fl)
        {
            if (para == TailWindCalcOptions.OrigToDest)
            {
                return WindAloft.Utilities.AvgTailWind(windTables, mainRoute.Expanded, Fl, tas);
            }
            else
            {
                // TODO: wrong.
                return WindAloft.Utilities.AvgTailWind(windTables, mainRoute.Expanded, Fl, tas);
            }
        }

        public enum TailWindCalcOptions
        {
            OrigToDest,
            DestToAltn
        }
    }
}
