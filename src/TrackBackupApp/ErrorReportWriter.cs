using QSP.LibraryExtension.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TrackBackupApp
{
    // This class is thread-safe.
    public class ErrorReportWriter
    {
        public static readonly int MaxBodaySize = 1_000_000;

        private TaskQueue queue = new TaskQueue();
        private string path;

        public ErrorReportWriter(string path = "~/error-report/error-report.txt")
        {
            this.path = path;
        }

        // @NoThrow
        // This method is thread-safe.
        public void Write(string ip, string text)
        {
            var str = "{ip:" + ip + ", time:" + DateTime.UtcNow.ToString() + ",text:" + text + "}";
            Func<Task> t = () => Task.Factory.StartNew(() =>
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.AppendAllText(path, str + "\n");
                }
                catch (Exception e)
                {
                    Shared.Logger.Log(e.ToString());
                }
            });

            queue.Add(t);
        }
    }
}