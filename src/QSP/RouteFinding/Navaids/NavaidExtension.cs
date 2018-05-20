using QSP.LibraryExtension;
using QSP.RouteFinding.Data.Interfaces;
using System.Linq;
using static System.Math;

namespace QSP.RouteFinding.Navaids
{
    public static class NavaidExtension
    {
        /// <summary>
        /// Returns null if not found.
        /// </summary>
        public static Navaid Find(this MultiMap<string, Navaid> navaids,
            string id, ICoordinate c, double delta = 1e-3)
        {
            var matches = navaids.FindAll(id);
            return matches.FirstOrDefault(x =>
                Abs(c.Lat - x.Lat) <= delta && Abs(c.Lon - x.Lon) <= delta);
        }
    }
}
