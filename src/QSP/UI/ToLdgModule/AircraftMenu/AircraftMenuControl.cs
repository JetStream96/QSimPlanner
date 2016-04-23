using QSP.AircraftProfiles;
using System.Linq;
using System.Windows.Forms;

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
            elements = new AcMenuElements(
                acListView,
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
                propertyGroupBox,
                newBtn,
                editBtn,
                deleteBtn);
        }

        private void subsribe()
        {
            newBtn.Click += controller.CreateConfig;
            saveBtn.Click += controller.SaveConfig;
            editBtn.Click += controller.EditConfig;
            deleteBtn.Click += controller.DeleteConfig;
            cancelBtn.Click += controller.CancelBtnClicked;
            acListView.SelectedIndexChanged +=
                controller.ListViewSelectedChanged;
        }

        private void unSubsribe()
        {
            newBtn.Click -= controller.CreateConfig;
            saveBtn.Click -= controller.SaveConfig;
            editBtn.Click -= controller.EditConfig;
            deleteBtn.Click -= controller.DeleteConfig;
            cancelBtn.Click -= controller.CancelBtnClicked;
            acListView.SelectedIndexChanged -=
                controller.ListViewSelectedChanged;
        }
    }
}
