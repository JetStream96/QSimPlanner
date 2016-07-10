using System.Collections.Generic;

namespace QSP.FuelCalculation
{
    public class AirDistanceCollection
    {
        public double AirDisToDestNm { get; private set; }
        public IEnumerable<double> AirDisToAltnNm { get; private set; }

        public AirDistanceCollection(
             double AirDisToDestNm,
             IEnumerable<double> AirDisToAltnNm)
        {
            this.AirDisToDestNm = AirDisToDestNm;
            this.AirDisToAltnNm = AirDisToAltnNm;
        }
    }
}
