using QSP.Utilities;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.Common.Options
{
    public static class OptionManager
    {
        public const string DefaultPath = @"Preference\options.xml";

        // If the file does not exist, create one.
        // Then read the file into an AppOptions instance.
        // Return value is never null.
        public static AppOptions ReadOrCreateFile()
        {
            try
            {
                return ReadFromFile();
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
                SaveToFile(AppOptions.Default);
                return ReadFromFile();
            }
        }

        public static AppOptions ReadFromFile(string filePath = DefaultPath)
        {
            var doc = XDocument.Load(filePath);
            return new AppOptions(doc);
        }

        public static void SaveToFile(
            AppOptions settings, string filePath = DefaultPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, settings.ToXml().ToString());
        }

        /// <summary>
        /// Returns whether file was successfully saved.
        /// </summary>
        public static bool TrySaveFile(
            AppOptions settings, string filePath = DefaultPath)
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
    }
}
