using QSP.LandingPerfCalculation;
using QSP.RouteFinding.Airports;
using QSP.UI.ToLdgModule.LandingPerf.FormControllers;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public partial class LandingPerfControl : UserControl
    {
        private CustomFuelControl fuelImportPanel;
        private FormController controller;
        private List<PerfTable> tables;
        private PerfTable currentTable;
        private LandingPerfElements elements;

        public AirportManager Airports
        {
            get { return airportInfoControl.Airports; }
            set { airportInfoControl.Airports = value; }
        }

        public LandingPerfControl()
        {
            InitializeComponent();

            // Create the reference to the UI controls.
            initailzeElements();

            // Set default values for the controls.
            initializeControls();
        }

        private void initializeControls()
        {
            appSpdIncTxtBox.Text = "5";
            wtUnitComboBox.SelectedIndex = 0; // KG
        }

        private void initailzeElements()
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
                surfCondComboBox,
                weatherInfoControl.pressUnitComboBox,
                wtUnitComboBox,
                flapsComboBox,
                revThrustComboBox,
                resultsRichTxtBox);
        }

        public void InitializeAircrafts()
        {
            tables = InstanceInitializer.Initialize();
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

        // The request button is not visible by default.
        // Call this method to enable it.
        public void EnableWeightRequest(double zfwKg, double fuelKg)
        {
            requestBtn.Visible = true;
            fuelImportPanel = new CustomFuelControl(zfwKg, fuelKg);
            fuelImportPanel.Location = new Point(253, 61);
        }

        private void requestBtn_Click(object sender, System.EventArgs e)
        {
            if (fuelImportPanel != null)
            {
                fuelImportPanel.Visible = true;
            }
        }

        private void acListComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
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
            surfCondComboBox.SelectedIndexChanged += controller.SurfCondChanged;
            wtUnitComboBox.SelectedIndexChanged += controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged += controller.ReverserChanged;
            brakeComboBox.SelectedIndexChanged += controller.BrakesChanged;
            calculateBtn.Click += controller.Compute;
        }

        private void unSubscribe(FormController controller)
        {
            surfCondComboBox.SelectedIndexChanged -= controller.SurfCondChanged;
            wtUnitComboBox.SelectedIndexChanged -= controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged -= controller.ReverserChanged;
            brakeComboBox.SelectedIndexChanged -= controller.BrakesChanged;
            calculateBtn.Click -= controller.Compute;
        }
    }
}
