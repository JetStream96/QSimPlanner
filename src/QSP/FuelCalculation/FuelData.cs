using QSP.FuelCalculation.Tables;
using System.Xml.Linq;

namespace QSP.FuelCalculation
{
    public class FuelData
    {
        public FlightTimeTable FlightTimeTable { get; private set; }
        public FuelTable FuelTable { get; private set; }
        public GroundToAirDisTable GtaTable { get; private set; }
        public double HoldingFuelPerMinuteKg { get; private set; }
        public double MaxFuelKg { get; private set; }
        public double TaxiFuelPerMinKg { get; private set; }
        public double ApuFuelPerMinKg { get; private set; }

        public FuelData(
            FlightTimeTable FlightTimeTable,
            FuelTable FuelTable,
            GroundToAirDisTable GtaTable,
            double HoldingFuelPerMinuteKg,
            double MaxFuelKg,
            double TaxiFuelPerMinKg,
            double ApuFuelPerMinKg)
        {
            this.FlightTimeTable = FlightTimeTable;
            this.FuelTable = FuelTable;
            this.GtaTable = GtaTable;
            this.HoldingFuelPerMinuteKg = HoldingFuelPerMinuteKg;
            this.MaxFuelKg = MaxFuelKg;
            this.TaxiFuelPerMinKg = TaxiFuelPerMinKg;
            this.ApuFuelPerMinKg = ApuFuelPerMinKg;
        }

        public static FuelData FromFile(string path)
        {
            var data = XDocument.Load(path);
            var root = data.Root;

            var gta = root.Element("GroundToAirDis");
            var fuel = root.Element("Fuel");
            var time = root.Element("Time");
            var general = root.Element("General");

            return new FuelData(
                new FlightTimeTable(time.Value),
                new FuelTable(fuel.Value),
                new GroundToAirDisTable(gta.Value),
                double.Parse(general.Element("HoldingFuelPerMinuteKg").Value),
                double.Parse(general.Element("MaxFuelKg").Value),
                double.Parse(general.Element("TaxiFuelPerMinKg").Value),
                double.Parse(general.Element("ApuFuelPerMinKg").Value));
        }
    }
}
