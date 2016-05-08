using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Parser;
using System.IO;
using IniParser.Model;
using IniParser;

namespace QSP.UI.ToLdgModule.Options
{
    public static class OptionManager
    {
        private static string path = @"preference\options.cfg";

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
