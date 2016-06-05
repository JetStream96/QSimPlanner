using QSP.AircraftProfiles;
using QSP.RouteFinding.Airports;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.AirportMap;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.Options;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.ToLdgModule.AboutPage;
using QSP.Utilities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static QSP.UI.Controllers.ButtonGroup.BtnGroupController;
using static QSP.UI.Controllers.ButtonGroup.ControlSwitcher;

namespace QSP.UI.ToLdgModule.Forms
{
    public partial class QspLiteForm : Form
    {
        public AircraftMenuControl AcMenu { get; private set; }
        public TOPerfControl ToMenu { get; private set; }
        public LandingPerfControl LdgMenu { get; private set; }
        public AirportMapControl AirportMenu { get; private set; }
        public OptionsControl OptionsMenu { get; private set; }
        public AboutPageControl AboutMenu { get; private set; }

        private BtnGroupController btnControl;
        private ControlSwitcher viewControl;
        private AirportManager _airports;

        public QspLiteForm()
        {
            InitializeComponent();
            addControls();
        }

        public void Initialize(ProfileManager manager)
        {
            checkRegistry();
            subscribeEvents();
            OptionsMenu.Initialize();

            var airports = OptionsMenu.Airports;
            AcMenu.Initialize(manager);
            AcMenu.AircraftsChanged += ToMenu.RefreshAircrafts;
            AcMenu.AircraftsChanged += LdgMenu.RefreshAircrafts;

            ToMenu.Initialize(manager.AcConfigs,
                manager.TOTables.ToList(), airports);
            ToMenu.TryLoadState();

            LdgMenu.InitializeAircrafts(manager.AcConfigs,
                manager.LdgTables.ToList(), airports);
            LdgMenu.TryLoadState();

            AirportMenu.Initialize(airports);
            AirportMenu.BrowserEnabled = true;

            enableBtnColorControls();
            enableViewControl();
            addToolTip();
        }

        private void addToolTip()
        {
            var tp = new ToolTip();
            
            tp.AutoPopDelay = 5000;
            tp.InitialDelay = 1000;
            tp.ReshowDelay = 500;
            tp.ShowAlways = true;

            tp.SetToolTip(optionsBtn, "Options");
            tp.SetToolTip(aboutBtn, "About");
        }

        private void refreshItemsRequireAirportList()
        {
            Airports = OptionsMenu.Airports;
            ToMenu.airportInfoControl.RefreshAirportInfo();
            LdgMenu.airportInfoControl.RefreshAirportInfo();
            AirportMenu.FindAirport();
        }

        private void subscribeEvents()
        {
            OptionsMenu.SaveAirportsCompleted += (sender, e) =>
            {
                refreshItemsRequireAirportList();
            };

            var origTxtBox = ToMenu.airportInfoControl.airportTxtBox;

            origTxtBox.TextChanged += (sender, e) =>
            {
                AirportMenu.Orig = origTxtBox.Text;
            };

            var destTxtBox = LdgMenu.airportInfoControl.airportTxtBox;

            destTxtBox.TextChanged += (sender, e) =>
            {
                AirportMenu.Dest = destTxtBox.Text;
            };
        }

        public AirportManager Airports
        {
            get
            {
                return _airports;
            }
            set
            {
                _airports = value;
                ToMenu.Airports = _airports;
                LdgMenu.Airports = _airports;
                AirportMenu.Airports = _airports;
                OptionsMenu.Airports = _airports;
            }
        }

        private void enableViewControl()
        {
            viewControl = new ControlSwitcher(
                new BtnControlPair(acConfigBtn, AcMenu),
                new BtnControlPair(toBtn, ToMenu),
                new BtnControlPair(ldgBtn, LdgMenu),
                new BtnControlPair(airportBtn, AirportMenu),
                new BtnControlPair(optionsBtn, OptionsMenu),
                new BtnControlPair(aboutBtn, AboutMenu));

            viewControl.Subscribed = true;
        }

        private void enableBtnColorControls()
        {
            var acConfigPair = new BtnColorPair(acConfigBtn, Color.Black,
                Color.WhiteSmoke, Color.White, Color.FromArgb(192, 0, 0));

            var toPair = new BtnColorPair(toBtn, Color.Black,
                Color.WhiteSmoke, Color.White, Color.DarkOrange);

            var ldgPair = new BtnColorPair(ldgBtn, Color.Black,
            Color.WhiteSmoke, Color.White, Color.ForestGreen);

            var airportPair = new BtnColorPair(airportBtn, Color.Black,
            Color.WhiteSmoke, Color.White, Color.DodgerBlue);

            var optionPair = new BtnColorPair(optionsBtn, Color.Black,
            Color.Black, Color.White, Color.Purple);

            var aboutPair = new BtnColorPair(aboutBtn, Color.Black,
            Color.Black, Color.White, Color.Turquoise);

            btnControl = new BtnGroupController(
                acConfigPair,
                toPair,
                ldgPair,
                airportPair,
                optionPair,
                aboutPair);

            btnControl.Initialize();
            btnControl.SetSelected(acConfigBtn);
        }

        private void addControls()
        {
            AcMenu = new AircraftMenuControl();
            AcMenu.Location = new Point(12, 60);
            Controls.Add(AcMenu);

            ToMenu = new TOPerfControl();
            ToMenu.Location = new Point(12, 60);
            Controls.Add(ToMenu);

            LdgMenu = new LandingPerfControl();
            LdgMenu.Location = new Point(12, 60);
            Controls.Add(LdgMenu);

            AirportMenu = new AirportMapControl();
            AirportMenu.Location = new Point(12, 60);
            Controls.Add(AirportMenu);

            OptionsMenu = new OptionsControl();
            OptionsMenu.Location = new Point(12, 60);
            Controls.Add(OptionsMenu);

            AboutMenu = new AboutPageControl();
            AboutMenu.Location = new Point(12, 60);
            Controls.Add(AboutMenu);
        }

        private static void checkRegistry()
        {
            //try to check/add registry so that google map works properly 
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
                LoggerInstance.WriteToLog(ex);
            }
        }
    }
}
