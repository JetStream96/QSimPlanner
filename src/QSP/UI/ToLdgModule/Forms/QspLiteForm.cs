using QSP.AircraftProfiles;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.ToLdgModule.AirportMap;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using System.Drawing;
using System.Windows.Forms;
using static QSP.UI.Controllers.ButtonGroup.BtnGroupController;
using static QSP.UI.Controllers.ButtonGroup.ControlSwitcher;
using System.Linq;

namespace QSP.UI.ToLdgModule.Forms
{
    public partial class QspLiteForm : Form
    {
        private AircraftMenuControl acMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private AirportMapControl airportMenu;

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

            enableBtnColorControls();
            enableViewControl();
        }
        
        private void enableViewControl()
        {
            viewControl = new ControlSwitcher(
                new BtnControlPair(acConfigBtn, acMenu),
                new BtnControlPair(toBtn, toMenu),
                new BtnControlPair(ldgBtn, ldgMenu),
                new BtnControlPair(airportBtn, airportMenu));

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
                new BtnColorPair(airportBtn, Color.DodgerBlue));

            btnControl.Initialize();
            btnControl.SetSelected(acConfigBtn);
        }

        private void addControls()
        {
            var acMenu = new AircraftMenuControl();
            acMenu.Location = new Point(12, 60);
            Controls.Add(acMenu);
            this.acMenu = acMenu;

            var toMenu = new TOPerfControl();
            toMenu.Location = new Point(12, 60);
            Controls.Add(toMenu);
            this.toMenu = toMenu;

            var ldgMenu = new LandingPerfControl();
            ldgMenu.Location = new Point(12, 60);
            Controls.Add(ldgMenu);
            this.ldgMenu = ldgMenu;

            var airportMenu = new AirportMapControl();
            airportMenu.Location = new Point(12, 60);
            Controls.Add(airportMenu);
            this.airportMenu = airportMenu;
        }
    }
}
