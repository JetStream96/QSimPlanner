using QSP.LibraryExtension.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace TrackBackupApp
{
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
                    Global.WriteToLog(e.ToString());
                }
            });

            queue.Add(t);
        }
    }
}