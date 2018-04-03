using QSP.AircraftProfiles.Configs;
using System.Collections.Generic;
using System.Linq;

namespace QSP.LandingPerfCalculation
{
    public static class FindTable
    {
        // TODO: Is this needed??
        /// <summary>
        /// Returns null is not found. 
        /// </summary>
        public static (AircraftConfig, PerfTable) Find(
            IReadOnlyList<PerfTable> tables, AcConfigManager aircrafts, string registration)
        {
            if (tables == null || tables.Count == 0) return (null, null);
            var config = aircrafts.Find(registration);
            var ac = config.Config;
            var profileName = ac.LdgProfile;
            return (config, tables.First(t => t.Entry.ProfileName == profileName));
        }
    }
}

