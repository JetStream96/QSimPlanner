using QSP.AircraftProfiles;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.AirportMap;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.ToLdgModule.Options;
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

        private BtnGroupController btnControl;
        private ControlSwitcher viewControl;

        public QspLiteForm()
        {
            InitializeComponent();
            addControls();
        }

        public void Initialize(ProfileManager manager)
        {
            OptionsMenu.Initialize();

            var airports = OptionsMenu.Airports;
            AcMenu.Initialize(manager);

            ToMenu.Initialize(manager.AcConfigs,
                manager.TOTables.ToList(), airports);

            LdgMenu.InitializeAircrafts(manager.AcConfigs,
                manager.LdgTables.ToList(), airports);

            AirportMenu.InitializeControls(airports);
            
            enableBtnColorControls();
            enableViewControl();
        }

        private void enableViewControl()
        {
            viewControl = new ControlSwitcher(
                new BtnControlPair(acConfigBtn, AcMenu),
                new BtnControlPair(toBtn, ToMenu),
                new BtnControlPair(ldgBtn, LdgMenu),
                new BtnControlPair(airportBtn, AirportMenu),
                new BtnControlPair(optionsBtn, OptionsMenu));

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

            btnControl = new BtnGroupController(
                acConfigPair,
                toPair,
                ldgPair,
                airportPair,
                optionPair);

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
        }
    }
}
