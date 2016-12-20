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
        private CancellableTask current;
        private bool isRunning = false;

        /// <summary>
        /// Add a cancellable task to the queue. The action is run on worker thread. 
        /// If the task is successfully cancelled, the cleanupAction will be executed.
        /// </summary>
        public void Add(Task task, CancellationTokenSource tokenSource, Action cleanupAction)
        {
            tasks.Enqueue(new CancellableTask(task, tokenSource, cleanupAction));
            if (!isRunning) Run();
        }

        private async Task Run()
        {
            isRunning = true;

            while (tasks.Count > 0)
            {
                current = tasks.Dequeue();

                try
                {
                    await current.Task;
                }
                catch (OperationCanceledException)
                {
                    current.Cleanup();
                }
            }

            isRunning = false;
        }

        public void CancelCurrentTask()
        {
            current.TokenSource.Cancel();
        }

        public struct CancellableTask
        {
            public Task Task { get; }
            public CancellationTokenSource TokenSource { get; }
            public Action Cleanup { get; }

            public CancellableTask(Task Task, CancellationTokenSource TokenSource, Action Cleanup)
            {
                this.Task = Task;
                this.TokenSource = TokenSource;
                this.Cleanup = Cleanup;
            }
        }
    }
}