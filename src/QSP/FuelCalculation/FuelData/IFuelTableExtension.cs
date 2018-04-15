namespace QSP.FuelCalculation.FuelData
{
    public static class IFuelTableExtension
    {
        /// <summary>
        /// Returns null if f is null.
        /// </summary>
        public static IFuelTable WithBias(this IFuelTable f, double bias)
        {
            return f == null ? null : new Helper(f, bias);
        }

        private class Helper : IFuelTable
        {
            private IFuelTable t;
            private double bias;

            public Helper(IFuelTable t, double bias)
            {
                this.t = t;
                this.bias = bias;
            }

            public double FuelRequired(double airDistance, double landingWt)
            {
                return t.FuelRequired(airDistance, landingWt) * bias;
            }
        }
    }
}
