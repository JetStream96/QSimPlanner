using System.Windows.Forms;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.AircraftProfiles;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public partial class AircraftMenuControl : UserControl
    {
        private AcMenuController controller;
        private AcMenuElements elements;
        
        public AircraftMenuControl()
        {
            InitializeComponent();
        }

        public void Initialize(ProfileManager profiles)
        {
            setElements();
            initController(profiles);
        }

        private void initController(ProfileManager profiles)
        {
            controller = new AcMenuController(elements, profiles);
            controller.InitializeControls();
            subsribe();
        }

        private void setElements()
        {
            elements = new AcMenuElements(acListView,
                acTypeComboBox,
                registrationTxtBox,
                toProfileComboBox,
                ldgProfileComboBox,
                zfwTxtBox,
                maxTOWtTxtBox,
                maxLdgWtTxtBox,
                zfwUnitComboBox,
                maxTOWtUnitComboBox,
                maxLdgWtUnitComboBox);
        }

        private void subsribe()
        {
            acListView.SelectedIndexChanged += controller.SelectedAcChanged;
        }

        private void unSubsribe()
        {
            acListView.SelectedIndexChanged -= controller.SelectedAcChanged;
        }
    }
}
