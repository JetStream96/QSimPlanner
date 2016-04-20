using System.Windows.Forms;
using QSP.UI.ToLdgModule.AircraftMenu;
using QSP.AircraftProfiles;
using System.Linq;

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
            showErrors(profiles);
        }

        private void showErrors(ProfileManager profiles)
        {
            var errors = profiles.Errors;

            if (errors.Count() > 0)
            {
                MessageBox.Show(string.Join("\n\n\n", errors));
            }
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
                maxLdgWtUnitComboBox,
                selectionGroupBox,
                propertyGroupBox);
        }

        private void subsribe()
        {
            acListView.SelectedIndexChanged += controller.SelectedAcChanged;
            newBtn.Click += controller.CreateConfig;
            acTypeComboBox.TextChanged += controller.AcTypeChanged;
            registrationTxtBox.TextChanged += controller.RegistrationChanged;
        }

        private void unSubsribe()
        {
            acListView.SelectedIndexChanged -= controller.SelectedAcChanged;
            newBtn.Click -= controller.CreateConfig;
            acTypeComboBox.TextChanged -= controller.AcTypeChanged;
            registrationTxtBox.TextChanged -= controller.RegistrationChanged;
        }
    }
}
