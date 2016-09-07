using QSP.AircraftProfiles.Configs;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.Updates.NewFileManagement
{
    public class AircraftConfigManager
    {
        private NewFileManager manager;

        public AircraftConfigManager(Version backupVersion, Version newVersion)
        {
            manager = new NewFileManager(
                backupVersion,
                newVersion,
                ConfigLoader.DefaultFolderPath,
                "AircraftConfig",
                SetRegistration,
                GetRegistration);
        }

        public void SetConfigs()
        {
            manager.SetConfigs();
        }

        // If the file format is wrong, returns null.
        public static string GetRegistration(string fileName)
        {
            try
            {
                return XDocument.Load(fileName)
                    .Root.Element("Registration").Value;
            }
            catch
            {
                return null;
            }
        }

        private static void SetRegistration(
            string fileName, string registration)
        {
            var doc = XDocument.Load(fileName);
            doc.Root.Element("Registration").Value = registration;
            File.WriteAllText(fileName, doc.ToString());
        }
    }
}