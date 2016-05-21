using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.LandingPerfCalculation;
using QSP.RouteFinding.Airports;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.ToLdgModule.LandingPerf.FormControllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public partial class LandingPerfControl : UserControl
    {
        private const string fileName = "LandingPerfControl.xml";

        // private CustomFuelControlOld fuelImportPanel;
        private FormController controller;
        private LandingPerfElements elements;
        private List<PerfTable> tables;
        private AcConfigManager aircrafts;

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
            initializeElements();

            // Set default values for the controls.
            initializeControls();

            setWeatherBtnHandlers();
        }

        private void setWeatherBtnHandlers()
        {
            wxSetter = new AutoWeatherSetter(
                weatherInfoControl, airportInfoControl);
            wxSetter.Subscribe();
        }

        private void initializeControls()
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

        private void trySaveState()
        {
            StateManager.Save(fileName, new ControlState(this).Save());
        }

        private void saveState(object sender, EventArgs e)
        {
            trySaveState();
        }

        private void initializeElements()
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

        public void InitializeAircrafts(
            AcConfigManager aircrafts,
            List<PerfTable> tables,
            AirportManager airports)
        {
            this.aircrafts = aircrafts;
            this.tables = tables;
            updateAircraftList();
            this.Airports = airports;
        }

        private string[] availAircraftTypes()
        {
            var avail = new List<string>();

            foreach (var i in tables)
            {
                if (aircrafts
                    .Aircrafts
                    .Where(c => c.Config.LdgProfile == i.Entry.ProfileName)
                    .Count()
                    > 0)
                {
                    avail.Add(i.Entry.Aircraft);
                }
            }

            return avail.ToArray();
        }

        private bool landingProfileExists(string profileName)
        {
            var searchResults =
                tables.Where(c => c.Entry.ProfileName == profileName);

            return searchResults.Count() > 0;
        }

        private void updateAircraftList()
        {
            var items = acListComboBox.Items;

            items.Clear();
            items.AddRange(availAircraftTypes());

            if (items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }
        }

        private void refreshRegistrations(object sender, EventArgs e)
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
                    .Where(c => landingProfileExists(c.Config.LdgProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());

                if (items.Count > 0)
                {
                    regComboBox.SelectedIndex = 0;
                }
            }
        }

        // The request button is not visible by default.
        // Call this method to enable it.
        public void EnableWeightRequest(double zfwKg, double fuelKg)
        {
            requestBtn.Visible = true;
            // fuelImportPanel = new CustomFuelControlOld(zfwKg, fuelKg);
            // fuelImportPanel.Location = new Point(253, 61);
        }

        private void requestBtnClick(object sender, EventArgs e)
        {
            //if (fuelImportPanel != null)
            //{
            //    fuelImportPanel.Visible = true;
            //}
        }

        private void registrationChanged(object sender, EventArgs e)
        {
            if (regComboBox.SelectedIndex < 0)
            {
                refreshWtColor();
                return;
            }

            // unsubsribe all event handlers
            if (controller != null)
            {
                unSubscribe(controller);
                currentTable = null;
                controller = null;
            }

            // set currentTable and controller
            if (tables != null && tables.Count > 0)
            {
                var profileName =
                      aircrafts
                      .FindRegistration(regComboBox.Text)
                      .Config
                      .LdgProfile;

                currentTable =
                    tables.First(t => t.Entry.ProfileName == profileName);

                controller = FormControllerFactory.GetController(
                    ControllerType.Boeing,
                    currentTable,
                    elements);
                // TODO: not completely right

                subscribe(controller);
                controller.Initialize();
                refreshWtColor();
            }
        }

        private void subscribe(FormController controller)
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

            controller.CalculationCompleted += saveState;
        }

        private void unSubscribe(FormController controller)
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

            controller.CalculationCompleted -= saveState;
        }

        private void refreshWtColor()
        {
            var ac = aircrafts?.FindRegistration(regComboBox.Text);
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

        private void weightTxtBoxChanged(object sender, EventArgs e)
        {
            refreshWtColor();
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

            updateAircraftList();
            acListComboBox.Text = ac;
            regComboBox.Text = reg;

            // Set the color of weight.
            weightTxtBoxChanged(this, EventArgs.Empty);
        }
    }
}
