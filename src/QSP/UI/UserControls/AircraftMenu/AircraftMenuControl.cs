using System;
using System.Windows.Forms;
using QSP.AircraftProfiles;
using QSP.UI.Util;

namespace QSP.UI.UserControls.AircraftMenu
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

        public void Init(ProfileCollection profiles)
        {
            SetElements();
            InitController(profiles);
            registrationTxtBox.UpperCaseOnly();
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
                this,
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
                selectionGroupBox,
                propertyGroupBox,
                newBtn,
                editBtn,
                deleteBtn,
                new [] {wtUnitLbl1, wtUnitLbl2, wtUnitLbl3, wtUnitLbl4, wtUnitLbl5});
        }

        private void Subsribe()
        {
            newBtn.Click += controller.CreateConfig;
            saveBtn.Click += controller.SaveConfig;
            editBtn.Click += controller.EditConfig;
            deleteBtn.Click += controller.DeleteConfig;
            cancelBtn.Click += controller.CancelBtnClicked;
            acListView.SelectedIndexChanged += controller.ListViewSelectedChanged;
        }

        private void UnSubsribe()
        {
            newBtn.Click -= controller.CreateConfig;
            saveBtn.Click -= controller.SaveConfig;
            editBtn.Click -= controller.EditConfig;
            deleteBtn.Click -= controller.DeleteConfig;
            cancelBtn.Click -= controller.CancelBtnClicked;
            acListView.SelectedIndexChanged -= controller.ListViewSelectedChanged;
        }
    }
}
