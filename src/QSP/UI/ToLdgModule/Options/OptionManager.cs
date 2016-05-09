using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace QSP.UI.ToLdgModule.Options
{
    public static class OptionManager
    {
        private static string path = @"preference\options.cfg";

        // If the file does not exist, create one.
        // Then read the file into an UserOption instance.
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
                int.Parse(section["DataType"]),
                section["DataPath"]);
        }

        public static void Save(UserOption option)
        {
            var keys = new KeyDataCollection();
            keys.AddKey("DataType", option.SourceType.ToString());
            keys.AddKey("DataPath", option.SourcePath);

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
