using QSP.FuelCalculation;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.Updates.NewFileManagement
{
    public class FuelProfileManager
    {
        private NewFileManager manager;

        public FuelProfileManager(Version backupVersion, Version newVersion)
        {
            manager = new NewFileManager(
                backupVersion,
                newVersion,
                FuelDataLoader.DefaultFolderPath,
                "FuelProfiles",
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
                    .Element("General").Element("ProfileName").Value;
            }
            catch
            {
                return null;
            }
        }

        private static void SetProfileName(string fileName, string profileName)
        {
            var doc = XDocument.Load(fileName);
            doc.Root.Element("General").Element("ProfileName").Value =
                profileName;
            File.WriteAllText(fileName, doc.ToString());
        }
    }
}
