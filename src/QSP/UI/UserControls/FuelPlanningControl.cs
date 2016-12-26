using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.FuelCalculation.Calculations;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.UserControls.RouteActions;
using QSP.UI.Utilities;
using QSP.Utilities.Units;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QSP.FuelCalculation.Results;
using static QSP.AviationTools.Constants;
using static QSP.AviationTools.SpeedConversion;
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
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private ProcedureFilter procFilter;
        private Locator<IWindTableCollection> windTableLocator;
        private Locator<CountryCodeManager> countryCodeLocator;
        private Locator<CountryCodeCollection> checkedCodesLocator;

        private RouteFinderSelection origController;
        private RouteFinderSelection destController;
        private DestinationSidSelection destSidProvider;
        public WeightController WeightControl { get; private set; }
        private AdvancedRouteTool advancedRouteTool;
        private AcConfigManager aircrafts;
        private IEnumerable<FuelData> fuelData;
        private ActionContextMenu routeActionMenu;
        private RouteOptionContextMenu routeOptionMenu;

        public AlternateController AltnControl { get; private set; }
        public WeightTextBoxController MissedApproach { get; private set; }
        public WeightTextBoxController Extra { get; private set; }

        // Do not set the values of these controllers directly. 
        // Use WeightControl to interact with the weights.
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        public WeightTextBoxController Zfw { get; private set; }

        /// <summary>
        /// Returns null if not available.
        /// </summary>
        public AircraftRequest AircraftRequest { get; private set; }

        public event EventHandler AircraftRequestChanged;

        private AirportManager airportList => airwayNetwork.AirportList;
        private AppOptions appSettings => appOptionsLocator.Instance;
        private RouteGroup RouteToDest => routeActionMenu.Route;

        public FuelPlanningControl()
        {
            InitializeComponent();
        }

        public void Init(
            Locator<AppOptions> appOptionsLocator,
            AirwayNetwork airwayNetwork,
            ProcedureFilter procFilter,
            Locator<CountryCodeManager> countryCodeLocator,
            Locator<IWindTableCollection> windTableLocator,
            AcConfigManager aircrafts,
            IEnumerable<FuelData> fuelData)
        {
            this.appOptionsLocator = appOptionsLocator;
            this.airwayNetwork = airwayNetwork;
            this.procFilter = procFilter;
            this.countryCodeLocator = countryCodeLocator;
            this.windTableLocator = windTableLocator;
            this.aircrafts = aircrafts;
            this.fuelData = fuelData;
            checkedCodesLocator = new CountryCodeCollection().ToLocator();

            SetDefaultState();
            SetOrigDestControllers();
            SetAltnController();
            SetRouteOptionControl();
            SetRouteActionControl();
            SetWeightController();
            SetAircraftSelection();
            SetBtnColorStyles();

            wtUnitComboBox.SelectedIndex = 0;
            SubscribeEventHandlers();
            advancedRouteTool = new AdvancedRouteTool();
            advancedRouteTool.Init(
                appOptionsLocator,
                airwayNetwork,
                countryCodeLocator,
                checkedCodesLocator,
                procFilter,
                countryCodeLocator.Instance,
                () => GetWindCalculator());

            if (acListComboBox.Items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }

            LoadSavedState();
        }

        private void SetBtnColorStyles()
        {
            var style = new ControlDisableStyleController.ColorStyle(
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            var removeBtnStyle = new ControlDisableStyleController(removeAltnBtn, style);

            var filterSidStyle = new ControlDisableStyleController(filterSidBtn, style);

            var filterStarStyle = new ControlDisableStyleController(filterStarBtn, style);

            removeBtnStyle.Activate();
            filterSidStyle.Activate();
            filterStarStyle.Activate();
        }

        private void SetRouteOptionControl()
        {
            routeOptionMenu = new RouteOptionContextMenu(checkedCodesLocator, countryCodeLocator);

            routeOptionMenu.Subscribe();
            routeOptionBtn.Click += (s, e) =>
                routeOptionMenu.Show(routeOptionBtn, new Point(0, routeOptionBtn.Height));
        }

        private void SetRouteActionControl()
        {
            routeActionMenu = new ActionContextMenu(
                appOptionsLocator,
                airwayNetwork,
                origController,
                destController,
                checkedCodesLocator,
                () => GetWindCalculator(),
                routeDisLbl,
                DistanceDisplayStyle.Long,
                () => mainRouteRichTxtBox.Text,
                s => mainRouteRichTxtBox.Text = s,
                ParentForm);

            routeActionMenu.Subscribe();
            showRouteActionsBtn.Click += (s, e) =>
               routeActionMenu.Show(showRouteActionsBtn, new Point(0, showRouteActionsBtn.Height));
        }

        public WeightUnit WeightUnit
        {
            get { return (WeightUnit)wtUnitComboBox.SelectedIndex; }

            set { wtUnitComboBox.SelectedIndex = (int)value; }
        }

        public void OnWptListChanged()
        {
            advancedRouteTool.OnWptListChanged();
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

            AltnControl = new AlternateController(
                alternateGroupBox,
                appOptionsLocator,
                airwayNetwork,
                altnLayoutPanel,
                destSidProvider,
                () => GetWindCalculator());

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
            AltnControl.RowCountChanged += RowCountChanged;
            advancedToolLbl.Click += ShowAdvancedTool;
        }

        private string[] AvailAircraftTypes()
        {
            var allProfileNames = fuelData.Select(t => t.ProfileName).ToHashSet();

            return aircrafts
                .Aircrafts
                .Where(c => allProfileNames.Contains(c.Config.FuelProfile))
                .Select(c => c.Config.AC)
                .Distinct()
                .OrderBy(i => i)
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

        public void SaveStateToFile()
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
               appOptionsLocator,
               () => airwayNetwork.AirportList,
               () => airwayNetwork.WptList,
               procFilter);

            destController = new RouteFinderSelection(
                destTxtBox,
                false,
                destRwyComboBox,
                starComboBox,
                filterStarBtn,
                appOptionsLocator,
                () => airwayNetwork.AirportList,
                () => airwayNetwork.WptList,
                procFilter);

            destSidProvider = new DestinationSidSelection(destController);

            origController.Subscribe();
            destController.Subscribe();
        }

        private void WtUnitChanged(object sender, EventArgs e)
        {
            var unit = WeightUnit;
            oew.Unit = unit;
            payload.Unit = unit;
            Zfw.Unit = unit;
            MissedApproach.Unit = unit;
            Extra.Unit = unit;
        }

        private void SetWeightController()
        {
            oew = new WeightTextBoxController(oewTxtBox, oewLbl);
            payload = new WeightTextBoxController(payloadTxtBox, payloadLbl);
            Zfw = new WeightTextBoxController(zfwTxtBox, zfwLbl);
            MissedApproach = new WeightTextBoxController(missedAppFuelTxtBox, missedAppLbl);
            Extra = new WeightTextBoxController(extraFuelTxtBox, extraFuelLbl);

            WeightControl = new WeightController(oew, payload, Zfw, payloadTrackBar);
            WeightControl.Enable();
        }

        private bool FuelProfileExists(string profileName)
        {
            return fuelData.Any(c => c.ProfileName == profileName);
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac = aircrafts.FindAircraft(acListComboBox.Text);
                var items = registrationComboBox.Items;
                items.Clear();

                items.AddRange(
                    ac.Where(c => FuelProfileExists(c.Config.FuelProfile))
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
            if (registrationComboBox.SelectedIndex < 0) return;

            var config = aircrafts.Find(registrationComboBox.Text).Config;
            WeightUnit = config.WtUnit;
            WeightControl.AircraftConfig = config;
            var maxPayloadKg = config.MaxZfwKg - config.OewKg;
            WeightControl.ZfwKg = config.OewKg + 0.5 * maxPayloadKg;

            MissedApproach.SetWeight(GetFuelData().MissedAppFuel);
        }

        /// <summary>
        /// Gets the fuel data of currently selected aircraft.
        /// Returns null if no aircraft exists in ComboBox.
        /// </summary>
        public FuelDataItem GetFuelData()
        {
            var dataName = GetCurrentAircraft().Config.FuelProfile;
            return fuelData.First(d => d.ProfileName == dataName).Data;
        }

        /// <summary>
        /// Returns null if no aircraft exists in ComboBox.
        /// </summary>
        public AircraftConfig GetCurrentAircraft()
        {
            if (registrationComboBox.SelectedIndex < 0) return null;
            return aircrafts.Find(registrationComboBox.Text);
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
                ShowWarning(ex.Message);
                return;
            }

            var data = GetFuelData();
            var altnRoutes = AltnControl.Routes;

            if (altnRoutes.Any(r => r == null))
            {
                ShowWarning("All alternate routes must be entered.");
                return;
            }

            if (RouteToDest == null)
            {
                ShowWarning("Route to destination must be entered.");
                return;
            }

            var windTables = windTableLocator.Instance;

            if (windTables is DefaultWindTableCollection)
            {
                var result = MessageBox.Show(
                    "The wind data has not been downloaded. " +
                    "Continue to calculate and ignore wind aloft?",
                    "",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;
            }

            var fuelReport = new FuelReportGenerator(
                airportList,
                new BasicCrzAltProvider(),
                windTables,
                RouteToDest.Expanded,
                altnRoutes,
                para).Generate();

            var ac = GetCurrentAircraft().Config;

            if (fuelReport.TotalFuel > ac.MaxFuelKg)
            {
                var msg = InsufficientFuelMsg(
                    fuelReport.TotalFuel, ac.MaxFuelKg, WeightUnit);

                MessageBox.Show(
                    msg,
                    "Insufficient fuel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            string outputText = fuelReport.ToString(WeightUnit);
            fuelReportTxtBox.Text = "\n" + outputText.ShiftToRight(20);

            AircraftRequest = new AircraftRequest(
                acListComboBox.Text,
                registrationComboBox.Text,
                para.Zfw + fuelReport.TakeoffFuel,
                para.Zfw + fuelReport.PredictedLdgFuel,
                para.Zfw,
                WeightUnit);

            AircraftRequestChanged?.Invoke(this, EventArgs.Empty);
            SaveStateToFile();
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            var panel = this.FindPanel().FindPanel();
            var scroll = panel.VerticalScroll;
            var target = 1 + scroll.Maximum - scroll.LargeChange;
            ScrollAnimation.ScrollToPosition(scroll, target, 500.0);
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

            return
                $"Fuel required for this flight is {fuelReqInt} {wtUnit}. " +
                $"Maximum fuel tank capacity is {fuelCapacityInt} {wtUnit}.";
        }

        private void AddAltn(object sender, EventArgs e)
        {
            AltnControl.AddRow();
        }

        private void RemoveAltn(object sender, EventArgs e)
        {
            AltnControl.RemoveLastRow();
        }

        private void RowCountChanged(object sender, EventArgs e)
        {
            removeAltnBtn.Enabled = AltnControl.RowCount > 1;
        }

        private void ShowAdvancedTool(object sender, EventArgs e)
        {
            var size = advancedRouteTool.Size;
            var newSize = new Size(size.Width + 25, size.Height + 40);

            using (var frm = GetForm(newSize))
            {
                frm.Owner = this.ParentForm;
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.Controls.Add(advancedRouteTool);

                // Remove control from form so that it is not disposed
                // when form closes.
                frm.FormClosing += (_s, _e) => frm.Controls.Remove(advancedRouteTool);

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

        /// <summary>
        /// Get AvgWindCalculator to approximate the wind.
        /// Returns null if user disabled wind optimization.
        /// </summary>
        /// <exception cref="InvalidUserInputException"></exception>
        private AvgWindCalculator GetWindCalculator()
        {
            if (appSettings.EnableWindOptimizedRoute == false) return null;

            if (windTableLocator.Instance is DefaultWindTableCollection)
            {
                throw new InvalidUserInputException(
                    "Wind data has not been downloaded or loaded from file.");
            }

            var fuelData = GetFuelData();
            if (fuelData == null)
            {
                throw new InvalidUserInputException("No aircraft is selected.");
            }

            double zfw = 0.0;
            try
            {
                zfw = Zfw.GetWeightKg() / 1000.0;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidUserInputException("Please enter a valid ZFW.");
            }

            var orig = airportList[origTxtBox.Text.Trim().ToUpper()];

            if (orig == null)
            {
                throw new InvalidUserInputException("Cannot find origin airport.");
            }

            var dest = airportList[destTxtBox.Text.Trim().ToUpper()];

            if (dest == null)
            {
                throw new InvalidUserInputException("Cannot find destination airport.");
            }

            var dis = orig.Distance(dest);
            var alt = fuelData.EstimatedCrzAlt(dis, zfw);
            var tas = Ktas(fuelData.CruiseKias(zfw), alt);

            return new AvgWindCalculator(windTableLocator.Instance, tas, alt);
        }

        public void RefreshForAirportListChange()
        {
            origController.RefreshRwyComboBox();
            destController.RefreshRwyComboBox();
            AltnControl.RefreshForAirportListChange();
        }

        public void RefreshForNavDataLocationChange()
        {
            origController.RefreshProcedureComboBox();
            destController.RefreshProcedureComboBox();
            AltnControl.RefreshForNavDataLocationChange();
        }
    }
}
