using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Forms.Options;
using QSP.UI.ToLdgModule.AboutPage;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.UserControls;
using QSP.UI.Utilities;
using QSP.Updates;
using QSP.Utilities;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks;
using QSP.UI.MsgBox;
using static QSP.Utilities.LoggerInstance;

namespace QSP.UI.Forms
{
    public partial class QspForm : Form
    {
        private AircraftMenuControl acMenu;
        private FuelPlanningControl fuelMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private MiscInfoControl miscInfoMenu;
        private AboutPageControl aboutMenu;

        private ProfileCollection profiles;
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private Locator<CountryCodeManager> countryCodesLocator;
        private ProcedureFilter procFilter;
        private Locator<IWindTableCollection> windTableLocator;
        private Updater updater;

        private TracksForm trackFrm;
        private WindDataForm windFrm;
        private bool failedToLoadNavDataAtStartUp = false;

        private AppOptions appSettings => appOptionsLocator.Instance;
        private AirportManager airportList => airwayNetwork.AirportList;
        private WaypointList wptList => airwayNetwork.WptList;
        private TrackInUseCollection tracksInUse => airwayNetwork.TracksInUse;
        private CountryCodeManager countryCodes => countryCodesLocator.Instance;

        private IEnumerable<UserControl> Pages
        {
            get
            {
                return new UserControl[]
                {
                    acMenu,
                    fuelMenu,
                    toMenu,
                    ldgMenu,
                    miscInfoMenu,
                    aboutMenu
                };
            }
        }

        public QspForm()
        {
            InitializeComponent();
        }

        public void Init()
        {
            ShowSplashWhile(() =>
            {
                AddControls();
                DoPostUpdateActions();
                InitData();
                InitControls();
                InitTrackForm();
                InitWindForm();
                DownloadWindIfNeeded();
                DownloadTracksIfNeeded();
            });

            if (failedToLoadNavDataAtStartUp)
            {
                if (appSettings.NavDataLocation == AppOptions.Default.NavDataLocation)
                {
                    // User did not set the path. 
                    // Maybe its the first time the app starts.
                    MsgBoxHelper.ShowInfo("Please set the correct Nav Data location " +
                    "before using the application.");
                }
                else
                {
                    MsgBoxHelper.ShowWarning("Please set the correct Nav Data location.");
                }

                ShowOptionsForm(FormStartPosition.CenterScreen, true, true);
            }
        }

        private void DoPostUpdateActions()
        {
#if (!DEBUG)
            var action = new PostUpdateAction();

            try
            {
                action.DoAction();
            }
            catch (Exception ex)
            {
                Log(ex);
                MsgBoxHelper.ShowWarning(
                    "An error occurred when copying profiles for the updated" +
                    $" verison.\n\n(Error:{ex.GetBaseException().ToString()}");
            }
#endif
        }

        private void InitWindForm()
        {
            windFrm = new WindDataForm();
            windFrm.Init(windDataStatusLabel,
                windTableLocator, WindDownloadStatus.WaitingManualDownload);
        }

        private void InitTrackForm()
        {
            trackFrm = new TracksForm();
            trackFrm.Init(airwayNetwork, trackStatusLabel);
        }

        private static void ShowSplashWhile(Action action)
        {
            var splash = new Splash();
            splash.Show();
            splash.Refresh();

            action();

            splash.Close();
        }

        private void InitData()
        {
            try
            {
                // Aircraft data
                profiles = new ProfileCollection();
                profiles.Initialize();
            }
            catch (PerfFileNotFoundException ex)
            {
                Log(ex);
                MsgBoxHelper.ShowWarning(ex.Message);
            }

            try
            {
                // Load options.
                appOptionsLocator = new Locator<AppOptions>();
                appOptionsLocator.Instance = OptionManager.ReadOrCreateFile();
            }
            catch (Exception ex)
            {
                Log(ex);
                MsgBoxHelper.ShowError("Cannot load options. The application will quit now.");
                Environment.Exit(1);
            }

            try
            {
                InitAirportAndWaypoints();
            }
            catch (Exception ex)
            {
                Log(ex);
                failedToLoadNavDataAtStartUp = true;

                countryCodesLocator = new Locator<CountryCodeManager>(null);
                airwayNetwork = new AirwayNetwork(
                    new DefaultWaypointList(), new DefaultAirportManager());
            }

            procFilter = new ProcedureFilter();
            windTableLocator = new Locator<IWindTableCollection>();
            windTableLocator.Instance = new DefaultWindTableCollection();
            updater = new Updater();
        }

