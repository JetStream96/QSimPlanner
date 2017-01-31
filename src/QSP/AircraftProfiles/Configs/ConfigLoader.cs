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
        /// (3) If a file is in default folder and the registration is in DeletedDefaultAc file,
        ///     the file is not loaded.
        /// </summary>
        public static LoadResult LoadAll()
        {
            var deleted = new DeletedDefaultAc();
            var reg = deleted.DeletedRegistration();
            if (reg == null)
            {
                return new LoadResult()
                {
                    Result = LoadAll(new string[0]),
                    ErrorMessage = deleted.ErrorMessage
                };
            }

            return new LoadResult() { Result = LoadAll(reg), ErrorMessage = null };
        }

        public struct LoadResult
        {
            public IEnumerable<AircraftConfig> Result;
            public string ErrorMessage;
        }

        private static IEnumerable<AircraftConfig> LoadAll(IEnumerable<string> deletedRegistration)
        {
            var configs = new Dictionary<string, AircraftConfig>();

            foreach (var i in Directory.GetFiles(CustomFolderPath))
            {
                var loaded = Load(i);
                if (loaded == null) continue;
                var reg = loaded.Registration;
                if (!configs.ContainsKey(reg))
                {
                    configs.Add(reg, new AircraftConfig(loaded, i));
                }
            }

            foreach (var i in Directory.GetFiles(DefaultFolderPath))
            {
                var loaded = Load(i);
                if (loaded == null) continue;
                var reg = loaded.Registration;
                if (!configs.ContainsKey(reg) && !deletedRegistration.Contains(reg))
                {
                    configs.Add(reg, new AircraftConfig(loaded, i));
                }
            }

            return configs.Select(kv => kv.Value);
        }

        private static IEnumerable<string> AllFiles =>
            Directory.GetFiles(CustomFolderPath).Concat(Directory.GetFiles(DefaultFolderPath));

        // Returns null if failed.
        public static AircraftConfigItem Load(string filePath)
        {
            try
            {
                var doc = XDocument.Load(filePath);
                return new AircraftConfigItem.Serializer().Deserialize(doc.Root);
            }
            catch
            {
                return null;
            }
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
