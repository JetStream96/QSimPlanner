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
using QSP.RouteFinding.Navaids;
using QSP.RouteFinding.Tracks;
using QSP.UI.Models.MsgBox;
using QSP.UI.UserControls;
using QSP.Updates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.UI.Util.MsgBoxHelper;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Forms.Options
{
    public partial class OptionsForm : Form
    {
        private Locator<CountryCodeManager> countryCodesLocator;
        private Locator<AppOptions> appSettingsLocator;
        private Updater updater;
        private TracksControl tracksControl;
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
            TracksControl tracksControl,
            AirwayNetwork airwayNetwork,
            Locator<CountryCodeManager> countryCodesLocator,
            Locator<AppOptions> appSettingsLocator,
            Updater updater)
        {
            this.tracksControl = tracksControl;
            this.airwayNetwork = airwayNetwork;
            this.countryCodesLocator = countryCodesLocator;
            this.appSettingsLocator = appSettingsLocator;
            this.updater = updater;

            InitExports();
            SetDefaultState();
            SetControlsAsInOptions();
            FormClosing += CurrentFormClosing;

#if (DEBUG)
            updateBtn.Enabled = false;
            updateBtn.Text = "Update disabled in debug mode.";
#else
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

                var result = this.ShowDialog(
                    "Nav Data needs to be set before proceeding. " +
                    "Otherwise this application will close. " +
                    "Quit the application?",
                    MsgBoxIcon.Warning,
                    "",
                    DefaultButton.Button1,
                    "Quit", "Back to options");

                if (result == MsgBoxResult.Button1) Environment.Exit(0);
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

        public void DetectAndSetAiracFolder()
        {
            var findResult = NavDataPath.DetectNavDataPath();
            if (findResult != null) pathTxtBox.Text = findResult.Directory;
        }

        private void InitExports()
        {
            var exports = new[]
            {
                new RouteExportMatching("Fsx", ProviderType.Fsx, checkBox1, textBox1, button1),
                new RouteExportMatching("P3d", ProviderType.Fsx, checkBox2, textBox2, button2),
                new RouteExportMatching("Fs9", ProviderType.Fs9, checkBox3, textBox3, button3),
                new RouteExportMatching("PmdgCommon", ProviderType.Pmdg, checkBox4, textBox4, button4),
                new RouteExportMatching("PmdgNGX", ProviderType.Pmdg, checkBox5, textBox5, button5),
                new RouteExportMatching("Pmdg777", ProviderType.Pmdg, checkBox6, textBox8, button6)
            };

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

        // Only called when nav data path changes.
        private async Task TryUpdateWptAndAirportsAndSaveOptions()
        {
            var wptList = TryLoadWpts();
            if (wptList == null) return;

            var airportList = TryLoadAirports();
            if (airportList == null) return;

            var navaids = TryLoadNavaids();
            if (navaids == null) return;

            if (!TrySaveOptions()) return;

            // Successful
            await tracksControl.Update(wptList, airportList, navaids);
            NavDataLocationChanged?.Invoke(this, EventArgs.Empty);
            Close();
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
                Log(ex);
                this.ShowError(ex.Message);
                return null;
            }
            catch (LoadCountryNamesException ex)
            {
                Log(ex);
                this.ShowError("Failed to load icao_nationality_code.txt.");
                return null;
            }
        }

        // If failed, returns null.
        private AirportManager TryLoadAirports()
        {
            try
            {
                var directory = pathTxtBox.Text;
                var filePath = Path.Combine(directory, "airports.txt");
                var loadResult = AirportDataLoader.LoadFromFile(filePath);
                var err = loadResult.Errors;
                if (err.Any()) Log(ReadFileErrorMsg.ErrorMsg(err, "ats.txt"));
                return loadResult.Airports;
            }
            catch (Exception ex)
            {
                Log(ex);
                this.ShowError("Failed to load airports.txt.");
                return null;
            }
        }

        // If failed, returns null.
        private MultiMap<string, Navaid> TryLoadNavaids()
        {
            try
            {
                var dir = pathTxtBox.Text;
                return NavaidsLoader.LoadFromFile(Path.Combine(dir, "navaids.txt"));
            }
            catch (Exception ex)
            {
                Log(ex);
                this.ShowError("Failed to load navaids.txt.");
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

                return true;
            }
            else
            {
                this.ShowError("Failed to save options.");
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
                new Dictionary<SimulatorType, string>(), // TODO:
                exportController.GetCommands());
        }

        private void CancelBtnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowseBtnClick(object sender, EventArgs e)
        {
            using (var dialog = new FolderSelectDialog())
            {
                dialog.InitialDirectory = pathTxtBox.Text;

                if (dialog.ShowDialog())
                {
                    pathTxtBox.Text = dialog.FileName;
                }
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
            string[] FilesToCheck =
            {
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
                Log(ex);

                navDataStatusLbl.Text = "Failed to load";
                airacLbl.Text = "N/A";
                airacPeriodLbl.Text = "N/A";
                airacPeriodLbl.Text = "";
            }
        }

        private void InfoLblMouseEnter(object sender, EventArgs e)
        {
            infoLbl.Font = new Font(infoLbl.Font, FontStyle.Underline);

            popUpPanel = InfoPanel();
            Controls.Add(popUpPanel);
            popUpPanel.BringToFront();
        }

        private void InfoLblMouseLeave(object sender, EventArgs e)
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
            panel.Location = new Point(50, pt.Y + 100);
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

        private static string LblTxt =>
            "If you are using Aerosoft's NavDataPro:\n" +
            "    you can update with Aerosoft's program since QSimPlanner is supported.\n" +
            "If you are using Navigraph:\n" +
            "    install the NavData version for Aerosoft Airbus A318/A319/A320/A321\n" +
            "    and select the folder which contains Airports.txt.";

        private void UpdateBtnClick(object sender, EventArgs e)
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
            updateStatusLbl.ForeColor = status.Status == Updater.Status.Failed ?
                Color.Red : Color.Green;

            // If update succeeded, some files are already changed and further updating
            // may not work as intended. So we disable this button.
            updateBtn.Enabled = status.Status != Updater.Status.Success;
        }

        private void UseDefaultBtnClick(object sender, EventArgs e)
        {
            pathTxtBox.Text = Path.GetFullPath(AppOptions.Default.NavDataLocation);
        }
    }
}