        /// <exception cref="Exception"></exception>
        private void InitAirportAndWaypoints()
        {
            string navDataPath = appSettings.NavDataLocation;
            var airportTxtPath = Path.Combine(navDataPath, "Airports.txt");

            var airportList = new AirportDataLoader(airportTxtPath)
                .LoadFromFile();

            var result = new WptListLoader(navDataPath).LoadFromFile();
            countryCodesLocator = result.CountryCodes.ToLocator();
            airwayNetwork = new AirwayNetwork(result.WptList, airportList);
        }

        private void InitControls()
        {
            LoadSavedState();
            CheckRegistry();
            SubscribeEvents();

            acMenu.Init(profiles);
            acMenu.AircraftsChanged += fuelMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += toMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += ldgMenu.RefreshAircrafts;

            fuelMenu.Init(
                appOptionsLocator,
                airwayNetwork,
                procFilter,
                countryCodesLocator,
                windTableLocator,
                profiles.AcConfigs,
                profiles.FuelData);

            toMenu.Init(
                profiles.AcConfigs,
                profiles.TOTables.ToList(),
                airportList,
                () => fuelMenu.AircraftRequest);

            toMenu.TryLoadState();

            ldgMenu.Init(
                profiles.AcConfigs,
                profiles.LdgTables.ToList(),
                airportList,
                () => fuelMenu.AircraftRequest);

            ldgMenu.TryLoadState();

            InitMiscInfoMenu();
            RefreshAirportInfoSelection();

            fuelMenu.AltnControl.AlternatesChanged += (s, e) => RefreshAirportInfoSelection();

            fuelMenu.AircraftRequestChanged += (s, e) =>
            {
                var showReqBtn = fuelMenu.AircraftRequest != null;
                toMenu.requestBtn.Visible = showReqBtn;
                ldgMenu.requestBtn.Visible = showReqBtn;
            };

            airwayNetwork.AirportListChanged += (s, e) =>
            {
                fuelMenu.RefreshForAirportListChange();
                toMenu.Airports = airportList;
                ldgMenu.Airports = airportList;
                miscInfoMenu.AirportList = airportList;
            };

            airwayNetwork.WptListChanged += (s, e) => fuelMenu.OnWptListChanged();

            aboutMenu.Init("QSimPlanner");
            navBar.Init(acMenu, fuelMenu, toMenu, ldgMenu, miscInfoMenu, aboutMenu, panel1, panel2);

            FormClosing += CloseMain;
        }

        private void RefreshAirportInfoSelection()
        {
            miscInfoMenu.SetAltn(fuelMenu.AltnControl.Alternates);
        }

        private void InitMiscInfoMenu()
        {
            Func<IEnumerable<string>> altnGetter = () =>
                fuelMenu.AltnControl.Controls.Select(c => c.IcaoTxtBox.Text.Trim().ToUpper());

            miscInfoMenu.Init(
                airportList,
                windTableLocator,
                true,
                () => fuelMenu.origTxtBox.Text.Trim().ToUpper(),
                () => fuelMenu.destTxtBox.Text.Trim().ToUpper(),
                altnGetter);
        }

        private void SubscribeEvents()
        {
            var origTxtBox = fuelMenu.origTxtBox;

            origTxtBox.TextChanged += (sender, e) =>
            {
                miscInfoMenu.SetOrig(origTxtBox.Text.Trim().ToUpper());
            };

            var destTxtBox = fuelMenu.destTxtBox;

            destTxtBox.TextChanged += (sender, e) =>
            {
                miscInfoMenu.SetDest(destTxtBox.Text.Trim().ToUpper());
            };

            EnableAirportRequests();
            navDataStatusLabel.Click += ViewOptions;
            navDataStatusLabel.MouseEnter += SetHandCursor;
            navDataStatusLabel.MouseLeave += SetDefaultCursor;
            windDataStatusLabel.Click += windDataStatusLabel_Click;
            windDataStatusLabel.MouseEnter += SetHandCursor;
            windDataStatusLabel.MouseLeave += SetDefaultCursor;
            trackStatusLabel.Click += (s, e) => trackFrm.ShowDialog();
            trackStatusLabel.MouseEnter += SetHandCursor;
            trackStatusLabel.MouseLeave += SetDefaultCursor;
            navBar.OptionLbl.Click += (s, e) => ShowOptionsForm();
        }

        private void EnableAirportRequests()
        {
            var toControl = toMenu.airportInfoControl;
            toControl.reqAirportBtn.Visible = true;
            toControl.reqAirportBtn.Click += (s, e) =>
            {
                toControl.airportTxtBox.Text = fuelMenu.origTxtBox.Text;
                toControl.rwyComboBox.Text = fuelMenu.origRwyComboBox.Text;
            };

            var ldgControl = ldgMenu.airportInfoControl;
            ldgControl.reqAirportBtn.Visible = true;
            ldgControl.reqAirportBtn.Click += (s, e) =>
            {
                ldgControl.airportTxtBox.Text = fuelMenu.destTxtBox.Text;
                ldgControl.rwyComboBox.Text = fuelMenu.destRwyComboBox.Text;
            };
        }

