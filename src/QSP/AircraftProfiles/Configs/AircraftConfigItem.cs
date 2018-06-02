using QSP.LibraryExtension.XmlSerialization;
using QSP.Utilities.Units;
using System.Xml.Linq;
using System.Xml.Serialization;
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

        public bool Equals(AircraftConfigItem other, double deltaWt, double deltaFuelBias)
        {
            return
                other.AC == AC &&
                other.Registration == Registration &&
                other.FuelProfile == FuelProfile &&
                other.TOProfile == TOProfile &&
                other.LdgProfile == LdgProfile &&
                Abs(other.OewKg - OewKg) <= deltaWt &&
                Abs(other.MaxTOWtKg - MaxTOWtKg) <= deltaWt &&
                Abs(other.MaxLdgWtKg - MaxLdgWtKg) <= deltaWt &&
                Abs(other.MaxZfwKg - MaxZfwKg) <= deltaWt &&
                Abs(other.MaxFuelKg - MaxFuelKg) <= deltaWt &&
                Abs(other.FuelBias - FuelBias) <= deltaFuelBias &&
                other.WtUnit == WtUnit;
        }

        public class Serialization
        {
            [XmlElement("AC")]
            public string AC { get; set; }

            [XmlElement("Registration")]
            public string Registration { get; set; }

            [XmlElement("FuelProfile")]
            public string FuelProfile { get; set; }

            [XmlElement("TOProfile")]
            public string TOProfile { get; set; }

            [XmlElement("LdgProfile")]
            public string LdgProfile { get; set; }

            [XmlElement("OewKg")]
            public double OewKg { get; set; }

            [XmlElement("MaxTOWtKg")]
            public double MaxTOWtKg { get; set; }

            [XmlElement("MaxLdgWtKg")]
            public double MaxLdgWtKg { get; set; }

            [XmlElement("MaxZfwKg")]
            public double MaxZfwKg { get; set; }

            [XmlElement("MaxFuelKg")]
            public double MaxFuelKg { get; set; }

            [XmlElement("FuelBias")]
            public double FuelBias { get; set; }

            [XmlElement("WtUnit")]
            public int WtUnit { get; set; }
        }

        public XElement Serialize()

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
