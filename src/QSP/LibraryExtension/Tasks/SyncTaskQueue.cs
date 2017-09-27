using System;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.Tasks
{
    // Execute the given tasks without blocking.
    public class SyncTaskQueue
    {
        private object _lock = new object();

        public void Add(Func<Task> taskGetter)
        {
            Task.Run(() =>
            {
                lock (_lock)
                {
                    taskGetter();
                }
            });
        }        
    }
}