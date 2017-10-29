using CommonLibrary.LibraryExtension;
using QSP.AircraftProfiles;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.Metar;
using QSP.NavData;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Forms.Options;
using QSP.UI.MsgBox;
using QSP.UI.UserControls;
using QSP.UI.UserControls.AircraftMenu;
using QSP.UI.UserControls.TakeoffLanding.LandingPerf;
using QSP.UI.UserControls.TakeoffLanding.TOPerf;
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
        private OptionsForm optionsForm;
        private readonly MetarCache metarCache = new MetarCache();

        private TracksForm trackFrm;
        private WindDataForm windFrm;
        private bool failedToLoadNavDataAtStartUp = false;

        private AppOptions AppSettings => appOptionsLocator.Instance;
        private AirportManager AirportList => airwayNetwork.AirportList;

        private IReadOnlyList<UserControl> Pages => new UserControl[]
        {
            acMenu,
            fuelMenu,
            toMenu,
            ldgMenu,
            miscInfoMenu,
            aboutMenu
        };

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
                InitOptionsForm();
                DownloadWindIfNeeded();
                DownloadTracksIfNeeded();
                PrepareForUserInteration();
            });

            if (failedToLoadNavDataAtStartUp)
            {
                if (AppSettings.NavDataLocation == AppOptions.Default.NavDataLocation)
                {
                    // User did not set the path. 
                    // Maybe its the first time the app starts.
                    MsgBoxHelper.ShowDialog(
                        null,
                        "Please set the correct Nav Data location " +
                        "before using the application.",
                        MsgBoxIcon.Info,
                        "",
                        DefaultButton.Button1,
                        "OK");
                }
                else
                {
                    this.ShowWarning("Please set the correct Nav Data location.");
                }

                ShowOptionsForm(FormStartPosition.CenterScreen, true, true);
            }

            DoubleBufferUtil.SetDoubleBuffered(panel1);
        }

        private void InitOptionsForm()
        {
            optionsForm = new OptionsForm();
            optionsForm.Init(
               trackFrm,
               airwayNetwork,
               countryCodesLocator,
               appOptionsLocator,
               updater);

            optionsForm.NavDataLocationChanged += (s, e) =>
                fuelMenu.RefreshForNavDataLocationChange();
        }

        private void ShowOptionsForm(
            FormStartPosition position = FormStartPosition.CenterParent,
            bool showInTaskbar = false,
            bool autoDetectAiracFolder = false)
        {
            optionsForm.ShowInTaskbar = showInTaskbar;
            optionsForm.StartPosition = position;
            optionsForm.SetControlsAsInOptions();
            if (autoDetectAiracFolder) optionsForm.DetectAndSetAiracFolder();
            optionsForm.ShowDialog();
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
                this.ShowWarning(
                    "An error occurred when copying profiles for the updated verison. Please " +
                    "back up any custom aircraft performance profiles to prevent data loss." +
                    $"\n\n(Error message:{ex.ToString()}");
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
            using (var splash = new Splash())
            {
                splash.Show();
                splash.ShowVersion();
                splash.Refresh();

                action();

                splash.Close();
            }
        }

        private void InitData()
        {
            // Aircraft data
            profiles = new ProfileCollection();
            var err = profiles.Initialize().ToList();
            if (err.Count > 0)
            {
                this.ShowWarning(string.Join("\n", err), "Performance file loading warning");
            }

            try
            {
                // Load options.
                appOptionsLocator = new Locator<AppOptions>()
                {
                    Instance = OptionManager.ReadOrCreateFile()
                };
            }
            catch (Exception ex)
            {
                Log(ex);
                this.ShowError("Cannot load options. The application will quit now.");
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
            windTableLocator = new Locator<IWindTableCollection>()
            {
                Instance = new DefaultWindTableCollection()
            };
            updater = new Updater();
        }

        /// <exception cref="Exception"></exception>
        private void InitAirportAndWaypoints()
        {
            string navDataPath = AppSettings.NavDataLocation;
            var airportTxtPath = Path.Combine(navDataPath, "Airports.txt");

            var airportResult = AirportDataLoader.LoadFromFile(airportTxtPath);
            var err = airportResult.Errors;
            if (err.Any()) Log(ReadFileErrorMsg.ErrorMsg(err, "ats.txt"));
            var airports = airportResult.Airports;

            var result = new WptListLoader(navDataPath).LoadFromFile();
            countryCodesLocator = result.CountryCodes.ToLocator();
            airwayNetwork = new AirwayNetwork(result.WptList, airports);
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
                profiles.FuelData,
                metarCache);

            toMenu.Init(
                profiles.AcConfigs,
                profiles.TOTables.ToList(),
                AirportList,
                () => fuelMenu.AircraftRequest);

            toMenu.TryLoadState();

            ldgMenu.Init(
                profiles.AcConfigs,
                profiles.LdgTables.ToList(),
                AirportList,
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
                toMenu.Airports = AirportList;
                ldgMenu.Airports = AirportList;
                miscInfoMenu.AirportList = AirportList;
            };

            airwayNetwork.WptListChanged += (s, e) => fuelMenu.OnWptListChanged();

            aboutMenu.Init("QSimPlanner");
            navBar.Init(acMenu, fuelMenu, toMenu, ldgMenu, miscInfoMenu, aboutMenu, panel2);

            FormClosing += CloseMain;
            new ScrollBarWorkaround(panel1).Enable();
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
                AirportList,
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
            SetCursorStatusLabel();
            navDataStatusLabel.Click += (s, e) => ShowOptionsForm();
            windDataStatusLabel.Click += (s, e) => windFrm.ShowDialog();
            trackStatusLabel.Click += (s, e) => trackFrm.ShowDialog();
            navBar.OptionLbl.Click += (s, e) => ShowOptionsForm();
        }

        private void PrepareForUserInteration()
        {
            ScrollBarsUtil.OverrideScrollBar(panel1, this);
        }

        private void SetCursorStatusLabel()
        {
            new[] { navDataStatusLabel, windDataStatusLabel, trackStatusLabel }.ForEach(i =>
            {
                i.MouseEnter += (s, e) => Cursor = Cursors.Hand;
                i.MouseLeave += (s, e) => Cursor = Cursors.Default;
            });
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

        private void DownloadTracksIfNeeded()
        {
            trackStatusLabel.Image = Properties.Resources.YellowLight;
            trackStatusLabel.Text = "Tracks: Not downloaded";
            if (AppSettings.AutoDLTracks) trackFrm.DownloadAndEnableTracks();
        }

        private async Task DownloadWindIfNeeded()
        {
            if (AppSettings.AutoDLWind)
            {
                await windFrm.DownloadWind();
            }
        }

        private void CloseMain(object sender, CancelEventArgs e)
        {
            if (AppSettings.PromptBeforeExit)
            {
                var result = this.ShowDialog(
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
                var result = this.ShowDialog(
                    "The automatic update is in progress. Wait for the update to finish?",
                    MsgBoxIcon.Info,
                    "",
                    DefaultButton.Button1,
                    "Wait", "Quit", "Cancel");

                if (result != MsgBoxResult.Button2)
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
                var state = new WindowSize(WindowState == FormWindowState.Maximized,
                    Width, Height);

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
