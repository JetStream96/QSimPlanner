using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QSP.AircraftProfiles.Configs
{
    public class ConfigLoader
    {
        public const string DefaultFolderPath = @"PerformanceData\Aircrafts";
        public const string CustomFolderPath = @"PerformanceData\Aircrafts\Custom";
        
        /// <summary>
        /// Files in wrong format are ignored.
        /// If two files have the same registration, the rules are:
        /// (1) The file in custom folder shadows file in default folder.
        /// (2) Only one of them is loaded.
        /// </summary>
        public IEnumerable<AircraftConfig> LoadAll()
        {
            var configs = new Dictionary<string, AircraftConfig>();
            var files = Directory.GetFiles(CustomFolderPath).Concat(
                Directory.GetFiles(DefaultFolderPath));

            foreach (var i in files)
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

        public static AircraftConfigItem Load(string filePath)
        {
            var doc = XDocument.Load(filePath);
            return new AircraftConfigItem.Serializer().Deserialize(doc.Root);
        }
    }
}
