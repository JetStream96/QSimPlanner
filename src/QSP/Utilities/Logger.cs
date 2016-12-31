using System;
using System.IO;

namespace QSP.Utilities
{
    public interface ILogger
    {
        void Log(Exception ex);
        void Log(string msg);
    }

    public class Logger : ILogger
    {
        private string filePath;

        public Logger(string filePath = "Log.txt")
        {
            this.filePath = filePath;
        }

        public void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        public void Log(string msg)
        {
            ExceptionHelpers.IgnoreException(() =>
                File.AppendAllText(filePath, DateTime.Now.ToString() + ":\n" + msg + "\n\n"));
        }
    }
}
