using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.Common.Options;
using QSP.FuelCalculation;
using QSP.FuelCalculation.Calculators;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;
using QSP.UI.ToLdgModule.Common;
using QSP.Utilities.Units;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Doubles;
using static QSP.UI.Factories.FormFactory;
using static QSP.UI.Utilities.MsgBoxHelper;
using static QSP.UI.Utilities.RouteDistanceDisplay;
using static QSP.Utilities.LoggerInstance;
using static QSP.Utilities.Units.Conversions;

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
        private Locator<WindTableCollection> windTableLocator;

        private RouteFinderSelection origController;
        private RouteFinderSelection destController;
        private DestinationSidSelection destSidProvider;
        private WeightController weightControl;
        private AdvancedRouteTool advancedRouteTool;
        private AcConfigManager aircrafts;
        private IEnumerable<FuelData> fuelData;

        public AlternateController altnControl { get; private set; }
        public WeightTextBoxController Oew { get; private set; }
        public WeightTextBoxController Payload { get; private set; }
        public WeightTextBoxController Zfw { get; private set; }
        public WeightTextBoxController MissedApproach { get; private set; }
        public WeightTextBoxController Extra { get; private set; }

        /// <summary>
        /// Returns null if not available.
        /// </summary>
        public AircraftRequest AircraftRequest { get; private set; }

        public event EventHandler AircraftRequestChanged;

        private RouteGroup RouteToDest
        {
            get
            {
                return routeOptionBtns.Route;
            }
        }

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
            Locator<WindTableCollection> windTableLocator,
            AcConfigManager aircrafts,
            IEnumerable<FuelData> fuelData)
        {
            this.appSettings = appSettings;
            this.wptList = wptList;
            this.airportList = airportList;
            this.tracksInUse = tracksInUse;
            this.procFilter = procFilter;
            this.countryCodes = countryCodes;
            this.windTableLocator = windTableLocator;
            this.aircrafts = aircrafts;
            this.fuelData = fuelData;

            SetDefaultState();
            SetOrigDestControllers();
            SetAltnController();
            SetRouteOptionControl();
            SetWeightController();
            SetAircraftSelection();

            wtUnitComboBox.SelectedIndex = 0;
            SubscribeEventHandlers();
            advancedRouteTool = new AdvancedRouteTool();
            advancedRouteTool.Init(
                appSettings,
                wptList,
                airportList,
                tracksInUse,
                procFilter,
                countryCodes);

            if (acListComboBox.Items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }

            LoadSavedState();
        }

        private void SetRouteOptionControl()
        {
            routeOptionBtns.Init(
                appSettings,
                wptList,
                airportList,
                tracksInUse,
                origController,
                destController,
                routeDisLbl,
                DistanceDisplayStyle.Long,
                () => mainRouteRichTxtBox.Text,
                s => mainRouteRichTxtBox.Text = s);

            routeOptionBtns.Subscribe();
        }

        public WeightUnit WeightUnit
        {
            get
            {
                return (WeightUnit)wtUnitComboBox.SelectedIndex;
            }

            set
            {
                wtUnitComboBox.SelectedIndex = (int)value;
            }
        }

        private void SetAltnController()
        {
            var controlsBelow = new Control[]
            {
                addAltnBtn,
                removeAltnBtn,
                calculateBtn,
                fuelParaGroupBox,
                fuelReportGroupBox
            };

            altnControl = new AlternateController(
                alternateGroupBox,
                appSettings,
                airportList,
                wptList,
                tracksInUse,
                altnLayoutPanel,
                destSidProvider);

            removeAltnBtn.Enabled = false;
            AddAltn(this, EventArgs.Empty);
        }

        private void SubscribeEventHandlers()
        {
            wtUnitComboBox.SelectedIndexChanged += WtUnitChanged;
            acListComboBox.SelectedIndexChanged += RefreshRegistrations;
            registrationComboBox.SelectedIndexChanged += RegistrationChanged;
            calculateBtn.Click += Calculate;
            addAltnBtn.Click += AddAltn;
            removeAltnBtn.Click += RemoveAltn;
            altnControl.RowCountChanged += RowCountChanged;
            advancedToolLbl.Click += ShowAdvancedTool;
        }

        private string[] AvailAircraftTypes()
        {
            var allProfileNames = fuelData.Select(t => t.ProfileName);

            return aircrafts
                .Aircrafts
                .Where(c => allProfileNames.Contains(c.Config.FuelProfile))
                .Select(c => c.Config.AC)
                .ToArray();
        }

        private void SetAircraftSelection()
        {
            acListComboBox.Items.Clear();
            acListComboBox.Items.AddRange(AvailAircraftTypes());
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

        private void LoadSavedState()
        {
            try
            {
                new FuelPageState(this).LoadFromFile();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }
        }

        private void SaveStateToFile()
        {
            try
            {
                new FuelPageState(this).SaveToFile();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }
        }

        private void SetOrigDestControllers()
        {
            origController = new RouteFinderSelection(
               origTxtBox,
               true,
               origRwyComboBox,
               sidComboBox,
               filterSidBtn,
               appSettings.NavDataLocation,
               airportList,
               wptList,
               procFilter);

            destController = new RouteFinderSelection(
                destTxtBox,
                false,
                destRwyComboBox,
                starComboBox,
                filterStarBtn,
                appSettings.NavDataLocation,
                airportList,
                wptList,
                procFilter);

            destSidProvider = new DestinationSidSelection(destController);

            origController.Subscribe();
            destController.Subscribe();
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

            var config = aircrafts.Find(registrationComboBox.Text).Config;
            weightControl.AircraftConfig = config;
            var maxPayloadKg = config.MaxZfwKg - config.OewKg;
            weightControl.ZfwKg = config.OewKg + 0.5 * maxPayloadKg;

            MissedApproach.SetWeight(GetFuelData().MissedAppFuelKG);
        }

        /// <summary>
        /// Gets the fuel data of currently selected aircraft.
        /// Returns null if no aircraft exists in ComboBox.
        /// </summary>
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
            SaveStateToFile();
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
                ShowWarning(ex.Message);
                return;
            }

            var data = GetFuelData();
            var altnRoutes = altnControl.Routes;

            if (altnRoutes.Any(r => r == null))
            {
                ShowWarning("All alternate routes must be entered.");
                return;
            }

            var windTables = windTableLocator.Instance;
            var fuelReport =
                new FuelCalculatorWithWind(data, para, windTables)
                .Compute(RouteToDest.Expanded, altnRoutes);

            if (fuelReport.TotalFuelKG > data.MaxFuelKg)
            {
                MessageBox.Show(InsufficientFuelMsg(
                    fuelReport.TotalFuelKG, data.MaxFuelKg, WeightUnit));
                return;
            }

            string outputText = fuelReport.ToString(WeightUnit);
            fuelReportTxtBox.Text = "\n" + outputText.ShiftToRight(20);

            AircraftRequest = new AircraftRequest(
                acListComboBox.Text,
                registrationComboBox.Text,
                para.ZfwKg + fuelReport.TakeoffFuelKg,
                para.ZfwKg + fuelReport.LdgFuelKgPredict,
                para.ZfwKg,
                WeightUnit);

            AircraftRequestChanged?.Invoke(this, EventArgs.Empty);

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

        private void AddAltn(object sender, EventArgs e)
        {
            altnControl.AddRow();
        }

        private void RemoveAltn(object sender, EventArgs e)
        {
            altnControl.RemoveLastRow();
        }

        private void RowCountChanged(object sender, EventArgs e)
        {
            removeAltnBtn.Enabled = altnControl.RowCount > 1;
        }

        private void ShowAdvancedTool(object sender, EventArgs e)
        {
            var advancedTool = new AdvancedRouteTool();
            var size = advancedRouteTool.Size;
            var newSize = new Size(size.Width + 25, size.Height + 40);

            using (var frm = GetForm(newSize))
            {
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.Controls.Add(advancedRouteTool);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// Refresh the aircraft and registration comboBoxes,
        /// after the AcConfigManager is updated.
        /// </summary>
        public void RefreshAircrafts(object sender, EventArgs e)
        {
            // Set the selected aircraft/registration.
            string ac = acListComboBox.Text;
            string reg = registrationComboBox.Text;

            SetAircraftSelection();

            if (acListComboBox.Items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }

            acListComboBox.Text = ac;
            registrationComboBox.Text = reg;
        }
    }
}
