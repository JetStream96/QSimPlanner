namespace QSP.FuelCalculation.FuelData
{
    public static class DataPointExtension
    {
        /// <summary>
        /// Apply fuel bias to the given DataPoint. If bias is 1 then 
        /// return value will be the same as original.
        /// </summary>
        public static DataPoint WithBias(this DataPoint p, double bias)
        {
            return new DataPoint(
                p.Weight,
                p.CruiseFuelFlow * bias,
                p.CruiseKias,
                p.ClimbGradient,
                p.ClimbFuelFlow * bias,
                p.DescentGradient,
                p.DescentFuelFlow * bias,
                p.OptCruiseAlt,
                p.EtopsCruiseKtas,
                p.EtopsCruiseFuelFlow * bias);
        }
    }
}