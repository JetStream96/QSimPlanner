using IniParser.Parser;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.Core.EnumConversionTools;

namespace QSP.AircraftProfiles
{
    public static class ConfigLoader
    {
        public static ConfigImportResult LoadAll(string folderPath)
        {
            var configs = new List<acConfigItem>();

            foreach (var i in Directory.GetFiles(folderPath))
            {
                try
                {
                    configs.Add(new acConfigItem(Load(i), i));
                }
                catch (Exception ex)
                {
                    ErrorLogger.WriteToLog(ex);
                }
            }

            var groups = configs.GroupBy(c => c.Config.Registration);
            var result = groups.Select(g => g.First().Config).ToList();

            return new ConfigImportResult(result, message(configs));
        }

        public static AircraftConfig Load(string filePath)
        {
            return Parse(File.ReadAllText(filePath));
        }

        public static AircraftConfig Parse(string text)
        {
            var parser = new IniDataParser();
            var data = parser.Parse(text);
            var section = data["Data"];

            return new AircraftConfig(
                section["AC"],
                section["Registration"],
                section["TOProfile"],
                section["LdgProfile"],
                double.Parse(section["ZfwKg"]),
                double.Parse(section["MaxTOWtKg"]),
                double.Parse(section["MaxLdgWtKg"]),
                StringToWeightUnit(section["WtUnit"]));
        }

        private static string message(List<acConfigItem> item)
        {
            var groups = item.GroupBy(x => x.Config.Registration);

            try
            {
                var duplicate = groups.First(g => g.Count() > 1);

                return
                    "The following aircrafts have" +
                    " identical registrations:\n" +
                    string.Join("\n", duplicate.Select(x => x.FilePath));
            }
            catch (InvalidOperationException)
            {
                // There is not duplicate.
                return null;
            }
        }

        private class acConfigItem
        {
            public AircraftConfig Config { get; private set; }
            public string FilePath { get; private set; }

            public acConfigItem(AircraftConfig Config, string FilePath)
            {
                this.Config = Config;
                this.FilePath = FilePath;
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
