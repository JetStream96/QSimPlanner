using QSP.AircraftProfiles.Configs;
using QSP.Common;
using QSP.Common.Options;
using QSP.FuelCalculation.Calculations;
using QSP.FuelCalculation.FuelData;
using QSP.FuelCalculation.Results;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.UI.Controllers.Units;
using QSP.UI.Controllers.WeightControl;
using QSP.UI.Models.FuelPlan;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Models.MsgBox;
using QSP.UI.Presenters.FuelPlan;
using QSP.UI.UserControls;
using QSP.UI.UserControls.TakeoffLanding.Common;
using QSP.UI.Util;
using QSP.UI.Util.ScrollBar;
using QSP.UI.Views.FuelPlan.Routes;
using QSP.Utilities.Units;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Numbers;
using static QSP.UI.Util.MsgBoxHelper;
using static QSP.UI.Views.Factories.FormFactory;
using static QSP.Utilities.LoggerInstance;
using static QSP.Utilities.Units.Conversions;

namespace QSP.UI.Views.FuelPlan
{
    // The implementation of ISupportActionContextMenu is used to support the actions 
    // for the route from origin to destination.

    public partial class FuelPlanningControl : UserControl, IRefreshForNavDataChange
    {
        public event EventHandler OrigIcaoChanged;
        public event EventHandler DestIcaoChanged;

        private FuelPlanningModel model;
        private RouteFinderControl advancedRouteTool;
        private ExportMenu exportMenu;

        public IRouteFinderView RouteFinderView => routeFinderControl;

        public WeightController WeightControl { get; private set; }
        public WeightTextBoxController Extra { get; private set; }
        public AlternatePresenter AltnPresenter { get; private set; }

        // Do not set the values of these controllers directly. 
        // Use WeightControl to interact with the weights.
        private WeightTextBoxController oew;
        private WeightTextBoxController payload;
        public WeightTextBoxController Zfw { get; private set; }

        /// <summary>
        /// After the fuel calculation completes, the user can press 'request' button
        /// in takeoff or landing calculation page to automatically fill some paramters of the 
        /// selected aircraft. 
        /// 
        /// Returns null if not available.
        /// </summary>
        public AircraftRequest AircraftRequest { get; private set; }

        public event EventHandler AircraftRequestChanged;

        public WeightUnit WeightUnit
        {
            get => (WeightUnit)wtUnitComboBox.SelectedIndex;

            set => wtUnitComboBox.SelectedIndex = (int)value;
        }

        public string OrigIcao => routeFinderControl.OrigIcao;
        public string DestIcao => routeFinderControl.DestIcao;
        public string OrigRwy => routeFinderControl.OrigRwy;
        public string DestRwy => routeFinderControl.DestRwy;

        private AirportManager AirportList => model.AirwayNetwork.AirportList;
        private WaypointList WptList => model.AirwayNetwork.WptList;
        private AppOptions AppOptions => model.AppOption.Instance;
        private RouteGroup RouteToDest => routeFinderControl.RouteGroup;

        public FuelPlanningControl()
        {
            InitializeComponent();
        }

        public void Init(FuelPlanningModel model)
        {
            this.model = model;

            SetDefaultState();
            SetOrigDestEvents();

            var routeFinderModel = new RouteFinderModel()
            {
                FuelPlanningModel = model,
                WindCalc = () => model.GetWindCalculator(this)
            };

            exportMenu = new ExportMenu();
            exportMenu.Init(model.AppOption);

            routeFinderControl.Init(routeFinderModel, ParentForm, exportMenu);

            AltnPresenter = new AlternatePresenter(
                alternateControl, model.AppOption, model.AirwayNetwork, model.WindTables,
                routeFinderControl.DestSidProvider, GetFuelData, GetZfwTon,
                () => OrigIcao, () => DestIcao, exportMenu);
            alternateControl.Init(AltnPresenter);

            SetWeightController();
            SetAircraftSelection();

            wtUnitComboBox.SelectedIndex = 0;
            SubscribeEventHandlers();

            advancedRouteTool = new RouteFinderControl();
            advancedRouteTool.Init(routeFinderModel, ParentForm, exportMenu);

            if (acListComboBox.Items.Count > 0) acListComboBox.SelectedIndex = 0;

            AltnEnabledCheckBox.Checked = true;

            LoadSavedState();
        }

        private void SubscribeEventHandlers()
        {
            wtUnitComboBox.SelectedIndexChanged += WtUnitChanged;
            acListComboBox.SelectedIndexChanged += RefreshRegistrations;
            registrationComboBox.SelectedIndexChanged += RegistrationChanged;
            calculateBtn.Click += Calculate;
            advancedToolLbl.Click += ShowAdvancedTool;
        }

        private string[] AvailAircraftTypes()
        {
            var allProfileNames = model.FuelData.Select(t => t.ProfileName).ToHashSet();

            return model.Aircrafts
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
                Log(ex);
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
                Log(ex);
            }
        }

        private void SetOrigDestEvents()
        {
            routeFinderControl.OrigIcaoChanged += (s, e) => OrigIcaoChanged?.Invoke(s, e);
            routeFinderControl.DestIcaoChanged += (s, e) => DestIcaoChanged?.Invoke(s, e);
        }

        private void WtUnitChanged(object sender, EventArgs e)
        {
            var unit = WeightUnit;
            oew.Unit = unit;
            payload.Unit = unit;
            Zfw.Unit = unit;
            Extra.Unit = unit;
        }

