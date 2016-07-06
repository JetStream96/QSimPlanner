using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.FuelCalculation.Tables;
using IniParser.Parser;
using IniParser;

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
            var data = new FileIniDataParser().ReadFile(path);

            var gta = data["GroundToAirDis"];
            var fuel = data["Fuel"];
            var time = data["Time"];
            var general = data["General"];

            return new FuelData(
                new FlightTimeTable(time.ToString()),
                new FuelTable(fuel.ToString()),
                new GroundToAirDisTable(gta.ToString()),
                double.Parse(general["HoldingFuelPerMinuteKg"]),
                double.Parse(general["MaxFuelKg"]),
                double.Parse(general["TaxiFuelPerMinKg"]),
                double.Parse(general["ApuFuelPerMinKg"]));
        }
    }
}
