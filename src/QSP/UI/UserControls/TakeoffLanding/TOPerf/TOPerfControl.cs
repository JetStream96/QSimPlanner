using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.LibraryExtension;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.TOPerfCalculation;
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
using QSP.TOPerfCalculation.Airbus.DataClasses;
using QSP.TOPerfCalculation.Boeing.PerfData;

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
        }

        private void UpdateAircraftList()
        {
            var items = acListComboBox.Items;

            items.Clear();
            items.AddRange(AvailAircraftTypes());

            if (items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }
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
                var items = regComboBox.Items;
                items.Clear();

                items.AddRange(ac
                    .Where(c => TakeoffProfileExists(c.Config.TOProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());

                if (items.Count > 0)
                {
                    regComboBox.SelectedIndex = 0;
                }
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

        private void SaveState(object sender, EventArgs e)
        {
            TrySaveState();
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
                thrustRatingLbl,
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
                controller.Initialize();
                RefreshWtColor();
            }
        }

        private void Subscribe(IFormController controller)
        {
            wtUnitComboBox.SelectedIndexChanged += controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            calculateBtn.Click += controller.Compute;

            controller.CalculationCompleted += SaveState;
        }

        private void UnSubscribe(IFormController controller)
        {
            wtUnitComboBox.SelectedIndexChanged -= controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            calculateBtn.Click -= controller.Compute;

            controller.CalculationCompleted -= SaveState;
        }

        private void RefreshWtColor()
        {
            var ac = aircrafts?.Find(regComboBox.Text);
            var config = ac?.Config;

            if (config != null && double.TryParse(weightTxtBox.Text, out var wtKg))
            {
                if (wtUnitComboBox.SelectedIndex == 1)
                {
                    wtKg *= Constants.LbKgRatio;
                }

                if (wtKg > config.MaxTOWtKg || wtKg < config.OewKg)
                {
                    weightTxtBox.ForeColor = Color.Red;
                }
                else
                {
                    weightTxtBox.ForeColor = Color.Green;
                }
            }
            else
            {
                weightTxtBox.ForeColor = Color.Black;
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
    }
}
