using System.Drawing;
using System.Windows.Forms;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.AircraftProfiles;

namespace QSP.UI.ToLdgModule.Forms
{
    public partial class QspLiteForm : Form
    {
        private AircraftMenuControl acMenu;

        public QspLiteForm()
        {
            InitializeComponent();
            addControls();
        }

        public void Initialize(ProfileManager manager)
        {
            acMenu.Initialize(manager);
        }

        private void addControls()
        {
            var acMenu = new AircraftMenuControl();

            acMenu.Location = new Point(30, 30);

            Controls.Add(acMenu);
            this.acMenu = acMenu;
        }
    }
}
