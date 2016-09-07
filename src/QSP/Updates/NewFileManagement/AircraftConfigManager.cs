using QSP.AircraftProfiles.Configs;
using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.FileNameGenerator;

namespace QSP.Updates.NewFileManagement
{
    public class AircraftConfigManager
    {       
        private Version backupVersion, newVersion;

        public AircraftConfigManager(Version backupVersion, Version newVersion)
        {
            this.backupVersion = backupVersion;
            this.newVersion = newVersion;
        }

        public void SetConfigs()
        {
            var oldFileFullPaths = Directory.GetFiles(Path.Combine("..",
                backupVersion.ToString(), ConfigLoader.DefaultFolderPath));

            var oldFileNames = oldFileFullPaths
                .Select(Path.GetFileName).ToHashSet();

            var newFileNames = GetFilesToPreserve().ToHashSet();
            var newFileFullPaths = newFileNames
                .Select(f => Path.Combine(ConfigLoader.DefaultFolderPath, f))
                .ToHashSet();

            var existingFiles = Directory.GetFiles(
                ConfigLoader.DefaultFolderPath);

            // Delete configs that were already present in old version.
            // This prevents some duplicates.
            DeleteNotNeededFiles(newFileNames, existingFiles);

            // If file names collides, rename the new ones.
            RenameNewFiles(oldFileNames, newFileFullPaths);

            // Copy the old config files.
            CopyOldConfigs(oldFileFullPaths);

            // Rename the registration of new configs, if there is collision.
            RenameRegistrations(oldFileFullPaths, newFileFullPaths);
        }

        private static void DeleteNotNeededFiles(HashSet<string> newFileNames,
            string[] existingFiles)
        {
            foreach (var i in existingFiles)
            {
                if (!newFileNames.Contains(Path.GetFileName(i)))
                {
                    File.Delete(i);
                }
            }
        }

        private static void RenameNewFiles(HashSet<string> oldFileNames,
            HashSet<string> newFileFullPaths)
        {
            var pathToRemove = new List<string>();
            var pathToAdd = new List<string>();

            foreach (var j in newFileFullPaths)
            {
                var nameWithoutDirectory = Path.GetFileName(j);

                if (oldFileNames.Contains(nameWithoutDirectory))
                {
                    var dir = Path.GetDirectoryName(j);
                    var name = Path.GetFileNameWithoutExtension(j);
                    var ext = Path.GetExtension(j);
                    var newName = Generate(dir, name, ext, n => $"({n})");
                    File.Move(j, newName);

                    pathToRemove.Add(j);
                    pathToAdd.Add(Path.GetFullPath(newName));
                }
            }

            foreach (var i in pathToRemove) newFileFullPaths.Remove(i);
            foreach (var i in pathToAdd) newFileFullPaths.Add(i);
        }

        private static void CopyOldConfigs(string[] oldFileFullPaths)
        {
            foreach (var k in oldFileFullPaths)
            {
                var newPath = Path.Combine(ConfigLoader.DefaultFolderPath,
                    Path.GetFileName(k));

                File.Copy(k, newPath);
            }
        }

        private static void RenameRegistrations(string[] oldFileFullPaths,
            HashSet<string> newFileFullPaths)
        {
            var existingReg = oldFileFullPaths
                            .Select(GetRegistration).ToHashSet();

            foreach (var m in newFileFullPaths)
            {
                var reg = GetRegistration(m);

                if (reg != null)
                {
                    var regRename = reg;
                    int num = 1;

                    while (existingReg.Contains(regRename))
                    {
                        regRename = reg + $"({num})";
                    }

                    SetRegistration(m, regRename);
                }
            }
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

        private IEnumerable<string> GetFilesToPreserve()
        {
            var elem = XDocument.Load("updater.xml")
                .Root
                .Element("NewFiles")
                .Element("AircraftConfig")
                .Elements("File");

            return elem.Where(e =>
            {
                var ver = Version.Parse(e.Attribute("Version").Value);
                return backupVersion < ver && ver <= newVersion;
            })
            .Select(e => e.Value);
        }
    }
}