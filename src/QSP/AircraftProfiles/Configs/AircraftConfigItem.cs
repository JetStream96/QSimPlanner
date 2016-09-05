using QSP.Utilities.Units;
using System.Xml.Linq;
using static System.Math;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.AircraftProfiles.Configs
{
    // TODO: Implement interface IXSerializable and IXSerializer, to 
    // seperate the class and serialization methods.
    //
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
        public double MaxFuelKg { get; private set; }
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
            double MaxFuelKg,
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
            this.MaxFuelKg = MaxFuelKg;
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
                Abs(other.MaxFuelKg-MaxFuelKg) <= delta &&
                other.WtUnit == WtUnit;
        }

        public XElement Serialize(string name)
        {
            var elem = new XElement[]
            {
                AC.Serialize("AC"),
                Registration.Serialize("Registration"),
                FuelProfile.Serialize("FuelProfile"),
                TOProfile.Serialize("TOProfile"),
                LdgProfile.Serialize("LdgProfile"),
                OewKg.Serialize("OewKg"),
                MaxTOWtKg.Serialize("MaxTOWtKg"),
                MaxLdgWtKg.Serialize("MaxLdgWtKg"),
                MaxZfwKg.Serialize("MaxZfwKg"),
                MaxFuelKg.Serialize("MaxFuelKg"),
                ((int)WtUnit).Serialize("WtUnit")
            };

            return new XElement(name, elem);
        }

        public static AircraftConfigItem Deserialize(XElement item)
        {
            return new AircraftConfigItem(
                item.GetString("AC"),
                item.GetString("Registration"),
                item.GetString("FuelProfile"),
                item.GetString("TOProfile"),
                item.GetString("LdgProfile"),
                item.GetDouble("OewKg"),
                item.GetDouble("MaxTOWtKg"),
                item.GetDouble("MaxLdgWtKg"),
                item.GetDouble("MaxZfwKg"),
                item.GetDouble("MaxFuelKg"),
                (WeightUnit)item.GetInt("WtUnit"));
        }
    }
}
