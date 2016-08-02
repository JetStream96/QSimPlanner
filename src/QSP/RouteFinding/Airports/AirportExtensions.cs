using System.Linq;

namespace QSP.RouteFinding.Airports
{
    public static class AirportExtensions
    {
        public static int RwyElevationFt(this Airport airport, string ident)
        {
            return airport.Rwys.First(r => r.RwyIdent == ident).Elevation;
        }        
    }
}
