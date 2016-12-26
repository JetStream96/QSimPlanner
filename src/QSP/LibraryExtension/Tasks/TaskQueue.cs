using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.Tasks
{
    /// <summary>
    /// The tasks added to this queue are executed in the order of addition.
    /// </summary>
    public class TaskQueue
    {
        private Queue<Func<Task>> tasks = new Queue<Func<Task>>();

        /// <summary>
        /// Returns ture if any task is running or pending. Otherwise false.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Add a task to the queue. The action is run on worker thread. 
        /// </summary>
        /// <param name="taskGetter">A method which starts and returns the task when called.</param>
        public void Add(Func<Task> taskGetter)
        {
            tasks.Enqueue(taskGetter);
            if (!IsRunning) Run();
        }

        private async Task Run()
        {
            IsRunning = true;
            while (tasks.Count > 0) await tasks.Dequeue()();
            IsRunning = false;
        }
    }
}