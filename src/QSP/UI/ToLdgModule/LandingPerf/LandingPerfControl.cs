﻿using QSP.AircraftProfiles.Configs;
using QSP.LandingPerfCalculation;
using QSP.RouteFinding.Airports;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.ToLdgModule.LandingPerf.FormControllers;
using System;
using System.Collections.Generic;
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
            wxSetter = new AutoWeatherSetter(weatherInfoControl, airportInfoControl);
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
            elements = new LandingPerfElements(
                airportInfoControl.airportNameLbl,
                airportInfoControl.airportTxtBox,
                airportInfoControl.lengthTxtBox,
                airportInfoControl.elevationTxtBox,
                airportInfoControl.rwyHeadingTxtBox,
                weatherInfoControl.windDirTxtBox,
                weatherInfoControl.windSpdTxtBox,
                weatherInfoControl.oatTxtBox,
                weatherInfoControl.pressTxtBox,
                weightTxtBox,
                appSpdIncTxtBox,
                airportInfoControl.rwyComboBox,
                airportInfoControl.lengthUnitComboBox,
                airportInfoControl.slopeComboBox,
                weatherInfoControl.tempUnitComboBox,
                brakeComboBox,
                weatherInfoControl.surfCondComboBox,
                weatherInfoControl.pressUnitComboBox,
                wtUnitComboBox,
                flapsComboBox,
                revThrustComboBox,
                resultsRichTxtBox);
        }

        public void InitializeAircrafts(
            AcConfigManager aircrafts,
            List<PerfTable> tables)
        {
            this.aircrafts = aircrafts;
            this.tables = tables;
            updateAircraftList();
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

        private void requestBtn_Click(object sender, EventArgs e)
        {
            //if (fuelImportPanel != null)
            //{
            //    fuelImportPanel.Visible = true;
            //}
        }

        private void registrationSelectedChanged(object sender, EventArgs e)
        {
            if (regComboBox.SelectedIndex < 0)
            {
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
            }
        }

        private void subscribe(FormController controller)
        {
            weatherInfoControl.surfCondComboBox.SelectedIndexChanged += controller.SurfCondChanged;
            wtUnitComboBox.SelectedIndexChanged += controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged += controller.ReverserChanged;
            brakeComboBox.SelectedIndexChanged += controller.BrakesChanged;
            calculateBtn.Click += controller.Compute;

            controller.CalculationCompleted += saveState;
        }

        private void unSubscribe(FormController controller)
        {
            weatherInfoControl.surfCondComboBox.SelectedIndexChanged -= controller.SurfCondChanged;
            wtUnitComboBox.SelectedIndexChanged -= controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged -= controller.ReverserChanged;
            brakeComboBox.SelectedIndexChanged -= controller.BrakesChanged;
            calculateBtn.Click -= controller.Compute;

            controller.CalculationCompleted -= saveState;
        }
    }
}
