using CommonLibrary.LibraryExtension.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Server
{
    // Thread-safe
    public class Logger
    {
        private SyncTaskQueue queue = new SyncTaskQueue();
        private string path;

        public Logger(string path = "~/App_Data/log.txt")
        {
            this.path = HostingEnvironment.MapPath(path);
        }

        // @NoThrow
        // Thread-safe.
        public void Log(string msg)
        {
            var text = Global.AddTimeStamp(msg) + "\n";
            Func<Task> t = () => Task.Factory.StartNew(() =>
            {
                try
                {
                    File.AppendAllText(path, text);
                }
                catch (Exception e)
                {
                    Shared.UnloggedError.Modify(s => s + text +
                        Global.AddTimeStamp(e.ToString()) + "\n");
                }
            });

          queue.Add(t);
        }
    }
}