using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.UI.Controllers.ControlGroup;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.UI.UserControls;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.AboutPage;
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
        private Form optionForm;

        private GroupController btnControl;
        private ControlSwitcher viewControl;

        private IEnumerable<Control> AllPages => new Control[]
        {
            acMenu, fuelMenu,toMenu,ldgMenu, aboutMenu
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
            Form optionForm)
        {
            this.acMenu = acMenu;
            this.toMenu = toMenu;
            this.ldgMenu = ldgMenu;
            this.optionForm = optionForm;
            this.miscInfoMenu = miscInfoMenu;
            this.aboutMenu = aboutMenu;

            EnableControlColors();
        }

        private void EnableControlColors()
        {
            var colors = new[]
            {
                Color.Black, Color.WhiteSmoke, Color.White, Color.FromArgb(0, 174, 219)
            };

            var pairs = new[] { acLbl, fuelLbl, tolbl, ldgLbl, miscLbl, helpLbl }
                .Select(lbl => new ControlColorPair(lbl, colors));

            btnControl = new GroupController(pairs.ToArray());
            btnControl.Initialize();
            btnControl.SetSelected(acLbl);
        }
    }
}
