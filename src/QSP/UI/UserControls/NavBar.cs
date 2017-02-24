using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.UI.Controllers.ControlGroup;
using QSP.UI.UserControls.AircraftMenu;
using QSP.UI.UserControls.TakeoffLanding.LandingPerf;
using QSP.UI.UserControls.TakeoffLanding.TOPerf;
using static QSP.UI.Controllers.ControlGroup.ControlSwitcher;
using static QSP.UI.Controllers.ControlGroup.GroupController;
using static QSP.UI.Utilities.OpenFileHelper;

namespace QSP.UI.UserControls
{
    public partial class NavBar : UserControl
    {
        private AircraftMenuControl acMenu;
        private FuelPlanningControl fuelMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private MiscInfoControl miscInfoMenu;
        private AboutPageControl aboutMenu;
        private Panel innerPanel;

        private GroupController btnControl;
        private ControlSwitcher viewControl;

        private IEnumerable<Control> AllPages => new Control[]
        {
            acMenu, fuelMenu, toMenu, ldgMenu, miscInfoMenu, aboutMenu
        };
        
        public NavBar()
        {
            InitializeComponent();
        }

        public void Init(AircraftMenuControl acMenu,
            FuelPlanningControl fuelMenu,
            TOPerfControl toMenu,
            LandingPerfControl ldgMenu,
            MiscInfoControl miscInfoMenu,
            AboutPageControl aboutMenu,
            Panel innerPanel)
        {
            this.acMenu = acMenu;
            this.fuelMenu = fuelMenu;
            this.toMenu = toMenu;
            this.ldgMenu = ldgMenu;
            this.miscInfoMenu = miscInfoMenu;
            this.aboutMenu = aboutMenu;
            this.innerPanel = innerPanel;

            EnableViewControl();
            EnableControlColors();
            SetExtraLblStyle();
            SetManualLblListener();
        }

        private void SetManualLblListener()
        {
            manualLbl.Click += (s, e) => TryOpenFile(Path.GetFullPath("manual/manual.html"), this);
        }

        private void SetControlPosition()
        {
            foreach (var i in AllPages)
            {
                i.Location = Point.Empty;
                i.Visible = i == acMenu;
                innerPanel.Controls.Add(i);
            }
        }

        private void EnableViewControl()
        {
            viewControl = new ControlSwitcher(
                new ControlPair(acLbl, acMenu),
                new ControlPair(fuelLbl, fuelMenu),
                new ControlPair(tolbl, toMenu),
                new ControlPair(ldgLbl, ldgMenu),
                new ControlPair(miscLbl, miscInfoMenu),
                new ControlPair(aboutLbl, aboutMenu));

            viewControl.Subscribed = true;
        }

        private ColorGroup ColorStyle => new ColorGroup(
            Color.Black, Color.White,
            Color.White, Color.DimGray,
            Color.White, Color.FromArgb(0, 174, 219));

        private void EnableControlColors()
        {
            var pairs = new[] { acLbl, fuelLbl, tolbl, ldgLbl, miscLbl, aboutLbl }
                .Select(lbl => new ControlColorPair(lbl, ColorStyle));

            btnControl = new GroupController(pairs.ToArray());
            btnControl.Initialize();
            btnControl.SetSelected(acLbl);
        }

        private void SetExtraLblStyle()
        {
            new[] { OptionLbl, manualLbl }.ForEach(
                i => new ColorController(i, ColorStyle).Subscribed = true);
        }
    }
}
