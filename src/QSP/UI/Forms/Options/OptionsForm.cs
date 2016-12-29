using FolderSelect;
using QSP.AviationTools.Airac;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.NavData;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.Tracks;
using QSP.Updates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.UI.Utilities.MsgBoxHelper;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Forms.Options
{
    public partial class OptionsForm : Form
    {
        private Locator<CountryCodeManager> countryCodesLocator;
        private Locator<AppOptions> appSettingsLocator;
        private Updater updater;
        private TracksForm tracksForm;
        private AirwayNetwork airwayNetwork;
        private FlightPlanExportController exportController;
        private Panel popUpPanel;

        public event EventHandler NavDataLocationChanged;

        public AppOptions AppSettings => appSettingsLocator.Instance;

        public OptionsForm()
        {
            InitializeComponent();
        }

        public void Init(
            TracksForm tracksForm,
            AirwayNetwork airwayNetwork,
            Locator<CountryCodeManager> countryCodesLocator,
            Locator<AppOptions> appSettingsLocator,
            Updater updater,
            bool autoDetectAiracFolder = false)
        {
            this.tracksForm = tracksForm;
            this.airwayNetwork = airwayNetwork;
            this.countryCodesLocator = countryCodesLocator;
            this.appSettingsLocator = appSettingsLocator;
            this.updater = updater;

            InitExports();
            SetDefaultState();
            SetControlsAsInOptions();
            FormClosing += CurrentFormClosing;

            if (autoDetectAiracFolder) DetectAiracFolder();

#if (!DEBUG)
            PerformAutoUpdate();
#endif
        }

        private void PerformAutoUpdate()
        {
            if (AppSettings.AutoUpdate) PerformUpdateNow();
        }

        private void CurrentFormClosing(object sender, FormClosingEventArgs e)
        {
            if (airwayNetwork.AirportList is DefaultAirportManager ||
                airwayNetwork.WptList is DefaultWaypointList ||
                countryCodesLocator.Instance == null)
            {
                e.Cancel = true;

                var result = MessageBox.Show(
                    "Nav Data needs to be set before proceeding. " +
                    "Otherwise this application will close. " +
                    "Close the application?",
                    "",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Exclamation);

                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
        }

        private void SetDefaultState()
        {
            PromptBeforeExit.Checked = true;

            navDataStatusLbl.Text = "";
            airacLbl.Text = "";
            airacPeriodLbl.Text = "";

            AutoDLTracksCheckBox.Checked = true;
            AutoDLWindCheckBox.Checked = true;

            updateFreqComboBox.SelectedIndex = 0;
            updateStatusLbl.Text = "";
        }

        public void SetControlsAsInOptions()
        {
            AutoDLTracksCheckBox.Checked = AppSettings.AutoDLTracks;
            AutoDLWindCheckBox.Checked = AppSettings.AutoDLWind;
            pathTxtBox.Text = AppSettings.NavDataLocation;
            PromptBeforeExit.Checked = AppSettings.PromptBeforeExit;
            WindOptimizedRouteCheckBox.Checked = AppSettings.EnableWindOptimizedRoute;
            hideDctCheckBox.Checked = AppSettings.HideDctInRoute;
            showTrackIdOnlyCheckBox.Checked = AppSettings.ShowTrackIdOnly;
            updateFreqComboBox.SelectedIndex = AppSettings.AutoUpdate ? 0 : 1;
            exportController.SetExports();
        }

        private void DetectAiracFolder()
        {
            var findResult = NavDataPath.DetectNavDataPath();
            if (findResult != null) pathTxtBox.Text = findResult.Directory;
        }

        private void InitExports()
        {
            var exports = new List<RouteExportMatching>();

            exports.Add(new RouteExportMatching(
                "Fsx",
                ProviderType.Fsx,
                checkBox4,
                textBox4,
                button4));

            exports.Add(new RouteExportMatching(
                "P3d",
                ProviderType.Fsx,
                checkBox5,
                textBox5,
                button5));

            exports.Add(new RouteExportMatching(
                "Fs9",
                ProviderType.Fs9,
                checkBox6,
                textBox6,
                button6));

            exports.Add(new RouteExportMatching(
                "PmdgCommon",
                ProviderType.Pmdg,
                CheckBox1,
                TextBox1,
                Button1));

            exports.Add(new RouteExportMatching(
                "PmdgNGX",
                ProviderType.Pmdg,
                CheckBox2,
                TextBox2,
                Button2));

            exports.Add(new RouteExportMatching(
                "Pmdg777",
                ProviderType.Pmdg,
                CheckBox3,
                TextBox3,
                Button3));

            exportController = new FlightPlanExportController(exports, appSettingsLocator);
            exportController.Init();
        }

        private async void SaveBtnClick(object sender, EventArgs e)
        {
            saveBtn.ForeColor = Color.Black;
            saveBtn.Enabled = false;
            saveBtn.BackColor = Color.FromArgb(224, 224, 224);
            saveBtn.Text = "Saving ...";
            Refresh();

            if (pathTxtBox.Text != AppSettings.NavDataLocation)
            {
                await TryUpdateWptAndAirportsAndSaveOptions();
            }
            else
            {
                if (TrySaveOptions()) Close();
            }

            saveBtn.ForeColor = Color.White;
            saveBtn.BackColor = Color.Green;
            saveBtn.Text = "Save";
            saveBtn.Enabled = true;
        }

        private async Task TryUpdateWptAndAirportsAndSaveOptions()
        {
            var wptList = TryLoadWpts();

            if (wptList != null)
            {
                var airportList = TryLoadAirports();

                if (airportList != null && TrySaveOptions())
                {
                    // Successful
                    await tracksForm.Update(wptList, airportList);
                    Close();
                }
            }
        }

        // If failed, returns null.
        private WaypointList TryLoadWpts()
        {
            try
            {
                var loader = new WptListLoader(pathTxtBox.Text);
                var result = loader.LoadFromFile();
                countryCodesLocator.Instance = result.CountryCodes;
                return result.WptList;
            }
            catch (WaypointFileReadException ex)
            {
                WriteToLog(ex);
                ShowError("Failed to load waypoints.txt.");
                return null;
            }
            catch (LoadCountryNamesException ex)
            {
                WriteToLog(ex);
                ShowError("Failed to load icao_nationality_code.txt.");
                return null;
            }
        }

        // If failed, returns null.
        private AirportManager TryLoadAirports()
        {
            try
            {
                var directory = pathTxtBox.Text;
                var filePath = Path.Combine(directory, "Airports.txt");
                var loader = new AirportDataLoader(filePath);
                return loader.LoadFromFile();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                ShowError("Failed to load airports.txt.");
                return null;
            }
        }

        private bool TrySaveOptions()
        {
            var newSetting = ValidateSetting();

            if (OptionManager.TrySaveFile(newSetting))
            {
                var oldSetting = appSettingsLocator.Instance;
                appSettingsLocator.Instance = newSetting;

                if (oldSetting.NavDataLocation != newSetting.NavDataLocation)
                {
                    NavDataLocationChanged?.Invoke(this, EventArgs.Empty);
                }

                return true;
            }
            else
            {
                ShowError("Failed to save options.");
                return false;
            }
        }

        private AppOptions ValidateSetting()
        {
            return new AppOptions(
                pathTxtBox.Text,
                PromptBeforeExit.Checked,
                AutoDLTracksCheckBox.Checked,
                AutoDLWindCheckBox.Checked,
                WindOptimizedRouteCheckBox.Checked,
                hideDctCheckBox.Checked,
                showTrackIdOnlyCheckBox.Checked,
                updateFreqComboBox.SelectedIndex == 0,
                exportController.GetCommands());
        }

        private void CancelBtnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowseBtnClick(object sender, EventArgs e)
        {
            var dialog = new FolderSelectDialog();
            dialog.InitialDirectory=pathTxtBox.Text;

            if (dialog.ShowDialog())
            {
                pathTxtBox.Text = dialog.FileName;
            }
        }

        private void PathTxtBoxTextChanged(object sender, EventArgs e)
        {
            DisplayNavDataStatus(pathTxtBox.Text);
        }

        private void DisplayNavDataStatus(string navDataPath)
        {
            // This section is to determine whether the database
            // files are found or not.
            string[] FilesToCheck = {
                "airports.txt",
                "ats.txt",
                "cycle.txt",
                "navaids.txt",
                "waypoints.txt"
            };

            bool navDataFound = FilesToCheck.All(i =>
                File.Exists(Path.Combine(navDataPath, i)));

            navDataStatusLbl.Text = navDataFound ? "Found" : "Not Found";

            try
            {
                var t = AiracTools.AiracCyclePeriod(navDataPath);
                airacLbl.Text = t.Cycle;
                airacPeriodLbl.Text = t.PeriodText;

                bool expired = !t.IsWithinValidPeriod;

                if (expired)
                {
                    airacPeriodLbl.Text += "  (Expired)";
                    airacPeriodLbl.ForeColor = Color.Red;
                    airacLbl.ForeColor = Color.Red;
                }
                else
                {
                    airacPeriodLbl.Text += "  (Within Valid Period)";
                    airacPeriodLbl.ForeColor = Color.Green;
                    airacLbl.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);

                navDataStatusLbl.Text = "Failed to load";
                airacLbl.Text = "N/A";
                airacPeriodLbl.Text = "N/A";
                airacPeriodLbl.Text = "";
            }
        }

        private void infoLbl_MouseEnter(object sender, EventArgs e)
        {
            infoLbl.Font = new Font(infoLbl.Font, FontStyle.Underline);

            popUpPanel = InfoPanel();
            Controls.Add(popUpPanel);
            popUpPanel.BringToFront();
        }

        private void infoLbl_MouseLeave(object sender, EventArgs e)
        {
            infoLbl.Font = new Font(infoLbl.Font, FontStyle.Regular);

            Controls.Remove(popUpPanel);
            popUpPanel = null;
        }

        private Panel InfoPanel()
        {
            var panel = new Panel();
            panel.BackColor = Color.FromArgb(216, 244, 215);
            panel.BorderStyle = BorderStyle.FixedSingle;
            var pt = infoLbl.Location;
            panel.Location = new Point(pt.X - 300, pt.Y + 100);
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var lbl = new Label();
            lbl.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular);
            lbl.ForeColor = Color.DarkGreen;
            lbl.Text = LblTxt;
            lbl.Location = new Point(0, 0);
            lbl.AutoSize = true;

            panel.Controls.Add(lbl);
            return panel;
        }

        private static string LblTxt
        {
            get
            {
                return
                @"Location of either Aerosoft's NavDataPro or Navigraph's
FMS Data (both are payware). Use the version of Aerosoft
Airbus A318/A319/A320/A321. Select the folder which
contains Airports.txt.";
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            PerformUpdateNow();
        }

        private async Task PerformUpdateNow()
        {
            updateBtn.Enabled = false;
            updateStatusLbl.Text = "Updating ...";
            updateStatusLbl.ForeColor = Color.Black;
            var status = await Task.Factory.StartNew(() => updater.Update());
            updateStatusLbl.Text = status.Message;
            updateStatusLbl.ForeColor = status.Success ? Color.Green : Color.Red;
            updateBtn.Enabled = true;
        }
    }
}
