using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.Common;
using QSP.LandingPerfCalculation;
using QSP.LandingPerfCalculation.Airbus;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.UI.Models.MsgBox;
using QSP.UI.Models.TakeoffLanding;
using QSP.UI.UserControls.TakeoffLanding.Common;
using QSP.UI.UserControls.TakeoffLanding.LandingPerf.FormControllers;
using QSP.UI.Util;
using QSP.UI.Views.Factories;
using QSP.Utilities.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.UserControls.TakeoffLanding.LandingPerf
{
    public partial class LandingPerfControl : UserControl
    {
        public const string FileName = "LandingPerfControl.xml";

        private IFormController controller;
        private LandingPerfElements elements;
        private List<PerfTable> tables;
        private AcConfigManager aircrafts;
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

        public LandingPerfControl()
        {
            InitializeComponent();
        }

        private void SetWeatherBtnHandlers()
        {
            wxSetter = new AutoWeatherSetter(weatherInfoControl, airportInfoControl);
            wxSetter.Subscribe();
        }

        private void InitializeControls()
        {
            appSpdIncTxtBox.Text = "5";
            wtUnitComboBox.SelectedIndex = 0; // KG 
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
            airportInfoControl.Init();

            // Create the reference to the UI controls.
            InitializeElements();

            // Set default values for the controls.
            InitializeControls();

            SetWeatherBtnHandlers();

            requestBtn.SetToolTip("Use aircraft and weights calculated from 'Fuel' page.");

            // Automatically update weather
            airportInfoControl.IcaoChanged += (s, e) => wxSetter.GetMetarAndFillWeather();

            this.aircrafts = aircrafts;
            this.tables = tables;
            UpdateAircraftList();
            this.Airports = airports;
            this.acRequestGetter = acRequestGetter;
        }

        private string[] AvailAircraftTypes()
        {
            var allProfileNames = tables.Select(t => t.Entry.ProfileName)
                .ToHashSet();

            return aircrafts
                .Aircrafts
                .Where(c => allProfileNames.Contains(c.Config.LdgProfile))
                .Select(c => c.Config.AC)
                .Distinct()
                .OrderBy(i => i)
                .ToArray();
        }

        private bool LandingProfileExists(string profileName)
        {
            return tables.Any(c => c.Entry.ProfileName == profileName);
        }

        private void UpdateAircraftList()
        {
            acListComboBox.SetItemsPreserveSelection(AvailAircraftTypes());
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex < 0) return;
            var ac = aircrafts.FindAircraft(acListComboBox.Text);
            regComboBox.SetItemsPreserveSelection(ac
                .Where(c => LandingProfileExists(c.Config.LdgProfile))
                .Select(c => c.Config.Registration)
                .ToArray());
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
                    weightTxtBox.Text = Numbers.RoundToInt(frm.LandingWtKg).ToString();
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
                var ac = aircrafts.Find(regComboBox.Text).Config;
                var profileName = ac.LdgProfile;

                currentTable = tables.First(t => t.Entry.ProfileName == profileName);

                controller = GetFormController(new ControllerData()
                {
                    ConfigItem = ac,
                    PerfTable = currentTable,
                    Elements = elements,
                    ParentControl = this
                });

                Subscribe(controller);
                UpdateFormOptions();

                RefreshWtColor();
            }
        }

        private static IFormController GetFormController(ControllerData d)
        {
            var table = d.PerfTable.Item;
            if (table is BoeingPerfTable) return new BoeingController(d);
            if (table is AirbusPerfTable) return new AirbusController(d);
            throw new ArgumentException();
        }

        private void UpdateFormOptions()
        {
            if (updatingFormOptions) return;
            updatingFormOptions = true;

            var o = controller.Options;
            var e = elements;
            var pairs = List(
                (e.Brake, o.Brakes),
                (e.Flaps, o.Flaps),
                (e.SurfCond, o.SurfaceConditions),
                (e.Reverser, o.Reversers));

            pairs.ForEach(p =>
            {
                var (x, y) = p;
                x.SetItemsPreserveSelection(y);
            });

            updatingFormOptions = false;
        }

        private void Subscribe(IFormController controller)
        {
            var c = controller;
            var e = elements;
            List(e.Brake, e.Reverser, e.Flaps, e.SurfCond).ForEach(i =>
                i.SelectedIndexChanged += (s, ev) => UpdateFormOptions());

            wtUnitComboBox.SelectedIndexChanged += WeightUnitChanged;
            calculateBtn.Click += Compute;
        }

        private void UnSubscribe(IFormController controller)
        {
            var c = controller;
            var e = elements;
            List(e.Brake, e.Reverser, e.Flaps, e.SurfCond).ForEach(i =>
                i.SelectedIndexChanged -= (s, ev) => UpdateFormOptions());

            wtUnitComboBox.SelectedIndexChanged -= WeightUnitChanged;
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

            if (wtKg > config.MaxLdgWtKg || wtKg < config.OewKg)
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
            WeightTxtBoxChanged(this, EventArgs.Empty);
        }

        private void TxtRichTextBoxContentsResized(object sender, ContentsResizedEventArgs e)
        {
            resultsRichTxtBox.Height = e.NewRectangle.Height + 10;
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

        public void Compute(object sender, EventArgs e)
        {
            try
            {
                var para = new ParameterValidator(elements).Validate();
                if (!CheckWeight(para)) return;

                var report = controller.GetReport(para);
                var text = report.ToString((LengthUnit)elements.LengthUnit.SelectedIndex);

                // To center the text in the richTxtBox
                elements.Result.Text = text.ShiftToRight(15);

                TrySaveState();
                elements.Result.ForeColor = Color.Black;
            }
            catch (InvalidUserInputException ex)
            {
                this.ShowWarning(ex.Message);
            }
            catch (RunwayTooShortException)
            {
                this.ShowWarning("Runway length is insufficient for landing.");
            }
        }

        // Returns whether continue to calculate.
        private bool CheckWeight(LandingParameters para)
        {
            var ac = aircrafts.Find(regComboBox.Text).Config;
            if (para.WeightKG > ac.MaxTOWtKg || para.WeightKG < ac.OewKg)
            {
                var result = this.ShowDialog(
                    "Landing weight is not within valid range. Continue to calculate?",
                    MsgBoxIcon.Warning,
                    "",
                    DefaultButton.Button2,
                    "Yes", "No");

                return result == MsgBoxResult.Button1;
            }

            return true;
        }
    }
}
