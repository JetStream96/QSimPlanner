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
        private string folderPath;

        // Do not change this. Updater depends on this path.
        public const string DefaultFolderPath = @"PerformanceData\Aircrafts";

        public ConfigLoader(string folderPath = DefaultFolderPath)
        {
            this.folderPath = folderPath;
        }

        /// <summary>
        /// Files in wrong format are ignored.
        /// Files containing the same registration are not loaded and
        /// a message will be included in returning value.
        /// </summary>
        public ConfigImportResult LoadAll()
        {
            var configs = new List<AircraftConfig>();

            foreach (var i in Directory.GetFiles(folderPath))
            {
                try
                {
                    configs.Add(new AircraftConfig(Load(i), i));
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex); 
                }
            }

            var groups = configs.GroupBy(c => c.Config.Registration);

            var result = groups
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .ToList();

            return new ConfigImportResult(result, Message(configs));
        }

        public static AircraftConfigItem Load(string filePath)
        {
            var doc = XDocument.Load(filePath);
            return new AircraftConfigItem.Serializer().Deserialize(doc.Root);
        }
        
        private static string Message(List<AircraftConfig> item)
        {
            var groups = item.GroupBy(x => x.Config.Registration);

            try
            {
                var duplicate = groups.First(g => g.Count() > 1);

                return
                    "The following aircrafts have" +
                    " identical registrations:\n\n" +
                    string.Join("\n", duplicate.Select(x => x.FilePath)) +
                    "\n\nNone of these profiles will be loaded.";
            }
            catch (InvalidOperationException)
            {
                // There is no duplicate.
                return null;
            }
        }

        public class ConfigImportResult
        {
            public List<AircraftConfig> Configs { get; private set; }
            public string Message { get; private set; }

            public ConfigImportResult(List<AircraftConfig> Configs,
                                      string Message)
            {
                this.Configs = Configs;
                this.Message = Message;
            }
        }
    }
}
