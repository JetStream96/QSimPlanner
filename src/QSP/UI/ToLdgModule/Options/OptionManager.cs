using IniParser;
using IniParser.Model;
using IniParser.Parser;
using QSP.NavData;
using System;
using System.IO;

namespace QSP.UI.ToLdgModule.Options
{
    public static class OptionManager
    {
        private static readonly string path = @"Preference\options.cfg";

        // If the file does not exist, create one.
        // Then read the file into an UserOption instance.
        // Return value is never null.
        public static UserOption ReadOrCreateFile()
        {
            try
            {
                return ReadFromFile();
            }
            catch (Exception)
            {
                Save(UserOption.Default);
                return ReadFromFile();
            }
        }

        // throws exceptions
        public static UserOption ReadFromFile()
        {
            var text = File.ReadAllText(path);
            var parser = new IniDataParser();
            var data = parser.Parse(text);
            var section = data["Options"];

            return new UserOption(
                (DataSource.Type)int.Parse(section["SourceType"]),
                section["OpenDataPath"],
                section["PaywarePath"]);
        }

        public static void Save(UserOption option)
        {
            var keys = new KeyDataCollection();
            keys.AddKey("SourceType", ((int)(option.SourceType)).ToString());
            keys.AddKey("OpenDataPath", option.OpenDataPath);
            keys.AddKey("PaywarePath", option.PaywarePath);

            var secData = new SectionData("Options");
            secData.Keys = keys;

            var sections = new SectionDataCollection();
            sections.Add(secData);

            var iniData = new IniData(sections);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            new FileIniDataParser().WriteFile(path, iniData);
        }
    }
}
