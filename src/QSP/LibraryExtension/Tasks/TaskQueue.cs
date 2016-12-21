using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.Tasks
{
    /// <summary>
    /// The tasks added to this queue are executed in the order of addition.
    /// </summary>
    public class TaskQueue
    {
        private Queue<CancellableTask> tasks = new Queue<CancellableTask>();
        private CancellableTask current;

        /// <summary>
        /// Returns ture if any task is running or pending. Otherwise false.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Add a cancellable task to the queue. The action is run on worker thread. 
        /// If the task is successfully cancelled, the cleanupAction will be executed.
        /// </summary>
        /// <param name="taskGetter">A method which starts and returns the task when called.</param>
        public void Add(Func<Task> taskGetter, CancellationTokenSource ts, Action cleanupAction)
        {
            tasks.Enqueue(new CancellableTask(taskGetter, ts, cleanupAction));
            if (!IsRunning) Run();
        }

        private async Task Run()
        {
            IsRunning = true;

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

            IsRunning = false;
        }

        /// <summary>
        /// Request to cancels the current task. If the task queue is empty, 
        /// this method does nothing.
        /// </summary>
        public void CancelCurrentTask()
        {
            current?.TokenSource.Cancel();
        }

        /// <summary>
        /// Cancels all tasks queued but not started yet. Request to cancel the task currently 
        /// running. If the task queue is empty and no task is running, this method does nothing.
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