using LibraryExtension.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ServerCore
{
    // Thread-safe
    public class Logger
    {
        private SyncTaskQueue queue = new SyncTaskQueue();
        private string path;

        public Logger(IHostingEnvironment env, string path = "log.txt")
        {
            this.path =Util.MapPath(env,path);
        }

        // @NoThrow
        // Thread-safe.
        public void Log(string msg)
        {
            var text = Util.AddTimeStamp(msg) + "\n";
            Func<Task> t = () => Task.Factory.StartNew(() =>
            {
                try
                {
                    File.AppendAllText(path, text);
                }
                catch (Exception e)
                {
                    Shared.UnloggedError.Modify(s => s + text +
                        Util.AddTimeStamp(e.ToString()) + "\n");
                }
            });

          queue.Add(t);
        }
    }
}