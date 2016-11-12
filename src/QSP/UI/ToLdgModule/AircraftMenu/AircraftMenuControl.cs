using QSP.AircraftProfiles;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public partial class AircraftMenuControl : UserControl
    {
        private AcMenuController controller;
        private AcMenuElements elements;

        public event EventHandler AircraftsChanged
        {
            add { controller.AircraftsChanged += value; }
            remove { controller.AircraftsChanged -= value; }
        }

        public AircraftMenuControl()
        {
            InitializeComponent();
        }

        public void Initialize(ProfileCollection profiles)
        {
            SetElements();
            InitController(profiles);
        }
        
        private void InitController(ProfileCollection profiles)
        {
            controller = new AcMenuController(elements, profiles);
            controller.InitializeControls();
            Subsribe();
        }

        private void SetElements()
        {
            elements = new AcMenuElements(
                acListView,
                acTypeComboBox,
                registrationTxtBox,
                wtUnitComboBox,
                fuelProfileComboBox,
                toProfileComboBox,
                ldgProfileComboBox,
                oewTxtBox,
                maxTOWtTxtBox,
                maxLdgWtTxtBox,
                maxZfwTxtBox,
                maxFuelTxtBox,
                biasPercentTxtBox,
                new Label[] { wtUnitLbl1, wtUnitLbl2, wtUnitLbl3, wtUnitLbl4 },
                selectionGroupBox,
                propertyGroupBox,
                newBtn,
                editBtn,
                deleteBtn);
        }

        private void Subsribe()
        {
            newBtn.Click += controller.CreateConfig;
            saveBtn.Click += controller.SaveConfig;
            editBtn.Click += controller.EditConfig;
            deleteBtn.Click += controller.DeleteConfig;
            cancelBtn.Click += controller.CancelBtnClicked;
            acListView.SelectedIndexChanged +=
                controller.ListViewSelectedChanged;
        }

        private void UnSubsribe()
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
