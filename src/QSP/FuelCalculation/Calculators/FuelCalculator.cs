using QSP.LibraryExtension;
using System.Linq;

namespace QSP.FuelCalculation.Calculators
{
    public class FuelCalculator
    {
        private FuelDataItem fuelData;
        private FuelParameters para;
        private AirDistanceCollection airDis;

        public FuelCalculator(
            FuelDataItem fuelData, 
            FuelParameters para,
            AirDistanceCollection airDis)
        {
            this.fuelData = fuelData;
            this.para = para;
            this.airDis = airDis;
        }

        public CalculationResult Compute()
        {
            var altnResults = airDis.AirDisToAltnNm.Select(dis =>
            {
                var altnCalc = new AlternateFuelCalculator(fuelData, para);
                return altnCalc.Compute(dis);
            });

            var maxAltnFuelResult = altnResults.MaxBy(r => r.FuelTon);
            var destCalc = new DestinationFuelCalculator(
                fuelData, para, maxAltnFuelResult);
            return destCalc.Compute(airDis.AirDisToDestNm);
        }
    }
}
