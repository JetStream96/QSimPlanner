using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QSP.LandingPerfCalculation
{
    public static class LdgTableLoader
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
        public static IEnumerable<PerfTable> Load()
        {
            var tables = new Dictionary<string, PerfTable>();
            var files = Directory.GetFiles(CustomFolderPath).Concat(
                Directory.GetFiles(DefaultFolderPath));

            files.ForEach(f =>
            {
                var table = TryLoadTable(f);
                if (table == null) return;
                var key = table.Entry.ProfileName;
                if (tables.ContainsKey(key)) tables.Remove(key);
                tables.Add(key, table);
            });

            return tables.Select(kv => kv.Value);
        }

        /// <summary>
        /// Returns null if failed.
        /// </summary>
        public static PerfTable TryLoadTable(string path)
        {
            try
            {
                var doc = XDocument.Load(path);
                var elem = doc.Root.Element("FileLocation");
                if (elem != null)
                {
                    var m = elem.Attribute("multiplier");
                    var multiplier = m == null ? 1.0 : double.Parse(m.Value);
                    var t = TryLoadTable(Path.Combine(path, "..", elem.Value));
                    t.Item.Multiplier = multiplier;
                    t.Entry = GetEntry(path, doc);
                    return t;
                }
            }
            catch { }

            foreach (var attempt in LoadTableAttempts())
            {
                try
                {
                    return attempt(path);
                }
                catch { }
            }

            return null;
        }

        public static Func<string, PerfTable>[] LoadTableAttempts()
        {
            return new Func<string, PerfTable>[]
            {
                file => new Boeing.PerfDataLoader().ReadFromXml(file),
                file => Airbus.Loader.ReadFromXml(file)
            };
        }

        public static Entry GetEntry(string path, XDocument doc)
        {
            var elem = doc.Root.Element("Parameters");
            return new Entry(elem.Element("ProfileName").Value, path);
        }
    }
}
