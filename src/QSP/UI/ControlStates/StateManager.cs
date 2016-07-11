using QSP.Utilities;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.UI.ControlStates
{
    public static class StateManager
    {
        private const string directory = "SavedStates";

        public static void Save(string fileName, XElement elem)
        {
            try
            {
                var filePath = Path.Combine(directory, fileName);
                Directory.CreateDirectory(directory);

                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.Write(elem);
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
            }
        }

        public static XDocument Load(string fileName)
        {
            try
            {
                return XDocument.Load(Path.Combine(directory, fileName));
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);
                return null;
            }
        }
    }
}
