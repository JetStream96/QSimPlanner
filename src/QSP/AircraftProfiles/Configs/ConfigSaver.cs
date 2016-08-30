using IniParser;
using IniParser.Model;
using System.IO;
using static QSP.Utilities.Units.Conversions;

namespace QSP.AircraftProfiles.Configs
{
    public static class ConfigSaver
    {
        public static void Save(AircraftConfigItem config, string filePath)
        {
            var keys = new KeyDataCollection();
            keys.AddKey("AC", config.AC);
            keys.AddKey("Registration", config.Registration);
            keys.AddKey("FuelProfile", config.FuelProfile);
            keys.AddKey("TOProfile", config.TOProfile);
            keys.AddKey("LdgProfile", config.LdgProfile);
            keys.AddKey("OewKg", config.OewKg.ToString());
            keys.AddKey("MaxTOWtKg", config.MaxTOWtKg.ToString());
            keys.AddKey("MaxLdgWtKg", config.MaxLdgWtKg.ToString());
            keys.AddKey("MaxZfwKg", config.MaxZfwKg.ToString());
            keys.AddKey("WtUnit", WeightUnitToString(config.WtUnit));

            var secData = new SectionData("Data");
            secData.Keys = keys;

            var sections = new SectionDataCollection();
            sections.Add(secData);

            var iniData = new IniData(sections);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            new FileIniDataParser().WriteFile(filePath, iniData);
        }
    }
}
