using QSP.RouteFinding.FileExport.Providers;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QSP.Common.Options
{
    public static class OptionManager
    {
        public const string DefaultPath = @"Preference\options.xml";

        /// <summary>
        /// If the file does not exist, create one.
        /// Then read the file into an AppOptions instance.
        /// Return value is never null.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static AppOptions ReadOrCreateFile()
        {
            try
            {
                return ReadFromFile();
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
                SaveToFile(AppOptions.Default);
                return ReadFromFile();
            }
        }

        public static AppOptions ReadFromFile(string filePath = DefaultPath)
        {
            var doc = XDocument.Load(filePath);
            return new AppOptions.Serializer().Deserialize(doc.Root);
        }

        public static void SaveToFile(AppOptions settings, string filePath = DefaultPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var serializer = new AppOptions.Serializer();
            var elem = serializer.Serialize(settings, "AppOptions");
            var doc = new XDocument(elem);
            File.WriteAllText(filePath, doc.ToString());
        }

        /// <summary>
        /// Returns whether file was successfully saved.
        /// </summary>
        public static bool TrySaveFile(AppOptions settings, string filePath = DefaultPath)
        {
            try
            {
                SaveToFile(settings, filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Replace the invalid simulator paths in currentOption with automatically
        /// detected path if possible. Then try to save the file. If successful, 
        /// returns the new AppOption. Otherwise, returns the old one.
        /// </summary>
        public static AppOptions TryDetectSimulatorPathsAndSave(AppOptions currentOption)
        {
            var sims = currentOption.SimulatorPaths.ToDictionary(x => x.Key, x => x.Value);

            foreach (var sim in sims.Keys)
            {
                if (Directory.Exists(sims[sim])) continue;
                var path = Types.GetSimulatorPath(sim);
                sims[sim] = path;
            }

            throw new NotImplementedException();
            //TODO
        }
    }
}
