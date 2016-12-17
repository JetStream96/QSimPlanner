using System;
using System.IO;

namespace QSP.Utilities
{
    public class Logger
    {
        private string filePath;

        public Logger(string filePath = "Log.txt")
        {
            this.filePath = filePath;
        }

        public void WriteToLog(Exception ex)
        {
            WriteToLog(ex.ToString());
        }

        public void WriteToLog(string msg)
        {
            try
            {
                File.AppendAllText(filePath, DateTime.Now.ToString() + ":\n" + msg + "\n\n");
            }
            catch
            { }
        }
    }
}
