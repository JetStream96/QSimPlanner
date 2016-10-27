using QSP.LibraryExtension.XmlSerialization;
using QSP.Utilities.Units;
using System.Xml.Linq;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;
using static System.Math;

namespace QSP.AircraftProfiles.Configs
{
    public class AircraftConfigItem
    {
        public static readonly string NoFuelTOLdgProfileText = "None";

        public string AC { get; }
        public string Registration { get; }
        public string FuelProfile { get; }
        public string TOProfile { get; }
        public string LdgProfile { get; }
        public double OewKg { get; }
        public double MaxTOWtKg { get; }
        public double MaxLdgWtKg { get; }
        public double MaxZfwKg { get; }
        public double MaxFuelKg { get; }
        public double FuelBias { get; }
        public WeightUnit WtUnit { get; }

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
            double FuelBias,
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
            this.FuelBias = FuelBias;
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
                Abs(other.MaxFuelKg - MaxFuelKg) <= delta &&
                Abs(other.FuelBias - FuelBias) <= delta &&
                other.WtUnit == WtUnit;
        }

        public class Serializer : IXSerializer<AircraftConfigItem>
        {
            public Serializer() { }

            public AircraftConfigItem Deserialize(XElement elem)
            {
                return new AircraftConfigItem(
                    elem.GetString("AC"),
                    elem.GetString("Registration"),
                    elem.GetString("FuelProfile"),
                    elem.GetString("TOProfile"),
                    elem.GetString("LdgProfile"),
                    elem.GetDouble("OewKg"),
                    elem.GetDouble("MaxTOWtKg"),
                    elem.GetDouble("MaxLdgWtKg"),
                    elem.GetDouble("MaxZfwKg"),
                    elem.GetDouble("MaxFuelKg"),
                    elem.GetDouble("FuelBias"),
                    (WeightUnit)elem.GetInt("WtUnit"));
            }

            public XElement Serialize(AircraftConfigItem item, string name)
            {
                var elem = new[]
                {
                    item.AC.Serialize("AC"),
                    item.Registration.Serialize("Registration"),
                    item.FuelProfile.Serialize("FuelProfile"),
                    item.TOProfile.Serialize("TOProfile"),
                    item.LdgProfile.Serialize("LdgProfile"),
                    item.OewKg.Serialize("OewKg"),
                    item.MaxTOWtKg.Serialize("MaxTOWtKg"),
                    item.MaxLdgWtKg.Serialize("MaxLdgWtKg"),
                    item.MaxZfwKg.Serialize("MaxZfwKg"),
                    item.MaxFuelKg.Serialize("MaxFuelKg"),
                    item.FuelBias.Serialize("FuelBias"),
                    ((int)item.WtUnit).Serialize("WtUnit")
                };

                return new XElement(name, elem);
            }
        }
    }
}
