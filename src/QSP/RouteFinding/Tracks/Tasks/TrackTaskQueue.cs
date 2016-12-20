using System;
using System.Threading;
using System.Threading.Tasks;
using QSP.Utilities;

namespace QSP.RouteFinding.Tracks.Tasks
{
    public class TrackTaskQueue
    {
        private TaskQueue queue = new TaskQueue();

        /// <summary>
        /// Add the task to the queue and cancel all other tasks previously added.
        /// </summary>
        public void Add(Task task, CancellationTokenSource tokenSource, Action cleanupAction)
        {
            queue.CancelAllTasks();
            queue.Add(task, tokenSource, cleanupAction);
        }
    }
}