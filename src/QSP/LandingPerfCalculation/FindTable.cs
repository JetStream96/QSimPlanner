using QSP.AircraftProfiles.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LandingPerfCalculation
{
    public static class FindTable
    {
        // Returns null is not found.
        public static (AircraftConfig, PerfTable) Find(
            IReadOnlyList<PerfTable> tables,
            AcConfigManager aircrafts,
            string registration)
        {
            if (tables != null && tables.Count > 0)
            {
                var config = aircrafts.Find(registration);
                var ac = config.Config;
                var profileName = ac.TOProfile;
                return (config, tables.First(t => t.Entry.ProfileName == profileName));
            }

            return (null, null);
        }
    }
}

