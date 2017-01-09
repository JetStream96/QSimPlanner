using System.Collections.Generic;
using System.Linq;
using System.Text;
using QSP.FuelCalculation.FuelData;
using QSP.LibraryExtension;
using TOTable = QSP.TOPerfCalculation.PerfTable;
using LdgTable = QSP.LandingPerfCalculation.PerfTable;

namespace QSP.AircraftProfiles.Configs
{
    public class AcConfigManager
    {
        private Dictionary<string, List<AircraftConfig>> aircrafts;
        private Dictionary<string, AircraftConfig> registrations;

        public AcConfigManager()
        {
            aircrafts = new Dictionary<string, List<AircraftConfig>>();
            registrations = new Dictionary<string, AircraftConfig>();
        }

        public IEnumerable<AircraftConfig> Aircrafts => registrations.Values;
        public int Count => registrations.Count;

        /// <exception cref="ArgumentException">The registration already exists.</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(AircraftConfig item)
        {
            registrations.Add(item.Config.Registration, item);
            AddToAircraftList(item);
        }

        private void AddToAircraftList(AircraftConfig item)
        {
            List<AircraftConfig> acSameType;

            if (aircrafts.TryGetValue(item.Config.AC, out acSameType))
            {
                acSameType.Add(item);
            }
            else
            {
                aircrafts.Add(item.Config.AC, new List<AircraftConfig>() { item });
            }
        }

        /// <summary>
        /// Returns whether the aircraft is found and removed.
        /// </summary>
        public bool Remove(string registration)
        {
            AircraftConfig config;

            if (registrations.TryGetValue(registration, out config))
            {
                registrations.Remove(registration);
                aircrafts[config.Config.AC].Remove(config);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            aircrafts = new Dictionary<string, List<AircraftConfig>>();
            registrations = new Dictionary<string, AircraftConfig>();
        }

        /// <summary>
        /// Check whether the takeoff and landing performance files exist
        /// for all aircraft configs. 
        /// </summary>
        /// <exception cref="PerfFileNotFoundException"></exception>
        public void Validate(IEnumerable<FuelData> fuelTables, IEnumerable<TOTable> takeoffTables,
            IEnumerable<LdgTable> ldgTables)
        {
            var errors = new List<string>();

            foreach (var i in registrations)
            {
                var config = i.Value;
                var item = config.Config;
                var fuel = item.FuelProfile;
                var to = item.TOProfile;
                var ldg = item.LdgProfile;

                bool fuelFound =
                    fuel == AircraftConfigItem.NoFuelTOLdgProfileText ||
                    fuelTables.Any(x => x.ProfileName == fuel);

                bool toFound =
                    to == AircraftConfigItem.NoFuelTOLdgProfileText ||
                    takeoffTables.Any(x => x.Entry.ProfileName == to);

                bool ldgFound =
                    ldg == AircraftConfigItem.NoFuelTOLdgProfileText ||
                    ldgTables.Any(x => x.Entry.ProfileName == ldg);

                var msg = GetError(item, fuelFound, toFound, ldgFound);
                if (msg != null) errors.Add(msg);
            }

            if (errors.Count > 0)
            {
                throw new PerfFileNotFoundException(string.Join("\n", errors));
            }
        }

        private static string GetError(AircraftConfigItem item,
            bool fuelFound, bool toFound, bool ldgFound)
        {
            var msgs = new List<string>();
            if (!fuelFound) msgs.Add("fuel");
            if (!toFound) msgs.Add("takeoff");
            if (!ldgFound) msgs.Add("landing");
            return (msgs.Count > 0) ? ErrorMessage(item, msgs) : null;
        }

        private static string ErrorMessage(AircraftConfigItem c, List<string> parts)
        {
            return $"Cannot find {parts.Combined()} profile(s) for {c.Registration} ({c.AC}).";
        }

        /// <summary>
        /// Returns null if not found.
        /// </summary>
        public AircraftConfig Find(string registration)
        {
            AircraftConfig config;
            return registrations.TryGetValue(registration, out config)
                ? config
                : null;
        }

        public IEnumerable<AircraftConfig> FindAircraft(string aircraft)
        {
            List<AircraftConfig> configs;
            return aircrafts.TryGetValue(aircraft, out configs)
                ? configs
                : new List<AircraftConfig>();
        }
    }
}
