using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.FileNameGenerator;

namespace QSP.Updates.NewFileManagement
{
    public class NewFileManager
    {
        // Directory in which the files are stored.
        private string directory;

        // Used to get files from updater.xml.
        private string id;

        // Arguments are: fileName, uniqueKey
        private Action<string, string> setUniqueKey;

        // Argument: fileName
        // Return value: uniqueKey, or null if file is not found or 
        // file format is incorrect
        private Func<string, string> getUniqueKey;

        private Version backupVersion, newVersion;

        public NewFileManager(
            Version backupVersion, 
            Version newVersion,
            string directory,
            string id,
            Action<string, string> uniqueKeySetter,
            Func<string, string> uniqueKeyGetter)
        {
            this.backupVersion = backupVersion;
            this.newVersion = newVersion;
            this.directory = directory;
            this.id = id;
            setUniqueKey = uniqueKeySetter;
            getUniqueKey = uniqueKeyGetter;
        }

        public void SetConfigs()
        {
            var oldFileFullPaths = Directory.GetFiles(Path.Combine("..",
                backupVersion.ToString(), directory));

            var oldFileNames = oldFileFullPaths
                .Select(Path.GetFileName).ToHashSet();

            var newFileNames = GetFilesToPreserve().ToHashSet();
            var newFileFullPaths = newFileNames
                .Select(f => Path.Combine(directory, f))
                .ToHashSet();

            var existingFiles = Directory.GetFiles(directory);

            // Delete configs that were already present in old version.
            // This prevents some duplicates.
            DeleteNotNeededFiles(newFileNames, existingFiles);

            // If file names collides, rename the new ones.
            RenameNewFiles(oldFileNames, newFileFullPaths);

            // Copy the old config files.
            CopyOldConfigs(oldFileFullPaths);

            // Rename the unique keys of new configs, if there is collision.
            RenameUniqueKeys(oldFileFullPaths, newFileFullPaths);
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
                }
            }
        }

        private void CopyOldConfigs(string[] oldFileFullPaths)
        {
            foreach (var k in oldFileFullPaths)
            {
                var newPath = Path.Combine(directory, Path.GetFileName(k));
                File.Copy(k, newPath);
            }
        }

        private void RenameUniqueKeys(string[] oldFileFullPaths,
            HashSet<string> newFileFullPaths)
        {
            var existingReg = oldFileFullPaths.Select(getUniqueKey)
                .ToHashSet();

            foreach (var m in newFileFullPaths)
            {
                var reg = getUniqueKey(m);

                if (reg != null)
                {
                    var regRename = reg;
                    int num = 1;

                    while (existingReg.Contains(regRename))
                    {
                        regRename = reg + $"({num})";
                    }

                    setUniqueKey(m, regRename);
                }
            }
        }
        
        private IEnumerable<string> GetFilesToPreserve()
        {
            var elem = XDocument.Load("updater.xml")
                .Root
                .Element("NewFiles")
                .Element(id)
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
