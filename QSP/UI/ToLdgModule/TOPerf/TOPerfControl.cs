using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.TOPerfCalculation;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.ToLdgModule.TOPerf.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
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
        private string metar;

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

        // TODO: Extract common codes.
        private void setWeatherBtnHandlers()
        {
            weatherInfoControl.GetMetarBtn.Click += getMetarClicked;
            weatherInfoControl.ViewMetarBtn.Click += viewMetarClicked;
        }

        private void enableDnBtn()
        {
            var btn = weatherInfoControl.GetMetarBtn;

            btn.Enabled = true;
            btn.BackColor = Color.DarkSlateGray;
            btn.Text = "Import METAR";
        }

        private void disableDnBtn()
        {
            var btn = weatherInfoControl.GetMetarBtn;

            btn.Enabled = false;
            btn.BackColor = Color.Gray;
            btn.Text = "Downloading ...";
        }

        private void viewMetarClicked(object sender, EventArgs e)
        {
            var frm = new MetarForm();
            frm.icaoTxtBox.Text = airportInfoControl.airportTxtBox.Text;
            frm.icaoTxtBox.Enabled = false;
            frm.resultRichTxtBox.Text = metar ?? "";
            frm.sendBtn.Visible = false;
            frm.downloadBtn.Visible = false;
            frm.getTafCheckBox.Visible = false;
            frm.ShowDialog();
        }

        // Get metar functions.
        private async void getMetarClicked(object sender, EventArgs e)
        {
            disableDnBtn();
            weatherInfoControl.pictureBox1.Visible = false;

            string icao = airportInfoControl.airportTxtBox.Text;
            metar = null;

            bool metarAcquired =
                 await Task.Run(() => MetarDownloader.TryGetMetar(icao, out metar));

            if (metarAcquired)
            {
                var w = weatherInfoControl;

                if (WeatherAutoFiller.Fill(
                    metar,
                    w.windDirTxtBox,
                    w.windSpdTxtBox,
                    w.oatTxtBox,
                    w.tempUnitComboBox,
                    w.pressTxtBox,
                    w.pressUnitComboBox))
                {
                    enableDnBtn();
                    w.pictureBox1.Visible = true;
                }
                else
                {
                    MessageBox.Show(@"Unable to fill the weather information automatically.");
                }
            }
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
