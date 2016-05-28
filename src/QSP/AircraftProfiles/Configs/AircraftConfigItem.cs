using QSP.Utilities.Units;

namespace QSP.AircraftProfiles.Configs
{
    public class AircraftConfigItem
    {
        public string AC { get; private set; }
        public string Registration { get; private set; }
        public string TOProfile { get; private set; }
        public string LdgProfile { get; private set; }
        public double OewKg { get; private set; }
        public double MaxTOWtKg { get; private set; }
        public double MaxLdgWtKg { get; private set; }
        public WeightUnit WtUnit { get; private set; }

        public AircraftConfigItem(
            string AC,
            string Registration,
            string TOProfile,
            string LdgProfile,
            double OewKg,
            double MaxTOWtKg,
            double MaxLdgWtKg,
            WeightUnit WtUnit)
        {
            this.AC = AC;
            this.Registration = Registration;
            this.TOProfile = TOProfile;
            this.LdgProfile = LdgProfile;
            this.OewKg = OewKg;
            this.MaxTOWtKg = MaxTOWtKg;
            this.MaxLdgWtKg = MaxLdgWtKg;
            this.WtUnit = WtUnit;
        }

        public bool Equals(AircraftConfigItem other)
        {
            return
                other.AC == AC &&
                other.Registration == Registration &&
                other.TOProfile == TOProfile &&
                other.LdgProfile == LdgProfile &&
                other.OewKg == OewKg &&
                other.MaxTOWtKg == MaxTOWtKg &&
                other.MaxLdgWtKg == MaxLdgWtKg &&
                other.WtUnit == WtUnit;
        }
    }
}
