using QSP.Utilities;
using System;
using System.IO;
using System.Xml.Linq;

namespace QSP.UI.ControlStates
{
    public static class StateManager
    {
        public const string Directory = "SavedStates";

        public static void Save(string fileName, XElement elem)
        {
            try
            {
                var filePath = Path.Combine(Directory, fileName);
                System.IO.Directory.CreateDirectory(Directory);

                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.Write(elem);
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
            }
        }

        public static XDocument Load(string fileName)
        {
            try
            {
                return XDocument.Load(Path.Combine(Directory, fileName));
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
                return null;
            }
        }
    }
}
