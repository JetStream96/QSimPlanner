using QSP.AircraftProfiles;
using QSP.AircraftProfiles.Configs;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AboutPage;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.AirportMap;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.Options;
using QSP.UI.ToLdgModule.TOPerf;
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

namespace QSP.UI.Forms
{
    public partial class QspForm : Form
    {
        public ProfileManager Profiles { get; private set; }
        public AircraftMenuControl AcMenu { get; private set; }
        public TOPerfControl ToMenu { get; private set; }
        public LandingPerfControl LdgMenu { get; private set; }
        public AirportMapControl AirportMenu { get; private set; }
        public OptionsControl OptionsMenu { get; private set; }
        public AboutPageControl AboutMenu { get; private set; }

        public IEnumerable<UserControl> Pages
        {
            get
            {
                return new UserControl[]
                {
                    AcMenu,
                    ToMenu,
                    LdgMenu,
                    AirportMenu,
                    OptionsMenu,
                    AboutMenu
                };
            }
        }

        private BtnGroupController btnControl;
        private ControlSwitcher viewControl;
        private AirportManager _airports;
        private Point controlDefaultLocation = new Point(12, 60);

        public QspForm()
        {
            InitializeComponent();
            AddControls();
        }

        private void InitProfiles()
        {
            try
            {
                Profiles = new ProfileManager();
                Profiles.Initialize();
            }
            catch (PerfFileNotFoundException ex)
            {
                LoggerInstance.WriteToLog(ex);
                MsgBoxHelper.ShowWarning(ex.Message);
            }
        }

        public void Init()
        {
            InitProfiles();
            ResizeForm();
            CheckRegistry();
            SubscribeEvents();
            OptionsMenu.Initialize();

            var airports = OptionsMenu.Airports;
            AcMenu.Initialize(Profiles);
            AcMenu.AircraftsChanged += ToMenu.RefreshAircrafts;
            AcMenu.AircraftsChanged += LdgMenu.RefreshAircrafts;

            ToMenu.Initialize(Profiles.AcConfigs,
                Profiles.TOTables.ToList(), airports);
            ToMenu.TryLoadState();

            LdgMenu.InitializeAircrafts(Profiles.AcConfigs,
                Profiles.LdgTables.ToList(), airports);
            LdgMenu.TryLoadState();

            AirportMenu.Initialize(airports);
            AirportMenu.BrowserEnabled = true;

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

        private void RefreshItemsRequireAirportList()
        {
            Airports = OptionsMenu.Airports;
            ToMenu.airportInfoControl.RefreshAirportInfo();
            LdgMenu.airportInfoControl.RefreshAirportInfo();
            AirportMenu.FindAirport();
        }

        private void SubscribeEvents()
        {
            OptionsMenu.SaveAirportsCompleted += (sender, e) =>
            {
                RefreshItemsRequireAirportList();
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

        private void EnableViewControl()
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

        private void EnableBtnColorControls()
        {
            var acConfigPair = new BtnColorPair(acConfigBtn, Color.Black,
                Color.WhiteSmoke, Color.White, Color.FromArgb(192, 0, 0));

            // fuel planning
            //Color.DarkOrange

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
            AcMenu = new AircraftMenuControl();
            ToMenu = new TOPerfControl();
            LdgMenu = new LandingPerfControl();
            AirportMenu = new AirportMapControl();
            OptionsMenu = new OptionsControl();
            AboutMenu = new AboutPageControl();

            foreach (var i in Pages)
            {
                i.Location = controlDefaultLocation;
                Controls.Add(i);
            }
        }

        private void ResizeForm()
        {
            int right = Pages.MaxBy(c => c.Width).Right;
            int bottom = Pages.MaxBy(c => c.Height).Bottom;            

            var newSize = new Size(right + 15, bottom + 15);
            MoveControls(newSize);
            ClientSize = newSize;
        }

        private void MoveControls(Size newSize)
        {
            int amount = ClientSize.Width - newSize.Width;
            MoveLeft(tableLayoutPanel2, amount);
            MoveLeft(tableLayoutPanel3, amount);
        }

        private static void MoveLeft(Control c, int amount)
        {
            var location = new Point(c.Location.X - amount, c.Location.Y);
            c.Location = location;
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
                LoggerInstance.WriteToLog(ex);
            }
        }
    }
}
