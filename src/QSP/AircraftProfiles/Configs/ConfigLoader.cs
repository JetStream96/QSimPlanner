using IniParser.Parser;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.Utilities.Units.Conversions;

namespace QSP.AircraftProfiles.Configs
{
    // Load all aircraft config files. When there are duplicate registration 
    // numbers, only one of them will be loaded and the file in 
    // UserDefinedFolderPath shadows the ones in DefaultFolderPath.
    //
    public class ConfigLoader
    {
        private string defaultFolder, userDefinedFolder;

        public const string DefaultFolderPath = @"PerformanceData\Aircrafts";
        public const string UserDefinedFolderPath =
            @"PerformanceData\Aircrafts\UserDefined";

        public ConfigLoader(string defaultFolder = DefaultFolderPath,
            string userDefinedFolder = UserDefinedFolderPath)
        {
            this.defaultFolder = defaultFolder;
            this.userDefinedFolder = userDefinedFolder;
        }

        public ConfigImportResult LoadAll()
        {
            var result = new Dictionary<string, AircraftConfig>();

            foreach (var i in Directory.GetFiles(userDefinedFolder))
            {
                try
                {
                    var c = new AircraftConfig(Load(i), i);
                    var reg = c.Config.Registration;
                    if (!result.ContainsKey(reg)) result.Add(reg, c);
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex);
                }
            }

            foreach (var i in Directory.GetFiles(defaultFolder))
            {
                try
                {
                    var c = new AircraftConfig(Load(i), i);
                    var reg = c.Config.Registration;
                    if (!result.ContainsKey(reg)) result.Add(reg, c);
                }
                catch (Exception ex)
                {
                    LoggerInstance.WriteToLog(ex);
                }
            }

            return new ConfigImportResult(result.Values.ToList(), null);
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
                section["FuelProfile"],
                section["TOProfile"],
                section["LdgProfile"],
                double.Parse(section["OewKg"]),
                double.Parse(section["MaxTOWtKg"]),
                double.Parse(section["MaxLdgWtKg"]),
                double.Parse(section["MaxZfwKg"]),
                StringToWeightUnit(section["WtUnit"]));
        }

        public class ConfigImportResult
        {
            public List<AircraftConfig> Configs { get; private set; }

            // Null when no message is available.
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
