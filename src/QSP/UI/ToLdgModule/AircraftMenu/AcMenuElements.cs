using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuElements
    {
        public ListView AcListView { get; private set; }
        public ComboBox AcType { get; private set; }
        public ComboBox Registration { get; private set; }
        public ComboBox ToProfile { get; private set; }
        public ComboBox LdgProfile { get; private set; }
        public TextBox Zfw { get; private set; }
        public TextBox MaxToWt { get; private set; }
        public TextBox MaxLdgWt { get; private set; }
        public ComboBox ZfwUnit { get; private set; }
        public ComboBox MaxToWtUnit { get; private set; }
        public ComboBox MaxLdgWtUnit { get; private set; }

        public AcMenuElements(
             ListView AcListView,
             ComboBox AcType,
             ComboBox Registration,
             ComboBox ToProfile,
             ComboBox LdgProfile,
             TextBox Zfw,
             TextBox MaxToWt,
             TextBox MaxLdgWt,
             ComboBox ZfwUnit,
             ComboBox MaxToWtUnit,
             ComboBox MaxLdgWtUnit)
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
        }
    }
}
