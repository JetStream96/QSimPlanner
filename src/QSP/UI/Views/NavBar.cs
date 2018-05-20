using QSP.LibraryExtension;
using QSP.UI.Controllers.ControlGroup;
using QSP.UI.UserControls;
using QSP.UI.UserControls.AircraftMenu;
using QSP.UI.UserControls.TakeoffLanding.LandingPerf;
using QSP.UI.UserControls.TakeoffLanding.TOPerf;
using QSP.UI.Views.FuelPlan;
using QSP.UI.Views.MiscInfo;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;
using static QSP.UI.Controllers.ControlGroup.ControlSwitcher;
using static QSP.UI.Controllers.ControlGroup.GroupController;
using static QSP.UI.Util.OpenFileHelper;

namespace QSP.UI.Views
{
    public partial class NavBar : UserControl
    {
        private AircraftMenuControl acMenu;
        private FuelPlanningControl fuelMenu;
        private TOPerfControl toMenu;
        private LandingPerfControl ldgMenu;
        private MiscInfoControl miscInfoMenu;
        private TracksControl tracksMenu;
        private WindControl windControl;
        private AboutPageControl aboutMenu;
        private Panel innerPanel;

        private GroupController btnControl;
        private ControlSwitcher viewControl;

        private IEnumerable<Control> AllPages => Arr<Control>
        (
            acMenu, fuelMenu, toMenu, ldgMenu, miscInfoMenu, tracksMenu, windControl, aboutMenu
        );

        public NavBar()
        {
            InitializeComponent();
        }

        public void Init(AircraftMenuControl acMenu,
            FuelPlanningControl fuelMenu,
            TOPerfControl toMenu,
            LandingPerfControl ldgMenu,
            MiscInfoControl miscInfoMenu,
            TracksControl tracksMenu,
            WindControl windControl,
            AboutPageControl aboutMenu,
            Panel innerPanel)
        {
            this.acMenu = acMenu;
            this.fuelMenu = fuelMenu;
            this.toMenu = toMenu;
            this.ldgMenu = ldgMenu;
            this.miscInfoMenu = miscInfoMenu;
            this.tracksMenu = tracksMenu;
            this.windControl = windControl;
            this.aboutMenu = aboutMenu;
            this.innerPanel = innerPanel;

            EnableViewControl();
            EnableControlColors();
            SetExtraLblStyle();
            SetManualLblListener();
        }

        public void ShowTracks()
        {
            viewControl.ShowControl(tracksLbl);
            btnControl.SetSelected(tracksLbl);
        }

        public void ShowWind()
        {
            viewControl.ShowControl(windLbl);
            btnControl.SetSelected(windLbl);
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
                new ControlPair(tracksLbl, tracksMenu),
                new ControlPair(windLbl, windControl),
                new ControlPair(aboutLbl, aboutMenu));

            viewControl.Subscribed = true;
        }

        private ColorGroup ColorStyle => new ColorGroup(
            Color.Black, Color.White,
            Color.White, Color.DimGray,
            Color.White, Color.FromArgb(0, 174, 219));

        private void EnableControlColors()
        {
            var pairs = Arr(acLbl, fuelLbl, tolbl, ldgLbl, miscLbl, tracksLbl, windLbl, aboutLbl)
                .Select(lbl => new ControlColorPair(lbl, ColorStyle));

            btnControl = new GroupController(pairs.ToArray());
            btnControl.Initialize();
            btnControl.SetSelected(acLbl);
        }

        private void SetExtraLblStyle()
        {
            Arr(OptionLbl, manualLbl).ForEach(i =>
                new ColorController(i, ColorStyle).Subscribed = true);
        }
    }
}
