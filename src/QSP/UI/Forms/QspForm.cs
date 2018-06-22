using QSP.AircraftProfiles;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.NavData;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Navaids;
using QSP.RouteFinding.TerminalProcedures;
using QSP.RouteFinding.Tracks;
using QSP.UI.Forms.Options;
using QSP.UI.Models;
using QSP.UI.Models.FuelPlan.Routes;
using QSP.UI.Models.MsgBox;
using QSP.UI.Models.Wind;
using QSP.UI.Presenters.MiscInfo;
using QSP.UI.Presenters.Wind;
using QSP.UI.UserControls;
using QSP.UI.UserControls.AircraftMenu;
using QSP.UI.UserControls.TakeoffLanding.LandingPerf;
using QSP.UI.UserControls.TakeoffLanding.TOPerf;
using QSP.UI.Util;
using QSP.UI.Util.ScrollBar;
using QSP.UI.Views;
using QSP.UI.Views.FuelPlan;
using QSP.UI.Views.MiscInfo;
using QSP.Updates;
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
    // Requirements:
    // * When NavData is changed,
    //   * Runway and SID/STAR list should refresh.
    //   * In advanced tool, the waypoints ComboBox should refresh.
    //   * Route finder should find results using the new NavData.
    //   * If a route has been found or analyzed before the NavData change,
    //     user should be required to find or analyze route again.
    //
    public partial class QspForm : Form
    {
        private AircraftMenuControl acMenu;
        private FuelPlanningControl fuelMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private MiscInfoControl miscInfoMenu;
        private TracksControl tracksContorl;
        private WindControl windControl;
        private AboutPageControl aboutMenu;

        private MiscInfoPresenter miscInfoPresenter;

        private ProfileCollection profiles;
        private AirwayNetwork airwayNetwork;
        private Locator<AppOptions> appOptionsLocator;
        private Locator<CountryCodeManager> countryCodesLocator;
        private ProcedureFilter procFilter;
        private Locator<IWxTableCollection> windTableLocator;
        private Updater updater;
        private OptionsForm optionsForm;

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
            tracksContorl,
            windControl,
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
                InitWindControl();
                InitOptionsForm();
                DownloadWindIfNeeded();
                DownloadTracksIfNeeded();
                PrepareForUserInteration();
            });

            if (failedToLoadNavDataAtStartUp)
            {
                this.ShowWarning("Please set the correct Nav Data location.");
                ShowOptionsForm(FormStartPosition.CenterScreen, true, true);
            }

            DoubleBufferUtil.SetDoubleBuffered(panel1);
        }

        private void InitOptionsForm()
        {
            optionsForm = new OptionsForm();
            optionsForm.Init(
               tracksContorl,
               airwayNetwork,
               countryCodesLocator,
               appOptionsLocator,
               updater);
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

        private void InitWindControl()
        {
            var presenter = new WindDataPresenter(windControl, windTableLocator);
            windControl.Init(windDataStatusLabel, presenter, WindDownloadStatus.WaitingManualDownload);
        }

        private void InitTrackControl()
        {
            tracksContorl.Init(airwayNetwork, trackStatusLabel);
        }

        private static void ShowSplashWhile(Action action)
        {
            using (var splash = new Splash())
            {
                splash.Show();
                splash.ShowVersion();
                splash.Refresh();

                action();

                splash.Hide();
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
                var o = OptionManager.ReadOrCreateFile();

                appOptionsLocator = new Locator<AppOptions>()
                {
                    Instance = OptionManager.TryDetectSimulatorPathsAndSave(o)
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
                    new DefaultWaypointList(), new DefaultAirportManager(),
                    new MultiMap<string, Navaid>());
            }

            procFilter = new ProcedureFilter();
            windTableLocator = new Locator<IWxTableCollection>()
            {
                Instance = new DefaultWxTableCollection()
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

            var navaids = NavaidsLoader.LoadFromFile(Path.Combine(navDataPath, "navaids.txt"));

            airwayNetwork = new AirwayNetwork(result.WptList, airports, navaids);
        }

        private void InitControls()
        {
            LoadSavedState();
            SubscribeEvents();

            acMenu.Init(profiles);
            acMenu.AircraftsChanged += fuelMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += toMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += ldgMenu.RefreshAircrafts;

            var fuelPlanningModel = new FuelPlanningModel()
            {
                AirwayNetwork = airwayNetwork,
                AppOption = appOptionsLocator,
                CountryCodeManager = countryCodesLocator,
                CheckedCountryCodes = new CountryCodeCollection().ToLocator(),
                ProcFilter = procFilter,
                WindTables = windTableLocator,
                Aircrafts = profiles.AcConfigs,
                FuelData = profiles.FuelData
            };

            fuelMenu.Init(fuelPlanningModel);

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

            fuelMenu.AltnPresenter.AlternatesChanged += (s, e) => RefreshAirportInfoSelection();

            fuelMenu.AircraftRequestChanged += (s, e) =>
            {
                var showReqBtn = fuelMenu.AircraftRequest != null;
                toMenu.requestBtn.Visible = showReqBtn;
                ldgMenu.requestBtn.Visible = showReqBtn;
            };

            airwayNetwork.NavDataChanged += (s, e) =>
            {
                fuelMenu.OnNavDataChange();
                toMenu.Airports = AirportList;
                ldgMenu.Airports = AirportList;
                miscInfoPresenter.AirportList = AirportList;
            };

            InitTrackControl();

            aboutMenu.Init("QSimPlanner");
            navBar.Init(acMenu, fuelMenu, toMenu, ldgMenu, 
                miscInfoMenu, tracksContorl, windControl, aboutMenu, panel2);

            FormClosing += CloseMain;
            new ScrollBarWorkaround(panel1).Enable();
            this.Text = $"QSimPlanner [v{Utilities.Version.AppProductVersion()}]";
        }

        private void RefreshAirportInfoSelection()
        {
            miscInfoPresenter.Altn = fuelMenu.AltnPresenter.Alternates;
        }

        private void InitMiscInfoMenu()
        {
            miscInfoPresenter = new MiscInfoPresenter(
                miscInfoMenu, AirportList, windTableLocator, true,
                () => fuelMenu.OrigIcao,
                () => fuelMenu.DestIcao,
                () => fuelMenu.AltnPresenter.Alternates);

            miscInfoMenu.Init(miscInfoPresenter);
        }

        private void SubscribeEvents()
        {
            var m = miscInfoPresenter;

            fuelMenu.OrigIcaoChanged += (sender, e) =>
            {
                if (m != null) m.Orig = fuelMenu.OrigIcao;
            };

            fuelMenu.DestIcaoChanged += (sender, e) =>
            {
                if (m != null) m.Dest = fuelMenu.DestIcao;
            };

            EnableAirportRequests();
            SetCursorStatusLabel();
            navDataStatusLabel.Click += (s, e) => ShowOptionsForm();
            windDataStatusLabel.Click += (s, e) => navBar.ShowWind();
            trackStatusLabel.Click += (s, e) => navBar.ShowTracks();
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
                toControl.airportTxtBox.Text = fuelMenu.OrigIcao;
                toControl.rwyComboBox.Text = fuelMenu.OrigRwy;
            };

            var ldgControl = ldgMenu.airportInfoControl;
            ldgControl.reqAirportBtn.Visible = true;
            ldgControl.reqAirportBtn.Click += (s, e) =>
            {
                ldgControl.airportTxtBox.Text = fuelMenu.DestIcao;
                ldgControl.rwyComboBox.Text = fuelMenu.DestRwy;
            };
        }

        private void AddControls()
        {
            acMenu = new AircraftMenuControl();
            fuelMenu = new FuelPlanningControl();
            toMenu = new TOPerfControl();
            ldgMenu = new LandingPerfControl();
            miscInfoMenu = new MiscInfoControl();
            tracksContorl = new TracksControl();
            windControl = new WindControl();
            aboutMenu = new AboutPageControl();

            foreach (var i in Pages)
            {
                i.Location = Point.Empty;
                i.Visible = i == acMenu;
                panel2.Controls.Add(i);
            }
        }

        private void DownloadTracksIfNeeded()
        {
            trackStatusLabel.Image = Properties.Resources.YellowLight;
            trackStatusLabel.Text = "Tracks: Not downloaded";
            if (AppSettings.AutoDLTracks) tracksContorl.DownloadAndEnableTracks();
        }

        private async Task DownloadWindIfNeeded()
        {
            if (AppSettings.AutoDLWind)
            {
                await windControl.DownloadWind();
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

                // This is needed because when the form is minimized, 
                // the Width and Height have the size of only the title bar.
                var bounds = RestoreBounds;

                var state = new WindowSize(WindowState == FormWindowState.Maximized,
                    bounds.Width, bounds.Height);

                var doc = new XDocument(serializer.Serialize(state, "root"));
                File.WriteAllText(WindowSize.FileLocation, doc.ToString());
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            SuspendLayout();
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            ResumeLayout();
            base.OnResizeEnd(e);
        }
    }
}
