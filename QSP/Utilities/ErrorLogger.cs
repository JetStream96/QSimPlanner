using System;
using System.IO;
using QSP.Core;

namespace QSP.Utilities
{
    public static class ErrorLogger
    {
        private static string filePath = QspCore.QspAppDataDirectory + "\\Log.txt";

        public static void WriteToLog(Exception ex)
        {
            WriteToLog(ex.ToString());
        }

        public static void WriteToLog(string msg)
        {
            try
            {
                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "    " + msg);
                }
            }
            finally { }

        }

    }
}
