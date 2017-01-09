using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QSP.AircraftProfiles.Configs
{
    public static class ConfigLoader
    {
        public const string DefaultFolderPath = @"PerformanceData\Aircrafts\Default";
        public const string CustomFolderPath = @"PerformanceData\Aircrafts\Custom";

        /// <summary>
        /// Files in wrong format are ignored.
        /// If two files have the same registration, the rules are:
        /// (1) The file in custom folder shadows file in default folder.
        /// (2) Only one of them is loaded.
        /// </summary>
        public static IEnumerable<AircraftConfig> LoadAll()
        {
            var configs = new Dictionary<string, AircraftConfig>();

            foreach (var i in AllFiles)
            {
                try
                {
                    var config = new AircraftConfig(Load(i), i);
                    configs.Add(config.Config.Registration, config);
                }
                catch { }
            }

            return configs.Select(kv => kv.Value);
        }

        private static IEnumerable<string> AllFiles =>
            Directory.GetFiles(CustomFolderPath).Concat(Directory.GetFiles(DefaultFolderPath));

        public static AircraftConfigItem Load(string filePath)
        {
            var doc = XDocument.Load(filePath);
            return new AircraftConfigItem.Serializer().Deserialize(doc.Root);
        }

        /// <summary>
        /// Find any file in folders which contains a profile with the specified registration.
        /// Returns null if failed to find or load.
        /// </summary>
        public static AircraftConfig Find(string registration)
        {
            foreach (var i in AllFiles)
            {
                try
                {
                    var config = new AircraftConfig(Load(i), i);
                    if (config.Config.Registration == registration) return config;
                }
                catch { }
            }

            return null;
        }
    }
}
