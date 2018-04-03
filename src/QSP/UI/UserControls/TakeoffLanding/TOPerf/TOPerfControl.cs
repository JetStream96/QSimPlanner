using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Common;
using QSP.LibraryExtension;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using QSP.TOPerfCalculation.Boeing.PerfData;
using QSP.UI.Models.MsgBox;
using QSP.UI.Models.TakeoffLanding;
using QSP.UI.UserControls.TakeoffLanding.Common;
using QSP.UI.UserControls.TakeoffLanding.TOPerf.Controllers;
using QSP.UI.Util;
using QSP.UI.Views.Factories;
using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.TOPerf
{
    public partial class TOPerfControl : UserControl
    {
        public const string FileName = "TakeoffPerfControl.xml";

        private IFormController controller;
        private TOPerfElements elements;
        private AcConfigManager aircrafts;
        private List<PerfTable> tables;
        private Func<AircraftRequest> acRequestGetter;

        private PerfTable currentTable;
        private AutoWeatherSetter wxSetter;
        private bool updatingFormOptions = false;

        public AirportManager Airports
        {
            get { return airportInfoControl.Airports; }
            set
            {
                airportInfoControl.Airports = value;
                airportInfoControl.RefreshAirportInfo();
            }
        }

        public TOPerfControl()
        {
            InitializeComponent();
        }

        public void Init(
            AcConfigManager aircrafts,
            List<PerfTable> tables,
            AirportManager airports,
            Func<AircraftRequest> acRequestGetter)
        {
            InitControls();

            this.aircrafts = aircrafts;
            this.tables = tables;
            UpdateAircraftList();
            this.Airports = airports;
            this.acRequestGetter = acRequestGetter;
        }

        private void InitControls()
        {
            airportInfoControl.Init();

            // Create the reference to the UI controls.
            InitializeElements();

            // Set default values for the controls.
            InitializeControls();

            SetWeatherBtnHandlers();
            requestBtn.SetToolTip("Use aircraft and weights calculated from 'Fuel' page.");

            // Automatically update weather
            airportInfoControl.IcaoChanged += (s, e) => wxSetter.GetMetarAndFillWeather();
            wtUnitComboBox.SelectedIndexChanged += WeightUnitChanged;
        }

        private void UpdateAircraftList()
        {
            acListComboBox.SetItems(AvailAircraftTypes());
            acListComboBox.SetItemsPreserveSelection(AvailAircraftTypes());
        }

        private string[] AvailAircraftTypes()
        {
            var allProfileNames = tables.Select(t => t.Entry.ProfileName)
                .ToHashSet();

            return aircrafts
                .Aircrafts
                .Where(c => allProfileNames.Contains(c.Config.TOProfile))
                .Select(c => c.Config.AC)
                .Distinct()
                .OrderBy(i => i)
                .ToArray();
        }

        private bool TakeoffProfileExists(string profileName)
        {
            return tables.Any(c => c.Entry.ProfileName == profileName);
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac = aircrafts.FindAircraft(acListComboBox.Text);
                regComboBox.SetItemsPreserveSelection(ac
                    .Where(c => TakeoffProfileExists(c.Config.TOProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());
            }
        }

        public void TryLoadState()
        {
            var doc = ViewStateSaver.Load(FileName);
            if (doc != null) new ControlState(this).Load(doc);
        }

        public void TrySaveState()
        {
            ViewStateSaver.Save(FileName, new ControlState(this).Save());
        }

        private void SetWeatherBtnHandlers()
        {
            wxSetter = new AutoWeatherSetter(weatherInfoControl, airportInfoControl);
            wxSetter.Subscribe();
        }

        private void InitializeControls()
        {
            wtUnitComboBox.SelectedIndex = 0; // KG  
        }

        private void InitializeElements()
        {
            var ac = airportInfoControl;
            var wic = weatherInfoControl;

            elements = new TOPerfElements(
                ac.airportNameLbl,
                ac.airportTxtBox,
                ac.lengthTxtBox,
                ac.elevationTxtBox,
                ac.rwyHeadingTxtBox,
                wic.windDirTxtBox,
                wic.windSpdTxtBox,
                wic.oatTxtBox,
                wic.pressTxtBox,
                weightTxtBox,
                ac.lengthUnitComboBox,
                ac.slopeComboBox,
                wic.tempUnitComboBox,
                wic.surfCondComboBox,
                wic.pressUnitComboBox,
                wtUnitComboBox,
                flapsComboBox,
                thrustRatingComboBox,
                antiIceComboBox,
                packsComboBox,
                resultsRichTxtBox);
        }

        private void RegistrationChanged(object sender, EventArgs e)
        {
            if (regComboBox.SelectedIndex < 0)
            {
                RefreshWtColor();
                return;
            }

            // unsubsribe all event handlers
            if (controller != null)
            {
                UnSubscribe(controller);
                currentTable = null;
                controller = null;
            }

            var (ac, perf) = FindTable.Find(tables, aircrafts, regComboBox.Text);
            if (ac != null && perf != null)
            {
                // Set currentTable and controller.

                currentTable = perf;

                controller = GetController(new FormControllerData()
                {
                    ConfigItem = ac.Config,
                    PerfTable = currentTable,
                    Elements = elements,
                    ParentControl = this
                });

                Subscribe(controller);
                UpdateFormOptions();
                RefreshWtColor();
            }
        }

        private void UpdateFormOptions()
        {
            if (updatingFormOptions) return;
            updatingFormOptions = true;

            var o = controller.Options;
            var e = elements;
            var pairs = List(
                (e.AntiIce, o.AntiIces),
                (e.Flaps, o.Flaps),
                (e.Packs, o.Packs),
                (e.SurfCond, o.Surfaces),
                (e.ThrustRating, o.Derates));

            pairs.ForEach(p =>
            {
                var (x, y) = p;
                x.SetItemsPreserveSelection(y);
            });

            updatingFormOptions = false;
        }

        private void Subscribe(IFormController controller)
        {
            var e = elements;
            List(e.ThrustRating, e.AntiIce, e.Flaps, e.SurfCond, e.Packs).ForEach(i =>
                i.SelectedIndexChanged += (s, ev) => UpdateFormOptions());

            calculateBtn.Click += Compute;
        }

        private void UnSubscribe(IFormController controller)
        {
            var e = elements;
            List(e.ThrustRating, e.AntiIce, e.Flaps, e.SurfCond, e.Packs).ForEach(i =>
                i.SelectedIndexChanged -= (s, ev) => UpdateFormOptions());

            calculateBtn.Click -= Compute;
        }

        private void RefreshWtColor()
        {
            var ac = aircrafts?.Find(regComboBox.Text);
            var config = ac?.Config;

            if (config == null || !double.TryParse(weightTxtBox.Text, out var wtKg))
            {
                weightTxtBox.ForeColor = Color.Black;
                return;
            }

            if (wtUnitComboBox.SelectedIndex == 1) wtKg *= Constants.LbKgRatio;

            if (wtKg > config.MaxTOWtKg || wtKg < config.OewKg)
            {
                weightTxtBox.ForeColor = Color.Red;
            }
            else
            {
                weightTxtBox.ForeColor = Color.Green;
            }
        }

        private void WeightTxtBoxChanged(object sender, EventArgs e)
        {
            RefreshWtColor();
        }

        /// <summary>
        /// Refresh the aircraft and registration comboBoxes,
        /// after the AcConfigManager is updated.
        /// </summary>
        public void RefreshAircrafts(object sender, EventArgs e)
        {
            // Set the selected aircraft/registration.
            string ac = acListComboBox.Text;
            string reg = regComboBox.Text;

            UpdateAircraftList();
            acListComboBox.Text = ac;
            regComboBox.Text = reg;

            // Set the color of weight.
            RefreshWtColor();
        }

        private void RequestBtnClick(object sender, EventArgs e)
        {
            var ac = acRequestGetter();

            if (ac == null ||
                aircrafts.Find(ac.Registration) == null ||
                aircrafts.Find(ac.Registration).Config.AC != ac.Aircraft)
            {
                this.ShowWarning(
                "The aircraft selected in fuel planning page does not " +
                "have a corresponding takeoff performance profile.");
                return;
            }

            acListComboBox.Text = ac.Aircraft;
            regComboBox.Text = ac.Registration;
            wtUnitComboBox.SelectedIndex = (int)WeightUnit.KG;
            weightTxtBox.Text = Numbers.RoundToInt(ac.TakeOffWeightKg).ToString();
            wtUnitComboBox.SelectedIndex = (int)ac.WtUnit;
        }

        private void TxtRichTextBoxContentsResized(object sender, ContentsResizedEventArgs e)
        {
            var box = resultsRichTxtBox;
            box.Height = e.NewRectangle.Height + 10;
        }

        private static IFormController GetController(FormControllerData d)
        {
            var item = d.PerfTable.Item;
            if (item is BoeingPerfTable) return new BoeingController(d);
            if (item is AirbusPerfTable) return new AirbusController(d);
            throw new ArgumentException();
        }

        private void WeightUnitChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(elements.Weight.Text, out var wt)) return;

            if (elements.WtUnit.SelectedIndex == 0)
            {
                // LB -> KG 
                wt *= AviationTools.Constants.LbKgRatio;
            }
            else
            {
                // KG -> LB
                wt *= AviationTools.Constants.KgLbRatio;
            }

            elements.Weight.Text = ((int)Math.Round(wt)).ToString();
        }

        // Returns whether continue to calculate.
        private bool CheckWeight(TOParameters para)
        {
            var (a, _) = FindTable.Find(tables, aircrafts, regComboBox.Text);
            var ac = a.Config;
            if (para.WeightKg > ac.MaxTOWtKg || para.WeightKg < ac.OewKg)
            {
                var result = this.ShowDialog(
                    "Takeoff weight is not within valid range. Continue to calculate?",
                    MsgBoxIcon.Warning,
                    "",
                    DefaultButton.Button2,
                    "Yes", "No");

                return result == MsgBoxResult.Button1;
            }

            return true;
        }

        public void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = ParameterValidator.Validate(elements);
                if (!CheckWeight(para)) return;
                var tempUnit = (TemperatureUnit)elements.TempUnit.SelectedIndex;
                var tempIncrement = tempUnit == TemperatureUnit.Celsius ? 1.0 : 2.0 / 1.8;
                var report = controller.GetReport(para, tempIncrement);

                var text = report.ToString(tempUnit, (LengthUnit)elements.lengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.Result.Text = text.ShiftToRight(20);

                elements.Result.ForeColor = Color.Black;
                TrySaveState();
            }
            catch (InvalidUserInputException ex)
            {
                this.ShowWarning(ex.Message);
            }
            catch (RunwayTooShortException)
            {
                this.ShowWarning("Runway length is insufficient for takeoff.");
            }
            catch (PoorClimbPerformanceException)
            {
                this.ShowWarning("Aircraft too heavy to meet " +
                    "climb performance requirement.");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
