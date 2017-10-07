using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryExtension.Tasks;
using QSP.RouteFinding.Tracks.Common;
using static QSP.RouteFinding.Tracks.Common.Helpers;

namespace QSP.RouteFinding.Tracks.Actions
{
    public class TrackTaskQueues
    {
        private TaskQueue[] queues = new TaskQueue[TrackTypes.Count];

        public TrackTaskQueues()
        {
            for (int i = 0; i < queues.Length; i++)
            {
                queues[i] = new TaskQueue();
            }
        }

        public void EnqueueSyncTask(TrackType type, Action action, ActionSequence seq)
        {
            Func<Task> task = () =>
            {
                seq.Before();
                action();
                seq.After();
                return Task.FromResult(0);
            };

            EnqueueTask(type, task);
        }

        public void EnqueueSyncTask(TrackType type, Action action)
        {
            Func<Task> task = () =>
            {
                action();
                return Task.FromResult(0);
            };

            EnqueueTask(type, task);
        }

        public void EnqueueTask(TrackType type, Func<Task> taskGetter, ActionSequence seq)
        {
            EnqueueSyncTask(type, seq.Before);
            queues[(int)type].Add(taskGetter);
            EnqueueSyncTask(type, seq.After);
        }

        public void EnqueueTask(TrackType type, Func<Task> taskGetter)
        {
            queues[(int)type].Add(taskGetter);
        }

        public async Task WaitForTasks()
        {
            while (queues.Any(q => q.IsRunning))
            {
                await Task.Delay(250);
            }
        }
    }
}