using System.Windows.Forms;

namespace QSP.UI.UserControls.AircraftMenu
{
    public class AcMenuElements
    {
        public Control ParentControl { get; }
        public ListView AcListView { get; }
        public ComboBox AcType { get; }
        public TextBox Registration { get; }
        public ComboBox WeightUnitCBox { get; }
        public ComboBox FuelProfile { get; }
        public ComboBox ToProfile { get; }
        public ComboBox LdgProfile { get; }
        public TextBox Oew { get; }
        public TextBox MaxToWt { get; }
        public TextBox MaxLdgWt { get; }
        public TextBox MaxZfw { get; }
        public TextBox MaxFuel { get; }
        public TextBox Bias { get; }
        public GroupBox SelectionBox { get; }
        public GroupBox PropertyBox { get; }
        public Button NewBtn { get; }
        public Button EditBtn { get; }
        public Button DeleteBtn { get; }

        public AcMenuElements(
            Control ParentControl,
            ListView AcListView,
            ComboBox AcType,
            TextBox Registration,
            ComboBox WeightUnitCBox,
            ComboBox FuelProfile,
            ComboBox ToProfile,
            ComboBox LdgProfile,
            TextBox Oew,
            TextBox MaxToWt,
            TextBox MaxLdgWt,
            TextBox MaxZfw,
            TextBox MaxFuel,
            TextBox Bias,
            GroupBox SelectionBox,
            GroupBox PropertyBox,
            Button NewBtn,
            Button EditBtn,
            Button DeleteBtn)
        {
            this.ParentControl = ParentControl;
            this.AcListView = AcListView;
            this.AcType = AcType;
            this.Registration = Registration;
            this.WeightUnitCBox = WeightUnitCBox;
            this.FuelProfile = FuelProfile;
            this.ToProfile = ToProfile;
            this.LdgProfile = LdgProfile;
            this.Oew = Oew;
            this.MaxToWt = MaxToWt;
            this.MaxLdgWt = MaxLdgWt;
            this.MaxZfw = MaxZfw;
            this.MaxFuel = MaxFuel;
            this.Bias = Bias;
            this.SelectionBox = SelectionBox;
            this.PropertyBox = PropertyBox;
            this.NewBtn = NewBtn;
            this.EditBtn = EditBtn;
            this.DeleteBtn = DeleteBtn;
        }
    }
}
