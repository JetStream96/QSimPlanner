using QSP.TOPerfCalculation;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.Updates.NewFileManagement
{
    public class TOProfileManager
    {
        private NewFileManager manager;

        public TOProfileManager(Version backupVersion, Version newVersion)
        {
            manager = new NewFileManager(
                backupVersion,
                newVersion,
                TOTableLoader.DefaultFolderPath,
                "TOProfiles",
                SetProfileName,
                GetProfileName);
        }

        public void SetConfigs()
        {
            manager.SetConfigs();
        }

        // If the file format is wrong, returns null.
        private static string GetProfileName(string fileName)
        {
            try
            {
                return XDocument.Load(fileName).Root
                    .Element("Parameters").Element("ProfileName").Value;
            }
            catch
            {
                return null;
            }
        }

        private static void SetProfileName(string fileName, string profileName)
        {
            var doc = XDocument.Load(fileName);
            doc.Root.Element("Parameters").Element("ProfileName").Value =
                profileName;
            File.WriteAllText(fileName, doc.ToString());
        }
    }
}
