using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuElements
    {
        public ListView AcListView { get; private set; }
        public ComboBox AcType { get; private set; }
        public TextBox Registration { get; private set; }
        public ComboBox ToProfile { get; private set; }
        public ComboBox LdgProfile { get; private set; }
        public TextBox Zfw { get; private set; }
        public TextBox MaxToWt { get; private set; }
        public TextBox MaxLdgWt { get; private set; }
        public ComboBox ZfwUnit { get; private set; }
        public ComboBox MaxToWtUnit { get; private set; }
        public ComboBox MaxLdgWtUnit { get; private set; }
        public GroupBox SelectionBox { get; private set; }
        public GroupBox PropertyBox { get; private set; }
        public Button NewBtn { get; private set; }
        public Button EditBtn { get; private set; }
        public Button DeleteBtn { get; private set; }

        public AcMenuElements(
             ListView AcListView,
             ComboBox AcType,
             TextBox Registration,
             ComboBox ToProfile,
             ComboBox LdgProfile,
             TextBox Zfw,
             TextBox MaxToWt,
             TextBox MaxLdgWt,
             ComboBox ZfwUnit,
             ComboBox MaxToWtUnit,
             ComboBox MaxLdgWtUnit,
             GroupBox SelectionBox,
             GroupBox PropertyBox,
             Button NewBtn,
             Button EditBtn,
             Button DeleteBtn)
        {
            this.AcListView = AcListView;
            this.AcType = AcType;
            this.Registration = Registration;
            this.ToProfile = ToProfile;
            this.LdgProfile = LdgProfile;
            this.Zfw = Zfw;
            this.MaxToWt = MaxToWt;
            this.MaxLdgWt = MaxLdgWt;
            this.ZfwUnit = ZfwUnit;
            this.MaxToWtUnit = MaxToWtUnit;
            this.MaxLdgWtUnit = MaxLdgWtUnit;
            this.SelectionBox = SelectionBox;
            this.PropertyBox = PropertyBox;
            this.NewBtn = NewBtn;
            this.EditBtn = EditBtn;
            this.DeleteBtn = DeleteBtn;
        }
    }
}
