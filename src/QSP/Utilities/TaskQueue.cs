using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QSP.Utilities
{
    /// <summary>
    /// The tasks added to this queue are executed in the order of addition.
    /// </summary>
    public class TaskQueue
    {
        private Queue<CancellableTask> tasks = new Queue<CancellableTask>();
        private bool isRunning = false;

        /// <summary>
        /// Add a cancellable task to the queue. The action is run on worker thread. 
        /// If the task is successfully cancelled, the cleanupAction will be executed.
        /// </summary>
        public void Add(Task task, CancellationToken token, Action cleanupAction)
        {
            tasks.Enqueue(new CancellableTask(task, token, cleanupAction));
            if (!isRunning) Run();
        }

        private async Task Run()
        {
            isRunning = true;

            while (tasks.Count > 0)
            {
                var t = tasks.Dequeue();
                await t.Task;
            }

            isRunning = false;
        }

        public struct CancellableTask
        {
            public Task Task { get; }
            public CancellationToken Token { get; }
            public Action Cleanup { get; }

            public CancellableTask(Task Task, CancellationToken Token, Action Cleanup)
            {
                this.Task = Task;
                this.Token = Token;
                this.Cleanup = Cleanup;
            }
        }
    }
}