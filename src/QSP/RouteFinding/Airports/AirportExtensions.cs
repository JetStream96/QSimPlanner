using System.Linq;

namespace QSP.RouteFinding.Airports
{
    public static class AirportExtensions
    {
        // Returns null if not found.
        public static RwyData FindRwy(this Airport airport, string ident)
        {
            return airport.Rwys.FirstOrDefault(r => r.RwyIdent == ident);
        }        
    }
}
