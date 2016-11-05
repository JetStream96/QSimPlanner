using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Calculations
{
    public interface ICrzAltProvider
    {
        bool IsValidCrzAlt(ICoordinate c, double heading, double altitude);
        double ClosestAlt(ICoordinate c, double heading, double altitude);
        double ClosestAltBelow(ICoordinate c, double heading, double altitude);
    }
}