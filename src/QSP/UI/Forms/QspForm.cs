﻿using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.NavData.AAX;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Containers.CountryCode;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AboutPage;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.AirportMap;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.UserControls;
using QSP.UI.Utilities;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.UI.Controllers.ButtonGroup.BtnGroupController;
using static QSP.UI.Controllers.ButtonGroup.ControlSwitcher;
using static QSP.UI.Factories.ToolTipFactory;
using static QSP.Utilities.LoggerInstance;
using QSP.WindAloft;
using System.Threading.Tasks;

namespace QSP.UI.Forms
{
    public partial class QspForm : Form
    {
        private AircraftMenuControl acMenu;
        private FuelPlanningControl fuelMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private AirportMapControl airportMenu;
        private OptionsControl optionsMenu;
        private AboutPageControl aboutMenu;

        private ProfileManager profiles;
        private AppOptions appSettings;
        private AirportManager airportList;
        private WaypointList wptList;
        private CountryCodeManager countryCodes;
        private ProcedureFilter procFilter;
        private TrackInUseCollection tracksInUse = new TrackInUseCollection(); // TODO: Initialize?
        private WindTableCollection windTables;

        private BtnGroupController btnControl;
        private ControlSwitcher viewControl;
        private Point controlDefaultLocation = new Point(12, 52);

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
                    airportMenu,
                    optionsMenu,
                    aboutMenu
                };
            }
        }

        public QspForm()
        {
            InitializeComponent();
            AddControls();
        }

        public void Init()
        {
            ShowSplashWhile(async () =>
            {
                InitData();
                InitControls();
                DownloadTracksIfNeeded();
                await DownloadWindIfNeeded();
                //InitRouteFinderSelections();

                //TODO: track in use is wrong
                //advancedRouteTool.Init(
                //    AppSettings,
                //    wptList,
                //    airportList,
                //    new TrackInUseCollection(),
                //    new ProcedureFilter(),
                //    countryCodes);
            });
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

            // Load options.
            try
            {
                appSettings = OptionManager.ReadOrCreateFile();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                MsgBoxHelper.ShowError("Cannot load options.");
            }

            // Airports and waypoints
            // TODO: exceptions?
            try
            {
                InitAirportList();
                InitWptList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            procFilter = new ProcedureFilter();
        }

        private void InitAirportList()
        {
            string navDataPath = appSettings.NavDataLocation;

            airportList =
            new AirportManager(
                new AirportDataLoader(navDataPath + @"\Airports.txt")
                .LoadFromFile());
        }

        private void InitWptList()
        {
            string navDataPath = appSettings.NavDataLocation;

            var result = new WptListLoader(navDataPath)
                .LoadFromFile();

            wptList = result.WptList;
            countryCodes = result.CountryCodes;
        }

        // To prevent AutoScroll from change ScrollBar's value property.
        protected override Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }

        private void InitControls()
        {
            AutoScroll = true;

            CheckRegistry();
            SubscribeEvents();
            optionsMenu.Init(appSettings);

            acMenu.Initialize(profiles);
            acMenu.AircraftsChanged += toMenu.RefreshAircrafts;
            acMenu.AircraftsChanged += ldgMenu.RefreshAircrafts;

            fuelMenu.Init(
                appSettings,
                wptList,
                airportList,
                tracksInUse,
                procFilter,
                countryCodes,
                windTables,          
                profiles.AcConfigs, 
                profiles.FuelData);

            toMenu.Initialize(profiles.AcConfigs,
                profiles.TOTables.ToList(), airportList);
            toMenu.TryLoadState();

            ldgMenu.InitializeAircrafts(profiles.AcConfigs,
                profiles.LdgTables.ToList(), airportList);
            ldgMenu.TryLoadState();

            airportMenu.Initialize(airportList);
            airportMenu.BrowserEnabled = true;

            EnableBtnColorControls();
            EnableViewControl();
            AddToolTip();
        }

        private void AddToolTip()
        {
            var tp = GetToolTip();
            tp.SetToolTip(optionsBtn, "Options");
            tp.SetToolTip(aboutBtn, "About");
        }

        //private void RefreshItemsRequireAirportList()
        //{
        //    Airports = airportList;
        //    ToMenu.airportInfoControl.RefreshAirportInfo();
        //    LdgMenu.airportInfoControl.RefreshAirportInfo();
        //    AirportMenu.FindAirport();
        //}

        private void SubscribeEvents()
        {
            optionsMenu.AppSettingChanged += (sender, e) =>
            {
                appSettings = optionsMenu.AppSettings;
            };

            optionsMenu.NavDataUpdated += (sender, e) =>
            {
                // TODO: Update Nav data here.
            };

            var origTxtBox = toMenu.airportInfoControl.airportTxtBox;

            origTxtBox.TextChanged += (sender, e) =>
            {
                airportMenu.Orig = origTxtBox.Text;
            };

            var destTxtBox = ldgMenu.airportInfoControl.airportTxtBox;

            destTxtBox.TextChanged += (sender, e) =>
            {
                airportMenu.Dest = destTxtBox.Text;
            };

            navDataStatusLabel.Click += ViewOptions;
            navDataStatusLabel.MouseEnter += SetHandCursor;
            navDataStatusLabel.MouseLeave += SetDefaultCursor;
            windDataStatusLabel.Click += async (s, e) => await DownloadWind();            
            windDataStatusLabel.MouseEnter += SetHandCursor;
            windDataStatusLabel.MouseLeave += SetDefaultCursor;
        }
        
        private void EnableViewControl()
        {
            viewControl = new ControlSwitcher(
                new BtnControlPair(acConfigBtn, acMenu),
                new BtnControlPair(fuelBtn, fuelMenu),
                new BtnControlPair(toBtn, toMenu),
                new BtnControlPair(ldgBtn, ldgMenu),
                new BtnControlPair(airportBtn, airportMenu),
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
            airportMenu = new AirportMapControl();
            optionsMenu = new OptionsControl();
            aboutMenu = new AboutPageControl();

            foreach (var i in Pages)
            {
                i.Location = controlDefaultLocation;
                i.Visible = i == acMenu;
                Controls.Add(i);
            }
        }

        //private void ResizeForm()
        //{
        //    int right = Pages.MaxBy(c => c.Width).Right;
        //    int bottom = Pages.MaxBy(c => c.Height).Bottom;

        //    var newSize = new Size(right + 15, bottom + 15);
        //    MoveControls(newSize);
        //    ClientSize = newSize;
        //}

        //private void MoveControls(Size newSize)
        //{
        //    int amount = ClientSize.Width - newSize.Width;
        //    MoveLeft(tableLayoutPanel2, amount);
        //    MoveLeft(tableLayoutPanel3, amount);
        //}

        //private static void MoveLeft(Control c, int amount)
        //{
        //    var location = new Point(c.Location.X - amount, c.Location.Y);
        //    c.Location = location;
        //}

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

        private async Task DownloadWind()
        {
            ShowWindStatus(WindDownloadStatus.Downloading);

            try
            {
                windTables = await WindManager.LoadWindAsync();
                ShowWindStatus(WindDownloadStatus.Finished);
            }
            catch (Exception ex) when (
                ex is ReadWindFileException ||
                ex is DownloadGribFileException)
            {
                WriteToLog(ex);
                ShowWindStatus(WindDownloadStatus.Failed);
            }
        }

        public enum WindDownloadStatus
        {
            Downloading,
            Finished,
            Failed,
            WaitingManualDownload
        }

        public void ShowWindStatus(WindDownloadStatus item)
        {
            var w = windDataStatusLabel;

            switch (item)
            {
                case WindDownloadStatus.Downloading:
                    w.Text = "Downloading lastest wind ...";
                    w.Image = null;
                    break;

                case WindDownloadStatus.Finished:
                    w.Text = "Lastest wind ready";
                    w.Image = Properties.Resources.GreenLight;
                    break;

                case WindDownloadStatus.Failed:
                    w.Text = "Failed to download wind data";
                    w.Image = Properties.Resources.RedLight;
                    break;

                case WindDownloadStatus.WaitingManualDownload:
                    w.Text = "Click here to download wind data";
                    w.Image = Properties.Resources.YellowLight;
                    break;
            }
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
                await DownloadWind();
            }
            else
            {
                ShowWindStatus(WindDownloadStatus.WaitingManualDownload);
            }
        }
    }
}