using QSP.AircraftProfiles.Configs;
using QSP.FuelCalculation.FuelData;
using System.Collections.Generic;
using System.Linq;
using TOTable = QSP.TOPerfCalculation.PerfTable;
using LdgTable = QSP.LandingPerfCalculation.PerfTable;

namespace QSP.AircraftProfiles
{
    // The collection of all aircraft profiles, and takeoff/landing performance tables.
    //
    public class ProfileCollection
    {
        public AcConfigManager AcConfigs { get; private set; }
        public IEnumerable<FuelData> FuelData { get; private set; }
        public IEnumerable<TOTable> TOTables { get; private set; }
        public IEnumerable<LdgTable> LdgTables { get; private set; }

        /// Returns error messages.
        public IEnumerable<string> Initialize()
        {
            FuelData = FuelDataLoader.Load();
            TOTables = new TOPerfCalculation.TOTableLoader().Load();
            LdgTables = new LandingPerfCalculation.LdgTableLoader().Load();
            var loadedAc = LoadConfig();
            AcConfigs = loadedAc.Result;

            var err = AcConfigs.Validate(FuelData, TOTables, LdgTables);
            return new[] { loadedAc.ErrorMessage, err }.Where(s => s != null);
        }

        private LoadResult LoadConfig()
        {
            var manager = new AcConfigManager();
            var configs = ConfigLoader.LoadAll();
            var err = configs.ErrorMessage;
            foreach (var i in configs.Result) manager.Add(i);
            return new LoadResult() { Result = manager, ErrorMessage = err };
        }

        private struct LoadResult
        {
            public AcConfigManager Result;
            public string ErrorMessage;
        }
    }
}
