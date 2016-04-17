using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.AircraftMenu
{
    public class AcMenuElements
    {
        public ComboBox acType { get; private set; }
        public ComboBox registration { get; private set; }
        public ComboBox toProfile { get; private set; }
        public ComboBox ldgProfile { get; private set; }
        public TextBox zfw { get; private set; }
        public TextBox maxToWt { get; private set; }
        public TextBox maxLdgWt { get; private set; }
        public ComboBox zfwUnit { get; private set; }
        public ComboBox maxToWtUnit { get; private set; }
        public ComboBox maxLdgWtUnit { get; private set; }

        public AcMenuElements(
             ComboBox acType,
             ComboBox registration,
             ComboBox toProfile,
             ComboBox ldgProfile,
             TextBox zfw,
             TextBox maxToWt,
             TextBox maxLdgWt,
             ComboBox zfwUnit,
             ComboBox maxToWtUnit,
             ComboBox maxLdgWtUnit)
        {
            this.acType = acType;
            this.registration = registration;
            this.toProfile = toProfile;
            this.ldgProfile = ldgProfile;
            this.zfw = zfw;
            this.maxToWt = maxToWt;
            this.maxLdgWt = maxLdgWt;
            this.zfwUnit = zfwUnit;
            this.maxToWtUnit = maxToWtUnit;
            this.maxLdgWtUnit = maxLdgWtUnit;
        }
    }
}
