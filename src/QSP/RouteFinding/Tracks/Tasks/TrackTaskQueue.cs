﻿using System;
using System.Threading;
using System.Threading.Tasks;
using QSP.Utilities;

namespace QSP.RouteFinding.Tracks.Tasks
{
    public class TrackTaskQueue
    {
        private TaskQueue queue = new TaskQueue();
    
        /// <summary>
        /// Returns ture if any task is running or pending. Otherwise false.
        /// </summary>
        public bool IsRunning => queue.IsRunning;

        /// <summary>
        /// Add the task to the queue and cancel all other tasks previously added.
        /// </summary>
        public void Add(Func<Task> taskGetter, CancellationTokenSource ts, Action cleanupAction)
        {
            queue.CancelAllTasks();
            queue.Add(taskGetter, ts, cleanupAction);
        }
    }
}