using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.WindAloft;
using QSP.RouteFinding.Routes;
using QSP.FuelCalculation.Results;

namespace QSP.FuelCalculation.Calculations
{
    public class FuelCalculator
    {
        private IWindTableCollection windTable;
        private Route route;
        private FuelDataItem fuelData;

        public FuelCalculator(
            IWindTableCollection windTable,
            Route route,
            FuelDataItem fuelData)
        {
            if (route.Count < 2) throw new ArgumentException();
            this.windTable = windTable;
            this.route = route;
            this.fuelData = fuelData;
        }

        public DetailedPlan Calculate(
            double zfwKg,
            double landingFuelKg,
            double maxAltFt)
        {
            var waypoints = new List<DetailedPlan.Node>();

            var node = route.Last;
            //waypoints.Add(new DetailedPlan.Node(node, RouteFinding.Containers.AirwayType.))
            throw new NotImplementedException();
        }
    }
}
