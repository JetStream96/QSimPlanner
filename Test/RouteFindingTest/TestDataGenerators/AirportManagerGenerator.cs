using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Airports;

namespace Test.RouteFindingTest.TestDataGenerators
{
    public static class AirportManagerGenerator
    {
        public static AirportManager AirportList = Generate();

        public static AirportManager Generate()
        {
            var db = new AirportDatabase();
            db.Add(airport1());
            db.Add(airport2());

            return new AirportManager(db);
        }

        public static Airport airport1()
        {
            var rwys = new List<RwyData>();

            rwys.Add(new RwyData("18", "180", 3500, 60, false, "0.000", "0", 25.0003, 50.0001, 15, 3.00, 50, 1, 0));
            rwys.Add(new RwyData("36", "360", 3500, 60, false, "0.000", "0", 25.0001, 50.0005, 15, 3.00, 50, 1, 0));
            rwys.Add(new RwyData("03", "033", 2100, 50, false, "0.000", "0", 25.0004, 50.0004, 15, 3.0, 50, 1, 0));
            rwys.Add(new RwyData("21", "213", 2100, 50, false, "0.000", "0", 25.0000, 50.0001, 15, 3.0, 50, 1, 0));

            return new Airport("AXYZ", "Test Airport 01", 25.0, 50.0, 15, 5000, 8000, 3500, rwys);
        }

        public static Airport airport2()
        {
            var rwys = new List<RwyData>();

            rwys.Add(new RwyData("05", "055", 3100, 60, false, "0.000", "0", 45.0003, 70.0001, 15, 3.0, 50, 1, 0));
            rwys.Add(new RwyData("23", "235", 3100, 60, false, "0.000", "0", 45.0001, 70.0005, 15, 3.0, 50, 1, 0));

            return new Airport("BCGH", "Test Airport 02", 45.0, 70.0, 15, 5000, 8000, 3100, rwys);
        }

    }
}
