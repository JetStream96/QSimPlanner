using QSP.AircraftProfiles.Configs;
using System.Collections.Generic;

namespace QSP.AircraftProfiles
{
    // Manages all aircraft profiles, and takeoff/landing performance
    // tables.
    //
    public class ProfileManager
    {
        private List<string> _errors;

        public AcConfigManager AcConfigs { get; private set; }
        public IEnumerable<TOPerfCalculation.PerfTable> TOTables
        { get; private set; }

        public IEnumerable<LandingPerfCalculation.PerfTable> LdgTables
        { get; private set; }

        public IEnumerable<string> Errors
        {
            get
            {
                return _errors;
            }
        }
        
        /// <exception cref="PerfFileNotFoundException"></exception>
        public void Initialize()
        {
            _errors = new List<string>();

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

        private List<TOPerfCalculation.PerfTable> LoadTOTables()
        {
            var loadResult = new TOPerfCalculation.TOTableLoader().Load();

            if (loadResult.Message != null)
            {
                _errors.Add(loadResult.Message);
            }

            return loadResult.Tables;
        }

        private List<LandingPerfCalculation.PerfTable> LoadLdgTables()
        {
            var loadResult = new LandingPerfCalculation.LdgTableLoader()
                                 .Load();

            if (loadResult.Message != null)
            {
                _errors.Add(loadResult.Message);
            }

            return loadResult.Tables;
        }
    }
}
