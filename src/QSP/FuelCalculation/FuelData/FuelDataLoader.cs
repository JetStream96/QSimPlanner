using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSP.FuelCalculation.FuelData
{
    public static class FuelDataLoader
    {
        public const string DefaultFolderPath = @"PerformanceData\FuelCalc\Default";
        public const string CustomFolderPath = @"PerformanceData\FuelCalc\Custom";

        /// <summary>
        /// Load all xml in the landing performance data folder.
        /// Files in wrong format are ignored.
        /// If two files have the same profile name, the rules are:
        /// (1) The file in custom folder shadows file in default folder.
        /// (2) Only one of them is loaded.
        /// </summary>
        public static IEnumerable<FuelData> Load()
        {
            var tables = new Dictionary<string, FuelData>();
            var files = Directory.GetFiles(CustomFolderPath).Concat(
                Directory.GetFiles(DefaultFolderPath));

            foreach (var i in files)
            {
                try
                {
                    var data = FuelData.FromFile(i);
                    var key = data.ProfileName;
                    if (tables.ContainsKey(key)) tables.Remove(key);
                    tables.Add(key, data);
                }
                catch { }
            }

            return tables.Select(kv => kv.Value);
        }
    }
}
