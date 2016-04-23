using QSP.AircraftProfiles;
using QSP.UI.Controllers.ButtonGroup;
using QSP.UI.ToLdgModule.AircraftMenu;
using System.Drawing;
using System.Windows.Forms;
using static QSP.UI.Controllers.ButtonGroup.BtnGroupController;

namespace QSP.UI.ToLdgModule.Forms
{
    public partial class QspLiteForm : Form
    {
        private AircraftMenuControl acMenu;
        private BtnGroupController btnControl;

        public QspLiteForm()
        {
            InitializeComponent();
            addControls();
        }

        public void Initialize(ProfileManager manager)
        {
            acMenu.Initialize(manager);
            enableBtnColorControls();
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
        }
    }
}
