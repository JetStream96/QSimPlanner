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
                    (WeightUnit)elem.GetInt("WtUnit"));
            }

            public XElement Serialize(AircraftConfigItem item, string name)
            {
                var elem = new XElement[]
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
                    ((int)item.WtUnit).Serialize("WtUnit")
                };

                return new XElement(name, elem);
            }
        }
    }
}
