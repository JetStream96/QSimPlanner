using CommonLibrary.LibraryExtension.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;

namespace ServerCore
{
    // Thread-safe.
    public class ErrorReportWriter
    {
        public static readonly int MaxBodaySize = 1_000_000;

        private SyncTaskQueue queue = new SyncTaskQueue();
        private SharedData shared;
        private string path;

        public ErrorReportWriter(IHostingEnvironment env, string path = "error-report/error-report.txt")
        {
            this.shared = SharedData.GetInstance(env);
            this.path = shared.MapPath(path);
        }

        // @NoThrow
        // Thread-safe.
        public void Write(string ip, string text)
        {
            var str = MakeJson(ip, text) + ",\n";

            Func<Task> t = () => Task.Factory.StartNew(() =>
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.AppendAllText(path, str);
                }
                catch (Exception e)
                {
                    shared.Logger.Log(e.ToString());
                }
            });

            queue.Add(t);
        }

        private static string MakeJson(string ip, string text)
        {
            var o = new JObject()
            {
                { "ip", ip },
                { "time", DateTime.UtcNow.ToString() },
                { "text", text }
            };

            return o.ToString();
        }
    }
}