        private void SetWeightController()
        {
            oew = new WeightTextBoxController(oewTxtBox, oewLbl);
            payload = new WeightTextBoxController(payloadTxtBox, payloadLbl);
            Zfw = new WeightTextBoxController(zfwTxtBox, zfwLbl);
            Extra = new WeightTextBoxController(extraFuelTxtBox, extraFuelLbl);

            WeightControl = new WeightController(oew, payload, Zfw, payloadTrackBar);
            WeightControl.Enable();
        }

        private bool FuelProfileExists(string profileName)
        {
            return model.FuelData.Any(c => c.ProfileName == profileName);
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac = model.Aircrafts.FindAircraft(acListComboBox.Text);
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

            var config = model.Aircrafts.Find(registrationComboBox.Text).Config;
            WeightUnit = config.WtUnit;
            WeightControl.SetAircraftWeights(config.OewKg, config.MaxZfwKg);
            var maxPayloadKg = config.MaxZfwKg - config.OewKg;
            WeightControl.ZfwKg = config.OewKg + 0.5 * maxPayloadKg;
        }

        /// <summary>
        /// Gets the fuel data of currently selected aircraft.
        /// Returns null if no aircraft exists in ComboBox.
        /// </summary>
        public FuelDataItem GetFuelData()
        {
            var dataName = GetCurrentAircraft().Config.FuelProfile;
            return model.FuelData.First(d => d.ProfileName == dataName).Data;
        }

        /// <summary>
        /// Returns null if no aircraft exists in ComboBox.
        /// </summary>
        public AircraftConfig GetCurrentAircraft()
        {
            if (registrationComboBox.SelectedIndex < 0) return null;
            return model.Aircrafts.Find(registrationComboBox.Text);
        }

        // Returns null and shows a dialog if user input is not valid.
        // Returns empty list if alternates are not enabled.
        private List<Route> GetExpandedAlternateRoutes()
        {
            if (!AltnEnabledCheckBox.Checked) return new List<Route>();

            var altnRoutes = AltnPresenter.Routes;

            if (altnRoutes.Any(r => r == null))
            {
                ShowMessage("All alternate routes must be entered.", MessageLevel.Warning);
                return null;
            }

            return altnRoutes.Select(r => r.Expanded).ToList();
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
                ShowMessage(ex.Message, MessageLevel.Warning);
                return;
            }

            var altnRoutes = GetExpandedAlternateRoutes();
            if (altnRoutes == null) return;

            if (RouteToDest == null)
            {
                ShowMessage("Route to destination must be entered.", MessageLevel.Warning);
                return;
            }

            var windTables = model.WindTables.Instance;

            if (windTables is DefaultWxTableCollection)
            {
                var result = this.ShowDialog(
                    "The wind data has not been downloaded. " +
                    "Continue to calculate and ignore wind aloft?",
                    MsgBoxIcon.Info,
                    "",
                    DefaultButton.Button1,
                    "Yes", "No", "Cancel");

                if (result != MsgBoxResult.Button1) return;
            }

            FuelReport fuelReport = null;

            try
            {
                fuelReport = new FuelReportGenerator(
                    AirportList,
                    new BasicCrzAltProvider(),
                    windTables,
                    RouteToDest.Expanded,
                    altnRoutes,
                    para).Generate();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageLevel.Warning);
                return;
            }

            var ac = GetCurrentAircraft().Config;

            if (fuelReport.TotalFuel > ac.MaxFuelKg)
            {
                var msg = InsufficientFuelMsg(fuelReport.TotalFuel, ac.MaxFuelKg, WeightUnit);
                this.ShowInfo(msg, "Insufficient fuel");
                return;
            }

            string outputText = fuelReport.ToString(WeightUnit);
            fuelReportTxtBox.Text = "\n" + outputText.ShiftToRight(30);

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
            double fuelReqKg, double fuelCapacityKg, WeightUnit unit)
        {
            int fuelReqInt, fuelCapacityInt;
            string wtUnit = WeightUnitToString(unit);

            if (unit == WeightUnit.KG)
            {
                fuelReqInt = RoundToInt(fuelReqKg);
                fuelCapacityInt = RoundToInt(fuelCapacityKg);
            }
            else // WeightUnit.LB
            {
                fuelReqInt = RoundToInt(fuelReqKg * KgLbRatio);
                fuelCapacityInt = RoundToInt(fuelCapacityKg * KgLbRatio);
            }

            return
                $"Fuel required for this flight is {fuelReqInt} {wtUnit}. " +
                $"Maximum fuel tank capacity is {fuelCapacityInt} {wtUnit}.";
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

        /// <exception cref="InvalidOperationException"></exception>
        public double GetZfwTon()
        {
            try
            {
                return Zfw.GetWeightKg() / 1000.0;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidUserInputException("Please enter a valid ZFW.");
            }
        }

        public void OnNavDataChange()
        {
            routeFinderControl.OnNavDataChange();
            AltnPresenter.OnNavDataChange();
            advancedRouteTool.OnNavDataChange();
        }

        public void ShowMessage(string s, MessageLevel lvl) => ParentForm.ShowMessage(s, lvl);

        private void altnEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            alternateControl.Enabled = AltnEnabledCheckBox.Checked;
        }
    }
}
