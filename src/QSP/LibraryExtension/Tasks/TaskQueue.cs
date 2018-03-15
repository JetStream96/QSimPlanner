using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.Tasks
{
    // Two things to be proved:
    // 1. No two tasks are executed at the same time:
    // If a task is executing, IsRunning is necessarily ture. Therefore when a new task 
    // is added, it won't be executed.
    // 
    // 2. If the queue will be eventually empty:
    // When a new task is added, since the code other than 'await' runs on a single thread,
    // the possibilities are:
    // (1) Run() already returned. In this case the new task will clearly be executed.
    // (2) The code inside Run() is still executing, i.e. awaiting a task.
    //     The new task will be run because of the while loop.
    //
    /// <summary>
    /// The tasks added to this queue are executed in the order of addition.
    /// This class is not thread-safe.
    /// </summary>
    public class TaskQueue
    {
        private Queue<Func<Task>> tasks = new Queue<Func<Task>>();

        /// <summary>
        /// Returns ture if any task is running or pending.
        /// If accessed on another thread, this can be true even if no task is running or pending.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Add a task to the queue. The action is run on worker thread. 
        /// </summary>
        /// <param name="taskGetter">A method which starts and returns the task when called.</param>
        public void Add(Func<Task> taskGetter)
        {
            tasks.Enqueue(taskGetter);
            if (!IsRunning) Util.NoAwait(()=> Run());
        }

        private async Task Run()
        {
            IsRunning = true;
            while (tasks.Count > 0) await tasks.Dequeue()();
            IsRunning = false;
        }
    }
}