        private void AddControls()
        {
            acMenu = new AircraftMenuControl();
            fuelMenu = new FuelPlanningControl();
            toMenu = new TOPerfControl();
            ldgMenu = new LandingPerfControl();
            miscInfoMenu = new MiscInfoControl();
            aboutMenu = new AboutPageControl();

            foreach (var i in Pages)
            {
                i.Location = Point.Empty;
                i.Visible = i == acMenu;
                panel2.Controls.Add(i);
            }
        }

        private static void CheckRegistry()
        {
            // Try to check/add registry so that google map works properly. 
            var regChecker = new IeEmulationChecker();

            try
            {
#if DEBUG
                regChecker.DebugRun();
#endif
                regChecker.Run();
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private void ViewOptions(object sender, EventArgs e)
        {
            ShowOptionsForm();
        }

        private void SetHandCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void SetDefaultCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void DownloadTracksIfNeeded()
        {
            trackStatusLabel.Image = Properties.Resources.YellowLight;
            trackStatusLabel.Text = "Tracks: Not downloaded";
            if (appSettings.AutoDLTracks) trackFrm.DownloadAndEnableTracks();
        }

        private async Task DownloadWindIfNeeded()
        {
            if (appSettings.AutoDLWind)
            {
                await windFrm.DownloadWind();
            }
        }

        private void CloseMain(object sender, CancelEventArgs e)
        {
            if (appSettings.PromptBeforeExit)
            {
                var result = MsgBoxHelper.ShowDialog(
                    "Exit the application?",
                    MsgBoxIcon.Info,
                    "",
                    DefaultButton.Button1,
                    "Quit", "Cancel");

                if (result != MsgBoxResult.Button1)
                {
                    // Do not exit the app.
                    e.Cancel = true;
                    return;
                }
            }

            if (updater.IsUpdating)
            {
                var result = MsgBoxHelper.ShowDialog(
                    "The automatic update is in progress. " +
                    "Wait for the update to finish?",
                    MsgBoxIcon.Info,
                    "",
                    DefaultButton.Button1,
                    "Wait", "Quit", "Cancel");
                
                if ( result != MsgBoxResult.Button2)
                {
                    // Do not exit the app.
                    e.Cancel = true;
                    return;
                }
            }

            toMenu.TrySaveState();
            ldgMenu.TrySaveState();
            fuelMenu.SaveStateToFile();
            SaveStateToFile();
        }

        private void windDataStatusLabel_Click(object sender, EventArgs e)
        {
            windFrm.ShowDialog();
        }

        private void ShowOptionsForm(
            FormStartPosition position = FormStartPosition.CenterParent,
            bool showInTaskbar = false,
            bool autoDetectAiracFolder = false)
        {
            using (var frm = new OptionsForm())
            {
                frm.Init(
                   trackFrm,
                   airwayNetwork,
                   countryCodesLocator,
                   appOptionsLocator,
                   updater,
                   autoDetectAiracFolder);

                frm.NavDataLocationChanged += (s, e) => fuelMenu.RefreshForNavDataLocationChange();

                frm.ShowInTaskbar = showInTaskbar;
                frm.StartPosition = position;
                frm.ShowDialog();
            }
        }

        private void LoadSavedState()
        {
            try
            {
                var serializer = new WindowSize.Serializer();
                var text = XDocument.Load(WindowSize.FileLocation);
                var state = serializer.Deserialize(text.Root);
                SetWindowSize(state);
            }
            catch (Exception ex)
            {
                Log(ex);
                SetWindowSize(WindowSize.Default);
            }
        }

        private void SetWindowSize(WindowSize state)
        {
            if (state.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                Width = state.WindowWidth;
                Height = state.WindowHeight;
            }
        }

        public void SaveStateToFile()
        {
            try
            {
                var serializer = new WindowSize.Serializer();
                var state = new WindowSize(
                    WindowState == FormWindowState.Maximized,
                    Width,
                    Height);

                var doc = new XDocument(serializer.Serialize(state, "root"));
                File.WriteAllText(WindowSize.FileLocation, doc.ToString());
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        // TODO: Some ideas for future:
        // (1) Flightaware flight plans
        // (2) NOAA temp./wind/sigWx charts
        //     http://aviationweather.gov/webiffdp/page/public?name=iffdp_main
        //     http://aviationweather.gov/iffdp/sgwx
        // (3) Charts (FAA, Eurocontrol)
    }
}
