using QSP.AviationTools;
using System.Linq;
using CommonLibrary.LibraryExtension;

namespace QSP.RouteFinding.Airports
{
    public static class IAirportExtensions
    {
        public static bool Equals(IAirport x, IAirport y)
        {
            return x != null &&
                y != null &&
                x.Icao == y.Icao &&
                x.Name == y.Name &&
                x.Lat == y.Lat &&
                x.Lon == y.Lon &&
                x.Elevation == y.Elevation &&
                x.TransAvail == y.TransAvail &&
                x.TransAlt == y.TransAlt &&
                x.TransLvl == y.TransLvl &&
                x.LongestRwyLengthFt == y.LongestRwyLengthFt &&
                x.Rwys.SetEquals(y.Rwys);
        }

        // Returns null if not found.
        public static IRwyData FindRwy(this IAirport airport, string ident)
        {
            return airport.Rwys.FirstOrDefault(r => r.RwyIdent == ident);
        }

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
