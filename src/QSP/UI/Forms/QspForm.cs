using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.NavData.AAX;
using QSP.RouteFinding;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AboutPage;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.UserControls;
using QSP.UI.Utilities;
using QSP.Utilities;
using QSP.WindAloft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.UI.Controllers.ButtonGroup.BtnGroupController;
using static QSP.UI.Controllers.ButtonGroup.ControlSwitcher;
using static QSP.UI.Factories.ToolTipFactory;
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
        private OptionsControl optionsMenu;
        private AboutPageControl aboutMenu;

        private ProfileManager profiles;
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private Locator<CountryCodeManager> countryCodesLocator;
        private ProcedureFilter procFilter;
        private Locator<IWindTableCollection> windTableLocator;

        private BtnGroupController btnControl;
        private ControlSwitcher viewControl;
        private readonly Point controlDefaultLocation = new Point(12, 52);
        private TracksForm trackFrm;
        private WindDataForm windFrm;

        private AppOptions appSettings
        { get { return appOptionsLocator.Instance; } }

        private AirportManager airportList
        { get { return airwayNetwork.AirportList; } }

        private WaypointList wptList
        { get { return airwayNetwork.WptList; } }

        private TrackInUseCollection tracksInUse
        { get { return airwayNetwork.TracksInUse; } }

        private CountryCodeManager countryCodes
        { get { return countryCodesLocator.Instance; } }

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
                    optionsMenu,
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
            ShowSplashWhile(async () =>
            {
                AddControls();
                InitData();
                InitControls();
                DownloadTracksIfNeeded();
                await DownloadWindIfNeeded();
                InitTrackForm();
                InitWindForm();
            });
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
                profiles = new ProfileManager();
                profiles.Initialize();
            }
            catch (PerfFileNotFoundException ex)
            {
                WriteToLog(ex);
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
                WriteToLog(ex);
                MsgBoxHelper.ShowError(
                    "Cannot load options. The application will quit now.");
                Environment.Exit(1);
            }

            // TODO: If failed, should show user the options page.
            try
            {
                InitAirportAndWaypoints();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                MsgBoxHelper.ShowError(ex.Message +
                    " The application will quit now.");
            }

            procFilter = new ProcedureFilter();
            windTableLocator = new Locator<IWindTableCollection>();
            windTableLocator.Instance = new DefaultWindTableCollection();
        }

        /// <exception cref="RwyDataFormatException"></exception>
        /// <exception cref="ReadAirportFileException"></exception>
        /// <exception cref="WaypointFileReadException"></exception>
        /// <exception cref="LoadCountryNamesException"></exception>
        private void InitAirportAndWaypoints()
        {
            string navDataPath = appSettings.NavDataLocation;

            var airportList = new AirportManager(
                new AirportDataLoader(navDataPath + @"\Airports.txt")
                .LoadFromFile());

            var result = new WptListLoader(navDataPath)
                .LoadFromFile();

            countryCodesLocator =
                new Locator<CountryCodeManager>(result.CountryCodes);

            airwayNetwork = new AirwayNetwork(result.WptList, airportList);
        }

        private void InitControls()
        {
            CheckRegistry();
            SubscribeEvents();

            optionsMenu.Init(
                airwayNetwork,
                countryCodesLocator,
                appOptionsLocator);

            acMenu.Initialize(profiles);
            acMenu.AircraftsChanged += fuelMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += toMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += ldgMenu.RefreshAircrafts;

            fuelMenu.Init(
                appOptionsLocator,
                airwayNetwork,
                procFilter,
                countryCodes,
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

            fuelMenu.altnControl.AlternatesChanged += (s, e) =>
            RefreshAirportInfoSelection();

            fuelMenu.AircraftRequestChanged += (s, e) =>
            {
                var showReqBtn = fuelMenu.AircraftRequest != null;
                toMenu.requestBtn.Visible = showReqBtn;
                ldgMenu.requestBtn.Visible = showReqBtn;
            };

            airwayNetwork.AirportListChanged += (s, e) =>
            {
                fuelMenu.RefreshForAirportListChange();
                toMenu.Airports = airwayNetwork.AirportList;
                ldgMenu.Airports = airwayNetwork.AirportList;
                miscInfoMenu.AirportList = airwayNetwork.AirportList;
            };

            optionsMenu.NavDataLocationChanged += (s, e) =>
            fuelMenu.RefreshForNavDataLocationChange();

            EnableBtnColorControls();
            EnableViewControl();
            AddToolTip();

            FormClosing += CloseMain;
            panel1.HorizontalScroll.Enabled = false;
            panel1.HorizontalScroll.Visible = false;
        }

        private void RefreshAirportInfoSelection()
        {
            miscInfoMenu.SetAltn(fuelMenu.altnControl.Alternates);
        }

        private void InitMiscInfoMenu()
        {
            Func<IEnumerable<string>> altnGetter = () =>
            fuelMenu.altnControl.Controls
            .Select(c => c.IcaoTxtBox.Text.Trim().ToUpper());

            miscInfoMenu.Init(
                airportList,
                windTableLocator,
                true,
                () => fuelMenu.origTxtBox.Text.Trim().ToUpper(),
                () => fuelMenu.destTxtBox.Text.Trim().ToUpper(),
                altnGetter);
        }

        private void AddToolTip()
        {
            var tp = GetToolTip();
            tp.SetToolTip(optionsBtn, "Options");
            tp.SetToolTip(aboutBtn, "About");
        }

        private void SubscribeEvents()
        {
            // TODO: ?
            var origTxtBox = toMenu.airportInfoControl.airportTxtBox;

            origTxtBox.TextChanged += (sender, e) =>
            {
                miscInfoMenu.SetOrig(origTxtBox.Text.Trim().ToUpper());
            };

            var destTxtBox = ldgMenu.airportInfoControl.airportTxtBox;

            destTxtBox.TextChanged += (sender, e) =>
            {
                miscInfoMenu.SetDest(destTxtBox.Text.Trim().ToUpper());
            };

            navDataStatusLabel.Click += ViewOptions;
            navDataStatusLabel.MouseEnter += SetHandCursor;
            navDataStatusLabel.MouseLeave += SetDefaultCursor;
            windDataStatusLabel.Click += windDataStatusLabel_Click;
            windDataStatusLabel.MouseEnter += SetHandCursor;
            windDataStatusLabel.MouseLeave += SetDefaultCursor;
            trackStatusLabel.Click += (s, e) => trackFrm.ShowDialog();
            trackStatusLabel.MouseEnter += SetHandCursor;
            trackStatusLabel.MouseLeave += SetDefaultCursor;
        }

        private void EnableViewControl()
        {
            viewControl = new ControlSwitcher(
                new BtnControlPair(acConfigBtn, acMenu),
                new BtnControlPair(fuelBtn, fuelMenu),
                new BtnControlPair(toBtn, toMenu),
                new BtnControlPair(ldgBtn, ldgMenu),
                new BtnControlPair(airportBtn, miscInfoMenu),
                new BtnControlPair(optionsBtn, optionsMenu),
                new BtnControlPair(aboutBtn, aboutMenu));

            viewControl.Subscribed = true;
        }

        private void EnableBtnColorControls()
        {
            var acConfigPair = new BtnColorPair(acConfigBtn, Color.Black,
                Color.WhiteSmoke, Color.White, Color.FromArgb(192, 0, 0));

            var fuelPair = new BtnColorPair(fuelBtn, Color.Black,
               Color.WhiteSmoke, Color.White, Color.DarkOrange);

            var toPair = new BtnColorPair(toBtn, Color.Black,
                Color.WhiteSmoke, Color.White, Color.ForestGreen);

            var ldgPair = new BtnColorPair(ldgBtn, Color.Black,
            Color.WhiteSmoke, Color.White, Color.FromArgb(0, 170, 170));

            var airportPair = new BtnColorPair(airportBtn, Color.Black,
            Color.WhiteSmoke, Color.White, Color.DodgerBlue);

            var optionPair = new BtnColorPair(optionsBtn, Color.Black,
            Color.Black, Color.White, Color.Purple);

            var aboutPair = new BtnColorPair(aboutBtn, Color.Black,
            Color.Black, Color.White, Color.Turquoise);

            btnControl = new BtnGroupController(
                acConfigPair,
                fuelPair,
                toPair,
                ldgPair,
                airportPair,
                optionPair,
                aboutPair);

            btnControl.Initialize();
            btnControl.SetSelected(acConfigBtn);
        }

        private void AddControls()
        {
            acMenu = new AircraftMenuControl();
            fuelMenu = new FuelPlanningControl();
            toMenu = new TOPerfControl();
            ldgMenu = new LandingPerfControl();
            miscInfoMenu = new MiscInfoControl();
            optionsMenu = new OptionsControl();
            aboutMenu = new AboutPageControl();

            foreach (var i in Pages)
            {
                i.Location = controlDefaultLocation;
                i.Visible = i == acMenu;
                panel1.Controls.Add(i);
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
                WriteToLog(ex);
            }
        }

        private void ViewOptions(object sender, EventArgs e)
        {
            optionsBtn.PerformClick();
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
            if (appSettings.AutoDLTracks)
            {
                //RouteFinding.Tracks.Interaction.Interactions.SetAllTracksAsync();
                //TODO: add code to start download tracks automatically.
            }
            else
            {
                trackStatusLabel.Image = Properties.Resources.YellowLight;
                trackStatusLabel.Text = "Tracks: Not downloaded";
            }
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
                var Result = MessageBox.Show(
                    "Exit the application?",
                    "",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (Result != DialogResult.Yes)
                {
                    // Do not exit the app.
                    e.Cancel = true;
                }
            }
        }

        private void windDataStatusLabel_Click(object sender, EventArgs e)
        {
            windFrm.ShowDialog();
        }

        // TODO: Some ideas for future:
        // (1) Flightaware flight plans
        // (2) NOAA temp./wind/sigWx charts
        //     http://aviationweather.gov/webiffdp/page/public?name=iffdp_main
        //     http://aviationweather.gov/iffdp/sgwx
        // (3) Charts (FAA, Eurocontrol)
    }
}
