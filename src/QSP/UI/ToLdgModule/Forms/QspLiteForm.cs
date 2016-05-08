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
        private AircraftMenuControl acMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private AirportMapControl airportMenu;
        private OptionsControl optionsMenu;

        private BtnGroupController btnControl;
        private ControlSwitcher viewControl;

        public QspLiteForm()
        {
            InitializeComponent();
            addControls();
        }

        public void Initialize(ProfileManager manager)
        {
            acMenu.Initialize(manager);
            toMenu.InitializeAircrafts(
                manager.AcConfigs, manager.TOTables.ToList());

            ldgMenu.InitializeAircrafts(
                manager.AcConfigs, manager.LdgTables.ToList());

            airportMenu.InitializeControls();
            optionsMenu.Initialize();

            enableBtnColorControls();
            enableViewControl();
        }

        private void enableViewControl()
        {
            viewControl = new ControlSwitcher(
                new BtnControlPair(acConfigBtn, acMenu),
                new BtnControlPair(toBtn, toMenu),
                new BtnControlPair(ldgBtn, ldgMenu),
                new BtnControlPair(airportBtn, airportMenu),
                new BtnControlPair(optionsBtn, optionsMenu));

            viewControl.Subscribed = true;
        }

        private void enableBtnColorControls()
        {
            btnControl = new BtnGroupController(
                Color.Black,
                Color.WhiteSmoke,
                Color.White,
                new BtnColorPair(acConfigBtn, Color.FromArgb(192, 0, 0)),
                new BtnColorPair(toBtn, Color.DarkOrange),
                new BtnColorPair(ldgBtn, Color.ForestGreen),
                new BtnColorPair(airportBtn, Color.DodgerBlue),
                new BtnColorPair(optionsBtn, Color.Purple));

            btnControl.Initialize();
            btnControl.SetSelected(acConfigBtn);
        }

        private void addControls()
        {
            acMenu = new AircraftMenuControl();
            acMenu.Location = new Point(12, 60);
            Controls.Add(acMenu);

            toMenu = new TOPerfControl();
            toMenu.Location = new Point(12, 60);
            Controls.Add(toMenu);

            ldgMenu = new LandingPerfControl();
            ldgMenu.Location = new Point(12, 60);
            Controls.Add(ldgMenu);

            airportMenu = new AirportMapControl();
            airportMenu.Location = new Point(12, 60);
            Controls.Add(airportMenu);

            optionsMenu = new OptionsControl();
            optionsMenu.Location= new Point(12, 60);
            Controls.Add(optionsMenu);
        }
    }
}
