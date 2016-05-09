using IniParser.Parser;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.Core.EnumConversionTools;

namespace QSP.AircraftProfiles.Configs
{
    public class ConfigLoader
    {
        private string folderPath;

        public const string DefaultFolderPath = @"PerformanceData\Aircrafts";

        public ConfigLoader(string folderPath = DefaultFolderPath)
        {
            this.folderPath = folderPath;
        }

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
                    LoggerInstance.WriteToLog(ex); // TODO: 
                }
            }

            var groups = configs.GroupBy(c => c.Config.Registration);

            var result =
                groups
                .Where(g => g.Count() == 1)
                .Select(g => g.First())
                .ToList();

            return new ConfigImportResult(result, message(configs));
        }

        public static AircraftConfigItem Load(string filePath)
        {
            return Parse(File.ReadAllText(filePath));
        }

        public static AircraftConfigItem Parse(string text)
        {
            var parser = new IniDataParser();
            var data = parser.Parse(text);
            var section = data["Data"];

            return new AircraftConfigItem(
                section["AC"],
                section["Registration"],
                section["TOProfile"],
                section["LdgProfile"],
                double.Parse(section["ZfwKg"]),
                double.Parse(section["MaxTOWtKg"]),
                double.Parse(section["MaxLdgWtKg"]),
                StringToWeightUnit(section["WtUnit"]));
        }

        private static string message(List<AircraftConfig> item)
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
