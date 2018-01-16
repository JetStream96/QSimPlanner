using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary.LibraryExtension.Tasks
{
    public static class Util
    {
        // Wrap an async method returning a Task to 'async void'.
        public static async void NoAwait(Func<Task> a)
        {
            await a();
        }

        // Runs the action on the calling thread.
        public static async Task RunPeriodic(Action action, TimeSpan interval,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                action();
                await Task.Delay(interval, cancellationToken);
            }
        }

        /// <summary>
        /// Runs the action on another thread.
        /// </summary>
        public static async Task RunPeriodicAsync(Action action, TimeSpan interval,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                await Task.Factory.StartNew(action);
                await Task.Delay(interval, cancellationToken);
            }
        }

        /// <summary>
        /// Runs the given Func<T> with a timeout. Returns the result if the task
        /// finished before timeout.
        /// 
        /// Note that even if the Func<T> timed out, it is NOT cancelled.
        /// </summary>
        public static async Task<(bool timedout, T result)>
            RunAsyncWithTimeout<T>(Func<T> func, int timeoutMs)
        {
            var task = Task.Run(() => func());

            if (await Task.WhenAny(task, Task.Delay(timeoutMs)) == task)
            {
                // Task completed within timeout
                return (false, await task);
            }
            else
            {
                // Timeout
                return (true, default(T));
            }
        }
    }
}
