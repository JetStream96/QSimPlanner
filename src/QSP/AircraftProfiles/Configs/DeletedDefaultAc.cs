using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using QSP.LibraryExtension;
using QSP.Utilities;

namespace QSP.AircraftProfiles.Configs
{
    public class DeletedDefaultAc
    {
        public static string DeletedAcFileName =
            Path.Combine(ConfigLoader.DefaultFolderPath, "DeletedDefaultAc.xml");

        private readonly string filePath;

        public DeletedDefaultAc() : this(DeletedAcFileName) { }

        public DeletedDefaultAc(string filePath)
        {
            this.filePath = filePath;
        }
        
        // Returns null if failed to load or create file.
        public IEnumerable<string> DeletedRegistration()
        {
            var doc = LoadOrCreateFile();
            if (doc == null) return null;
            return doc.Root.Elements("Item").Select(i => i.Value);
        }

        // Returns whether the addition succeeds.
        public bool Add(string registration)
        {
            var old = DeletedRegistration();
            if (old == null) return false;
            var elem = old.Concat(registration).Select(reg => new XElement("Item", reg));
            var root = new XElement("Root", elem.ToArray());

            try
            {
                File.WriteAllText(filePath, new XDocument(root).ToString());
                return true;
            }
            catch (Exception e)
            {
                LoggerInstance.Log(e);
                return false;
            }
        }

        // Returns null if failed to load or create file.
        private XDocument LoadOrCreateFile()
        {
            if (File.Exists(filePath)) return XDocument.Load(filePath);

            try
            {
                var doc = CreateDoc();
                File.WriteAllText(filePath, doc.ToString());
                return doc;
            }
            catch (Exception e)
            {
                LoggerInstance.Log(e);
                return null;
            }
        }

        private static XDocument CreateDoc() => new XDocument(new XElement("Root"));

        public string ErrorMessage => $"Unable to access file {Path.GetFullPath(filePath)}";
    }
}