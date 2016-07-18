using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.LandingPerfCalculation;
using QSP.RouteFinding.Airports;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.ToLdgModule.LandingPerf.FormControllers;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.MathTools.Doubles;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public partial class LandingPerfControl : UserControl
    {
        private const string fileName = "LandingPerfControl.xml";

        private FormController controller;
        private LandingPerfElements elements;
        private List<PerfTable> tables;
        private AcConfigManager aircrafts;
        private Func<AircraftRequest> acRequestGetter;

        private PerfTable currentTable;
        private AutoWeatherSetter wxSetter;

        public AirportManager Airports
        {
            get { return airportInfoControl.Airports; }
            set { airportInfoControl.Airports = value; }
        }

        public LandingPerfControl()
        {
            InitializeComponent();

            // Create the reference to the UI controls.
            InitializeElements();

            // Set default values for the controls.
            InitializeControls();

            setWeatherBtnHandlers();
        }

        private void setWeatherBtnHandlers()
        {
            wxSetter = new AutoWeatherSetter(
                weatherInfoControl, airportInfoControl);
            wxSetter.Subscribe();
        }

        private void InitializeControls()
        {
            appSpdIncTxtBox.Text = "5";
            wtUnitComboBox.SelectedIndex = 0; // KG 
        }

        public void TryLoadState()
        {
            var doc = StateManager.Load(fileName);
            if (doc != null)
            {
                new ControlState(this).Load(doc);
            }
        }

        private void TrySaveState()
        {
            StateManager.Save(fileName, new ControlState(this).Save());
        }

        private void SaveState(object sender, EventArgs e)
        {
            TrySaveState();
        }

        private void InitializeElements()
        {
            var ap = airportInfoControl;
            var wx = weatherInfoControl;

            elements = new LandingPerfElements(
                ap.airportNameLbl,
                ap.airportTxtBox,
                ap.lengthTxtBox,
                ap.elevationTxtBox,
                ap.rwyHeadingTxtBox,
                wx.windDirTxtBox,
                wx.windSpdTxtBox,
                wx.oatTxtBox,
                wx.pressTxtBox,
                weightTxtBox,
                appSpdIncTxtBox,
                ap.rwyComboBox,
                ap.lengthUnitComboBox,
                ap.slopeComboBox,
                wx.tempUnitComboBox,
                brakeComboBox,
                wx.surfCondComboBox,
                wx.pressUnitComboBox,
                wtUnitComboBox,
                flapsComboBox,
                revThrustComboBox,
                resultsRichTxtBox);
        }

        public void Init(
            AcConfigManager aircrafts,
            List<PerfTable> tables,
            AirportManager airports,
            Func<AircraftRequest> acRequestGetter)
        {
            this.aircrafts = aircrafts;
            this.tables = tables;
            UpdateAircraftList();
            this.Airports = airports;
            this.acRequestGetter = acRequestGetter;
        }

        private string[] AvailAircraftTypes()
        {
            var allProfileNames = tables.Select(t => t.Entry.ProfileName);

            return aircrafts
                .Aircrafts
                .Where(c => allProfileNames.Contains(c.Config.LdgProfile))
                .Select(c => c.Config.AC)
                .ToArray();
        }

        private bool LandingProfileExists(string profileName)
        {
            return tables.Any(c => c.Entry.ProfileName == profileName);
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

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac =
                    aircrafts
                    .FindAircraft(acListComboBox.Text);

                var items = regComboBox.Items;

                items.Clear();

                items.AddRange(
                    ac
                    .Where(c => LandingProfileExists(c.Config.LdgProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());

                if (items.Count > 0)
                {
                    regComboBox.SelectedIndex = 0;
                }
            }
        }
        
        private void RequestBtnClick(object sender, EventArgs e)
        {
            var ac = acRequestGetter();

            if (ac == null ||
                aircrafts.Find(ac.Registration) == null ||
                aircrafts.Find(ac.Registration).Config.AC != ac.Aircraft)
            {
                MsgBoxHelper.ShowWarning(
                "The aircraft selected in fuel planning page does not " +
                "have a corresponding landing performance profile.");
                return;
            }
            
            using (var frm = new CustomFuelForm())
            {
                frm.Init(ac);
                frm.ShowInTaskbar = false;
                frm.StartPosition = FormStartPosition.Manual;
                var pt = requestBtn.PointToScreen(Point.Empty);
                frm.Location = new Point(pt.X - 150, pt.Y + 50);
                frm.WeightSet += (_s, _e) =>
                {
                    acListComboBox.Text = ac.Aircraft;
                    regComboBox.Text = ac.Registration;
                    weightTxtBox.Text = RoundToInt(frm.LandingWtKg).ToString();
                    wtUnitComboBox.SelectedIndex = (int)ac.WtUnit;
                };

                frm.ShowDialog();
            }
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

            // set currentTable and controller
            if (tables != null && tables.Count > 0)
            {
                var profileName =
                      aircrafts
                      .Find(regComboBox.Text)
                      .Config
                      .LdgProfile;

                currentTable =
                    tables.First(t => t.Entry.ProfileName == profileName);

                controller = FormControllerFactory.GetController(
                    ControllerType.Boeing,
                    currentTable,
                    elements);
                // TODO: not completely right

                Subscribe(controller);
                controller.Initialize();
                RefreshWtColor();
            }
        }

        private void Subscribe(FormController controller)
        {
            weatherInfoControl.surfCondComboBox.SelectedIndexChanged +=
                controller.SurfCondChanged;

            wtUnitComboBox.SelectedIndexChanged +=
                controller.WeightUnitChanged;

            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged +=
                controller.ReverserChanged;

            brakeComboBox.SelectedIndexChanged += controller.BrakesChanged;
            calculateBtn.Click += controller.Compute;

            controller.CalculationCompleted += SaveState;
        }

        private void UnSubscribe(FormController controller)
        {
            weatherInfoControl.surfCondComboBox.SelectedIndexChanged -=
                controller.SurfCondChanged;

            wtUnitComboBox.SelectedIndexChanged -=
                controller.WeightUnitChanged;

            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged -=
                controller.ReverserChanged;

            brakeComboBox.SelectedIndexChanged -= controller.BrakesChanged;
            calculateBtn.Click -= controller.Compute;

            controller.CalculationCompleted -= SaveState;
        }

        private void RefreshWtColor()
        {
            var ac = aircrafts?.Find(regComboBox.Text);
            var config = ac?.Config;
            double wtKg;

            if (config != null && double.TryParse(weightTxtBox.Text, out wtKg))
            {
                if (wtUnitComboBox.SelectedIndex == 1)
                {
                    wtKg *= Constants.LbKgRatio;
                }

                if (wtKg > config.MaxLdgWtKg || wtKg < config.OewKg)
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
            WeightTxtBoxChanged(this, EventArgs.Empty);
        }
    }
}
