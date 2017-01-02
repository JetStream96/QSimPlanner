using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.UI.Controllers.ControlGroup;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.UserControls;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.AboutPage;
using static QSP.UI.Controllers.ControlGroup.ControlSwitcher;
using static QSP.UI.Controllers.ControlGroup.GroupController;

namespace QSP.UI.Forms.NavigationBar
{
    public partial class NavBar : UserControl
    {
        private AircraftMenuControl acMenu;
        private FuelPlanningControl fuelMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private MiscInfoControl miscInfoMenu;
        private AboutPageControl aboutMenu;
        private Panel outerPanel;
        private Panel innerPanel;

        private GroupController btnControl;
        private ControlSwitcher viewControl;

        private IEnumerable<Control> AllPages => new Control[]
        {
            acMenu, fuelMenu, toMenu, ldgMenu, miscInfoMenu, aboutMenu
        };

        private Action HideAll => () =>
        {
            AllPages.ForEach(p => p.Hide());

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
            Panel outerPanel,
            Panel innerPanel)
        {
            this.acMenu = acMenu;
            this.fuelMenu = fuelMenu;
            this.toMenu = toMenu;
            this.ldgMenu = ldgMenu;
            this.miscInfoMenu = miscInfoMenu;
            this.aboutMenu = aboutMenu;
            this.outerPanel = outerPanel;
            this.innerPanel = innerPanel;

            EnableViewControl();
            EnableControlColors();
            SetOptionLblStyle();
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
                outerPanel,
                new ControlPair(acLbl, acMenu),
                new ControlPair(fuelLbl, fuelMenu),
                new ControlPair(tolbl, toMenu),
                new ControlPair(ldgLbl, ldgMenu),
                new ControlPair(miscLbl, miscInfoMenu),
                new ControlPair(helpLbl, aboutMenu));

            viewControl.Subscribed = true;
        }

        private Color[] ColorStyle => new[]
        {
            Color.Black, Color.White, Color.White, Color.FromArgb(0, 174, 219)
        };

        private void EnableControlColors()
        {
            var pairs = new[] { acLbl, fuelLbl, tolbl, ldgLbl, miscLbl, helpLbl }
                .Select(lbl => new ControlColorPair(lbl, ColorStyle));

            btnControl = new GroupController(pairs.ToArray());
            btnControl.Initialize();
            btnControl.SetSelected(acLbl);
        }

        private void SetOptionLblStyle()
        {
            new ColorController(OptionLbl, ColorStyle).Subscribed = true;
        }
    }
}
