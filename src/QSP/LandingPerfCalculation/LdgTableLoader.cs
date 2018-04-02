using QSP.LandingPerfCalculation.Boeing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QSP.LandingPerfCalculation
{
    public class LdgTableLoader
    {
        public const string DefaultFolderPath = @"PerformanceData\LDG\Default";
        public const string CustomFolderPath = @"PerformanceData\LDG\Custom";

        /// <summary>
        /// Load all xml in the landing performance data folder.
        /// Files in wrong format are ignored.
        /// If two files have the same profile name, the rules are:
        /// (1) The file in custom folder shadows file in default folder.
        /// (2) Only one of them is loaded.
        /// </summary>
        public IEnumerable<PerfTable> Load()
        {
            var tables = new Dictionary<string, PerfTable>();
            var files = Directory.GetFiles(CustomFolderPath).Concat(
                Directory.GetFiles(DefaultFolderPath));

            foreach (var i in files)
            {
                try
                {
                    var table = new PerfDataLoader().ReadFromXml(i);
                    tables.Add(table.Entry.ProfileName, table);
                }
                catch { }
            }

            return tables.Select(kv => kv.Value);
        }

        public static Entry GetEntry(string path, XDocument doc)
        {
            var elem = doc.Root.Element("Parameters");
            return new Entry(elem.Element("ProfileName").Value, path);
        }
    }
}
