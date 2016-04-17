using QSP.RouteFinding.Airports;
using QSP.TOPerfCalculation;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.ToLdgModule.TOPerf.Controllers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.TOPerf
{
    public partial class TOPerfControl : UserControl
    {
        private const string fileName = "TakeoffPerfControl.xml";

        private FormController controller;
        private TOPerfElements elements;
        private List<PerfTable> tables;
        private PerfTable currentTable;
        private AutoWeatherSetter wxSetter;

        public AirportManager Airports
        {
            get { return airportInfoControl.Airports; }
            set { airportInfoControl.Airports = value; }
        }

        public TOPerfControl()
        {
            InitializeComponent();

            // Create the reference to the UI controls.
            initializeElements();

            // Set default values for the controls.
            initializeControls();

            setWeatherBtnHandlers();
        }

        public void InitializeAircrafts()
        {
            var result = TOPerfCollection.Initialize();
            tables = result.Tables;

            if (result.Message != null)
            {
                MessageBox.Show(result.Message);
            }

            updateAircraftList();
        }

        private void updateAircraftList()
        {
            acListComboBox.Items.Clear();

            foreach (var i in tables)
            {
                acListComboBox.Items.Add(i.Entry.Aircraft);
            }
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

        private void setWeatherBtnHandlers()
        {
            wxSetter = new AutoWeatherSetter(weatherInfoControl, airportInfoControl);
            wxSetter.Subscribe();
        }

        private void initializeControls()
        {
            wtUnitComboBox.SelectedIndex = 0; // KG  
            thrustRatingLbl.Visible = false;
            thrustRatingComboBox.Visible = false;

            if (acListComboBox.Items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }
        }

        private void initializeElements()
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

        private void acListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                currentTable = tables[acListComboBox.SelectedIndex];

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
            wtUnitComboBox.SelectedIndexChanged += controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            calculateBtn.Click += controller.Compute;

            controller.CalculationCompleted += saveState;
        }

        private void unSubscribe(FormController controller)
        {
            wtUnitComboBox.SelectedIndexChanged -= controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            calculateBtn.Click -= controller.Compute;

            controller.CalculationCompleted -= saveState;
        }
    }
}
