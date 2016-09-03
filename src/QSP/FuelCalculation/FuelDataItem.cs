using QSP.FuelCalculation.Tables;
using QSP.Utilities.Units;
using System.Xml.Linq;

namespace QSP.FuelCalculation
{
    public class FuelDataItem
    {
        public FlightTimeTable FlightTimeTable { get; private set; }
        public FuelTable FuelTable { get; private set; }
        public OptCrzTable OptCrzTable { get; private set; }
        public SpeedProfile SpeedProfile { get; private set; }
        public double HoldingFuelPerMinuteKg { get; private set; }
        public double TaxiFuelPerMinKg { get; private set; }
        public double ApuFuelPerMinKg { get; private set; }
        public double MissedAppFuelKG { get; private set; }

        public FuelDataItem(
            FlightTimeTable FlightTimeTable,
            FuelTable FuelTable,
            OptCrzTable OptCrzTable,
            SpeedProfile SpeedProfile,
            double HoldingFuelPerMinuteKg,
            double TaxiFuelPerMinKg,
            double ApuFuelPerMinKg,
            double MissedAppFuelKG)
        {
            this.FlightTimeTable = FlightTimeTable;
            this.FuelTable = FuelTable;
            this.OptCrzTable = OptCrzTable;
            this.SpeedProfile = SpeedProfile;
            this.HoldingFuelPerMinuteKg = HoldingFuelPerMinuteKg;
            this.TaxiFuelPerMinKg = TaxiFuelPerMinKg;
            this.ApuFuelPerMinKg = ApuFuelPerMinKg;
            this.MissedAppFuelKG = MissedAppFuelKG;
        }

        public static FuelDataItem FromFile(string path)
        {
            var data = XDocument.Load(path);
            var root = data.Root;

            var gta = root.Element("GroundToAirDis");
            var fuel = root.Element("Fuel");
            var time = root.Element("Time");
            var general = root.Element("General");
            var cruize = root.Element("CruiseProfile");

            return new FuelDataItem(
                new FlightTimeTable(time.Value),
                new FuelTable(fuel.Value),
                GetOptAltTable(cruize),
                SpeedProfile.FromXml(cruize),
                double.Parse(general.Element("HoldingFuelPerMinuteKg").Value),
                double.Parse(general.Element("TaxiFuelPerMinKg").Value),
                double.Parse(general.Element("ApuFuelPerMinKg").Value),
                double.Parse(general.Element("MissedAppFuelKG").Value));
        }

        private static OptCrzTable GetOptAltTable(XElement CruiseProfileNode)
        {
            var optAltTable = CruiseProfileNode.Element("OptimumAlt");
            var unitTxt = optAltTable.Element("WeightUnit").Value;
            var unit = Conversions.StringToWeightUnit(unitTxt);

            return new OptCrzTable(
                optAltTable.Element("Table").Value, unit);
        }
    }
}
