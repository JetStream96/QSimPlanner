using QSP.AircraftProfiles.Configs;
using QSP.FuelCalculation.FuelData;
using System.Collections.Generic;
using TOTable = QSP.TOPerfCalculation.PerfTable;
using LdgTable = QSP.LandingPerfCalculation.PerfTable;

namespace QSP.AircraftProfiles
{
    // The collection of all aircraft profiles, and takeoff/landing performance
    // tables.
    //
    public class ProfileCollection
    {
        public AcConfigManager AcConfigs { get; private set; }
        public IEnumerable<FuelData> FuelData { get; private set; }
        public IEnumerable<TOTable> TOTables { get; private set; }
        public IEnumerable<LdgTable> LdgTables { get; private set; }

        /// <exception cref="PerfFileNotFoundException"></exception>
        public void Initialize()
        {
            FuelData = FuelDataLoader.Load();
            TOTables = new TOPerfCalculation.TOTableLoader().Load();
            LdgTables = new LandingPerfCalculation.LdgTableLoader().Load();
            AcConfigs = LoadConfig();

            AcConfigs.Validate(TOTables, LdgTables);
        }

        private AcConfigManager LoadConfig()
        {
            var manager = new AcConfigManager();
            var configs = new ConfigLoader().LoadAll();
            foreach (var i in configs) manager.Add(i);
            return manager;
        }
    }
}
