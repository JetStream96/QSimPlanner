using QSP.LibraryExtension.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TrackBackupApp
{
    // Thread-safe
    public class Logger
    {
        private TaskQueue queue = new TaskQueue();
        private string path;

        public Logger(string path = "~/log.txt")
        {
            this.path = path;
        }

        // @NoThrow
        // This method is thread-safe.
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