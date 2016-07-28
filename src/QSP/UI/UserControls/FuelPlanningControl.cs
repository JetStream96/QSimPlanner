using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.Common.Options;
using QSP.FuelCalculation;
using QSP.FuelCalculation.Calculators;
using QSP.LibraryExtension;
using QSP.RouteFinding;
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
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private ProcedureFilter procFilter;
        private Locator<IWindTableCollection> windTableLocator;
        private Locator<CountryCodeManager> countryCodeLocator;
        private Locator<CountryCodeCollection> checkedCodesLocator;

        private RouteFinderSelection origController;
        private RouteFinderSelection destController;
        private DestinationSidSelection destSidProvider;
        private WeightController weightControl;
        private AdvancedRouteTool advancedRouteTool;
        private AcConfigManager aircrafts;
        private IEnumerable<FuelData> fuelData;
        private ActionContextMenu routeActionMenu;
        private RouteOptionContextMenu routeOptionMenu;

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

        private AirportManager airportList
        {
            get { return airwayNetwork.AirportList; }
        }

        private AppOptions appSettings
        {
            get { return appOptionsLocator.Instance; }
        }

        private RouteGroup RouteToDest
        {
            get { return routeActionMenu.Route; }
        }

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
            var removeBtnStyle = new ControlDisableStyleController(
                removeAltnBtn,
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            removeBtnStyle.Activate();

            var filterSidStyle = new ControlDisableStyleController(
                filterSidBtn,
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            filterSidStyle.Activate();

            var filterStarStyle = new ControlDisableStyleController(
                filterStarBtn,
                Color.DarkSlateGray,
                Color.FromArgb(224, 224, 224),
                Color.White,
                Color.LightGray);

            filterStarStyle.Activate();
        }

        private void SetRouteOptionControl()
        {
            routeOptionMenu = new RouteOptionContextMenu(
                checkedCodesLocator, countryCodeLocator);

            routeOptionMenu.Subscribe();
            routeOptionBtn.Click += (s, e) =>
            routeOptionMenu.Show(routeOptionBtn, new Point(20, 30));
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
                s => mainRouteRichTxtBox.Text = s);

            routeActionMenu.Subscribe();
            showRouteActionsBtn.Click += (s, e) =>
            routeActionMenu.Show(showRouteActionsBtn, new Point(20, 30));
        }

        public WeightUnit WeightUnit
        {
            get { return (WeightUnit)wtUnitComboBox.SelectedIndex; }

            set { wtUnitComboBox.SelectedIndex = (int)value; }
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
                return;
            }

            var config = aircrafts.Find(registrationComboBox.Text).Config;
            WeightUnit = config.WtUnit;
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

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

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
            SaveStateToFile();
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            var panel = ParentControlFinder.FindPanel(this);
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
            var size = advancedRouteTool.Size;
            var newSize = new Size(size.Width + 25, size.Height + 40);

            using (var frm = GetForm(newSize))
            {
                frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frm.Controls.Add(advancedRouteTool);

                // Remove control from form so that it is not disposed
                // when form closes.
                frm.FormClosing +=
                    (_s, _e) => frm.Controls.Remove(advancedRouteTool);

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
                throw new InvalidUserInputException(
                    "No aircraft is selected.");
            }

            double zfw = 0.0;
            try
            {
                zfw = Zfw.GetWeightKg() / 1000.0;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidUserInputException(
                    "Please enter a valid ZFW.");
            }

            var orig = airportList.Find(origTxtBox.Text.Trim().ToUpper());

            if (orig == null)
            {
                throw new InvalidUserInputException(
                    "Cannot find origin airport.");
            }

            var dest = airportList.Find(destTxtBox.Text.Trim().ToUpper());

            if (dest == null)
            {
                throw new InvalidUserInputException(
                    "Cannot find destination airport.");
            }

            var dis = orig.Distance(dest);

            var fuelTon =
                fuelData.FuelTable.GetFuelRequiredTon(dis, zfw / 1000.0);

            var avgWtTon = zfw + 0.5 * fuelTon;
            var altitude = fuelData.OptCrzTable.ActualCrzAltFt(avgWtTon, dis);
            var tas = fuelData.SpeedProfile.CruiseTasKnots(altitude);

            return new AvgWindCalculator(
                windTableLocator.Instance, tas, altitude);
        }

        public void RefreshForAirportListChange()
        {
            origController.RefreshRwyComboBox();
            destController.RefreshRwyComboBox();
            altnControl.RefreshForAirportListChange();
        }

        public void RefreshForNavDataLocationChange()
        {
            origController.RefreshProcedureComboBox();
            destController.RefreshProcedureComboBox();
            altnControl.RefreshForNavDataLocationChange();
        }
    }
}
