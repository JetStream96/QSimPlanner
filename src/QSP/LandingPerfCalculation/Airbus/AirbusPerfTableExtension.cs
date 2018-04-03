using System.Linq;

namespace QSP.LandingPerfCalculation.Airbus
{
    public static class AirbusPerfTableExtension
    {
        /// <summary>
        /// All available flaps options, in the file order.
        /// </summary>
        public static string[] AllFlaps(this AirbusPerfTable t) =>
            t.Entries.Select(x => x.Flaps).Distinct().ToArray();

        /// <summary>
        /// All available brakes options, in the file order.
        /// </summary>
        public static string[] AllBrakes(this AirbusPerfTable t) =>
            t.Entries.Select(x => x.Autobrake).Distinct().ToArray();
    }
}
