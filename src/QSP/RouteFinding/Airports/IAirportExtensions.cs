using QSP.AviationTools;
using System.Linq;

namespace QSP.RouteFinding.Airports
{
    public static class IAirportExtensions
    {
        // Returns null if not found.
        public static IRwyData FindRwy(this IAirport airport, string ident)
        {
            return airport.Rwys.FirstOrDefault(r => r.RwyIdent == ident);
        }

        // TODO: Add test.
        // Returns null if the runway or the opposite runway cannot be found.
        public static double? GetSlopePercent(this IAirport a, string runway)
        {
            var rwy = a.FindRwy(runway);
            if (rwy == null) return null;

            var oppositeId = RwyIdentConversion.RwyIdentOppositeDir(runway);
            if (oppositeId == null) return null;

            var opposite = a.FindRwy(oppositeId);
            if (opposite == null) return null;

            return (opposite.ElevationFt - rwy.ElevationFt) * 100.0 / rwy.LengthFt;
        }
    }
}
