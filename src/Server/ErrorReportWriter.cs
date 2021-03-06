﻿using QSP.LibraryExtension.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;

namespace Server
{
    // Thread-safe.
    public class ErrorReportWriter
    {
        public static readonly int MaxBodaySize = 1_000_000;

        private SyncTaskQueue queue = new SyncTaskQueue();
        private string path;

        public ErrorReportWriter(string path = "~/App_Data/error-report/error-report.txt")
        {
            this.path = HostingEnvironment.MapPath(path);
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
                    Shared.Logger.Log(e.ToString());
                }
            });

            queue.Add(t);
        }

        private static string MakeJson(string ip, string text)
        {
            var o = new JObject()
            {
                { "ip", ip},
                { "time", DateTime.UtcNow.ToString()},
                { "text", text}
            };

            return o.ToString();
        }
    }
}