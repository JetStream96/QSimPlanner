using QSP.Utilities.Units;
using static System.Math;

namespace QSP.AircraftProfiles.Configs
{
    public class AircraftConfigItem
    {
        public static readonly string NoFuelTOLdgProfileText = "None";

        public string AC { get; private set; }
        public string Registration { get; private set; }
        public string FuelProfile { get; private set; }
        public string TOProfile { get; private set; }
        public string LdgProfile { get; private set; }
        public double OewKg { get; private set; }
        public double MaxTOWtKg { get; private set; }
        public double MaxLdgWtKg { get; private set; }
        public double MaxZfwKg { get; private set; }
        public WeightUnit WtUnit { get; private set; }

        public AircraftConfigItem(
            string AC,
            string Registration,
            string FuelProfile,
            string TOProfile,
            string LdgProfile,
            double OewKg,
            double MaxTOWtKg,
            double MaxLdgWtKg,
            double MaxZfwKg,
            WeightUnit WtUnit)
        {
            this.AC = AC;
            this.Registration = Registration;
            this.FuelProfile = FuelProfile;
            this.TOProfile = TOProfile;
            this.LdgProfile = LdgProfile;
            this.OewKg = OewKg;
            this.MaxTOWtKg = MaxTOWtKg;
            this.MaxLdgWtKg = MaxLdgWtKg;
            this.WtUnit = WtUnit;
            this.MaxZfwKg = MaxZfwKg;
        }

        public bool Equals(AircraftConfigItem other, double delta)
        {
            return
                other.AC == AC &&
                other.Registration == Registration &&
                other.FuelProfile == FuelProfile &&
                other.TOProfile == TOProfile &&
                other.LdgProfile == LdgProfile &&
                Abs(other.OewKg - OewKg) <= delta &&
                Abs(other.MaxTOWtKg - MaxTOWtKg) <= delta &&
                Abs(other.MaxLdgWtKg - MaxLdgWtKg) <= delta &&
                Abs(other.MaxZfwKg - MaxZfwKg) <= delta &&
                other.WtUnit == WtUnit;
        }
    }
}
