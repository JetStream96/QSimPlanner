using QSP.AircraftProfiles.Configs;
using System.Collections.Generic;

namespace QSP.AircraftProfiles
{
    // Manages all aircraft profiles, and takeoff/landing performance
    // tables.
    //
    public class ProfileManager
    {
        public AcConfigManager AcConfigs { get; set; }
        public IEnumerable<TOPerfCalculation.PerfTable> TOTables
        { get; set; }

        public IEnumerable<LandingPerfCalculation.PerfTable> LdgTables
        { get; set; }        
    }
}
