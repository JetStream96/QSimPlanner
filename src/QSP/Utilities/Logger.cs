using System;
using System.IO;
using QSP.LibraryExtension;
using static QSP.Updates.Utilities;

namespace QSP.Utilities
{
    public interface ILogger
    {
        void Log(Exception ex);
        void Log(string msg);
    }

    public sealed class Logger : ILogger
    {
        private readonly string filePath;

        public Logger(string filePath = "Log.txt")
        {
            this.filePath = filePath;
        }

        public void Log(Exception ex)
        {
            Log(ex.ToString());
        }

        /// <summary>
        /// Logs the message, along with current app version and UTC time.
        /// </summary>
        public void Log(string msg)
        {
            var contents = "Version: " + TryGetVersion() + "\n" +
                DateTime.Now.ToStringWithUtc() + ":\n"
                + msg + "\n\n";

            ExceptionHelpers.IgnoreException(() =>
                File.AppendAllText(filePath, contents));
        }

        public static readonly Logger Instance = new Logger();
    }

    public sealed class EmptyLogger : ILogger
    {
        public static EmptyLogger Instance = new EmptyLogger();

        public void Log(string msg) { }

        public void Log(Exception ex) { }
    }
}
