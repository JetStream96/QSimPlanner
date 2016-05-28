using System.IO;
using System.Xml.Linq;

namespace QSP.Core.Options
{
    public static class OptionManager
    {
        public const string DefaultPath = @"Preference\options.xml";

        public static AppOptions ReadFromFile(string filePath = DefaultPath)
        {
            if (File.Exists(filePath) == false)
            {
                var settings = new AppOptions();
                SaveToFile(settings, filePath);
                return settings;
            }
            else
            {
                var doc = XDocument.Load(filePath);
                return new AppOptions(doc);
            }
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
