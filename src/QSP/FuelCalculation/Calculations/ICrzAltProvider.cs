using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Calculations
{
    public interface ICrzAltProvider
    {
        double ClosestAltitudeFtTo(ICoordinate current,
            ICoordinate next, double altitude);

        double ClosestAltitudeFtFrom(ICoordinate previous,
            ICoordinate current, double altitude);
    }
}