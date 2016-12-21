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
        public void Add(Func<Task> taskGetter, CancellationTokenSource ts, Action cleanupAction)
        {
            tasks.Enqueue(new CancellableTask(taskGetter, ts, cleanupAction));
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
                    await current.TaskGetter();
                }
                catch (OperationCanceledException)
                {
                    current.Cleanup();
                }
            }

            isRunning = false;
        }

        /// <summary>
        /// Cancels the current task. If the task queue is empty, this method does nothing.
        /// </summary>
        public void CancelCurrentTask()
        {
            current?.TokenSource.Cancel();
        }

        /// <summary>
        /// Cancels all tasks including the current one. 
        /// If the task queue is empty, this method does nothing.
        /// </summary>
        public void CancelAllTasks()
        {
            tasks.Clear();
            CancelCurrentTask();
        }

        public class CancellableTask
        {
            public Func<Task> TaskGetter { get; }
            public CancellationTokenSource TokenSource { get; }
            public Action Cleanup { get; }

            public CancellableTask(Func<Task> TaskGetter, 
                CancellationTokenSource ts, Action Cleanup)
            {
                this.TaskGetter = TaskGetter;
                this.TokenSource = ts;
                this.Cleanup = Cleanup;
            }
        }
    }
}