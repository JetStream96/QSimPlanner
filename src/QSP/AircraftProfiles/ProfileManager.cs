using QSP.AircraftProfiles.Configs;
using QSP.FuelCalculation;
using System.Collections.Generic;
using TOTable = QSP.TOPerfCalculation.PerfTable;
using LdgTable = QSP.LandingPerfCalculation.PerfTable;

namespace QSP.AircraftProfiles
{
    // Manages all aircraft profiles, and takeoff/landing performance
    // tables.
    //
    public class ProfileManager
    {
        private List<string> _errors;

        public AcConfigManager AcConfigs { get; private set; }
        public IEnumerable<FuelData> FuelData { get; private set; }
        public IEnumerable<TOTable> TOTables { get; private set; }
        public IEnumerable<LdgTable> LdgTables { get; private set; }
        public IEnumerable<string> Errors { get { return _errors; } }

        /// <exception cref="PerfFileNotFoundException"></exception>
        public void Initialize()
        {
            _errors = new List<string>();

            FuelData = LoadFuelData();
            TOTables = LoadTOTables();
            LdgTables = LoadLdgTables();
            AcConfigs = LoadConfig();

            AcConfigs.Validate(TOTables, LdgTables);
        }

        private AcConfigManager LoadConfig()
        {
            var manager = new AcConfigManager();
            var configs = new ConfigLoader().LoadAll();

            if (configs.Message != null)
            {
                _errors.Add(configs.Message);
            }

            foreach (var i in configs.Configs)
            {
                manager.Add(i);
            }

            return manager;
        }

        private List<FuelData> LoadFuelData()
        {
            var loadResult = FuelDataLoader.Load();

            if (loadResult.Message != null)
            {
                _errors.Add(loadResult.Message);
            }

            return loadResult.Data;
        }

        private List<TOTable> LoadTOTables()
        {
            var loadResult = new TOPerfCalculation.TOTableLoader().Load();

            if (loadResult.Message != null)
            {
                _errors.Add(loadResult.Message);
            }

            return loadResult.Tables;
        }

        private List<LdgTable> LoadLdgTables()
        {
            var loadResult =
                new LandingPerfCalculation.LdgTableLoader().Load();

            if (loadResult.Message != null)
            {
                _errors.Add(loadResult.Message);
            }

            return loadResult.Tables;
        }
    }
}
