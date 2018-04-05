namespace QSP.FuelCalculation.FuelData
{
    public interface IFuelTable
    {
        double FuelRequired(double airDistance, double landingWt);
    }
}
