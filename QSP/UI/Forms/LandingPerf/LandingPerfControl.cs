using System.Windows.Forms;
using System.Drawing;
using QSP.LandingPerfCalculation;
using System.Collections.Generic;
using QSP.UI.Forms.LandingPerf.FormControllers;

namespace QSP.UI.Forms.LandingPerf
{
    public partial class LandingPerfControl : UserControl
    {
        private CustomFuelControl fuelImportPanel;
        private FormController controller;
        private List<PerfTable> tables;
        private PerfTable currentTable;
        private LandingPerfElements elements;

        public LandingPerfControl()
        {
            InitializeComponent();
            initailzeElements();
        }

        private void initailzeElements()
        {
            elements = new LandingPerfElements(
                airportNameLbl,
                airportTxtBox,
                lengthTxtBox,
                elevationTxtBox,
                rwyHeadingTxtBox,
                windDirTxtBox,
                windSpdTxtBox,
                oatTxtBox,
                pressTxtBox,
                weightTxtBox,
                appSpdIncTxtBox,
                rwyComboBox,
                lengthUnitComboBox,
                slopeComboBox,
                tempUnitComboBox,
                brakeComboBox,
                surfCondComboBox,
                pressUnitComboBox,
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
                controller = new FormController(currentTable, elements);// TODO: not right
                subscribe(controller);
            }
        }

        private void subscribe(FormController controller)
        {
            airportTxtBox.TextChanged += controller.AirportChanged;
            rwyComboBox.SelectedIndexChanged += controller.RunwayChanged;
            lengthUnitComboBox.SelectedIndexChanged += controller.LengthUnitChanged;
            tempUnitComboBox.SelectedIndexChanged += controller.TempUnitChanged;
            pressUnitComboBox.SelectedIndexChanged += controller.PressureUnitChanged;
            surfCondComboBox.SelectedIndexChanged += controller.SurfCondChanged;
            wtUnitComboBox.SelectedIndexChanged += controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged += controller.ReverserChanged;
            brakeComboBox.SelectedIndexChanged += controller.BrakesChanged;
            calculateBtn.Click += controller.Compute;
        }

        private void unSubscribe(FormController controller)
        {
            airportTxtBox.TextChanged -= controller.AirportChanged;
            rwyComboBox.SelectedIndexChanged -= controller.RunwayChanged;
            lengthUnitComboBox.SelectedIndexChanged -= controller.LengthUnitChanged;
            tempUnitComboBox.SelectedIndexChanged -= controller.TempUnitChanged;
            pressUnitComboBox.SelectedIndexChanged -= controller.PressureUnitChanged;
            surfCondComboBox.SelectedIndexChanged -= controller.SurfCondChanged;
            wtUnitComboBox.SelectedIndexChanged -= controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            revThrustComboBox.SelectedIndexChanged -= controller.ReverserChanged;
            brakeComboBox.SelectedIndexChanged -= controller.BrakesChanged;
            calculateBtn.Click -= controller.Compute;
        }
    }
}
