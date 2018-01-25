using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary.LibraryExtension.Tasks
{
    public static class Util
    {
        /// <summary>
        /// Wrap an async method returning a Task to 'async void'.
        /// </summary>
        public static async void NoAwait(Func<Task> a)
        {
            await a();
        }

        /// <summary>
        /// Periodically runs the action on the calling thread.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static async Task RunPeriodic(Action action, TimeSpan interval,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                var delay = Task.Delay(interval, cancellationToken);
                action();
                await delay;
            }
        }

        /// <summary>
        /// Periodically runs the action on another thread.
        /// </summary>
        /// /// <exception cref="Exception"></exception>
        public static async Task RunPeriodicAsync(Action action, TimeSpan interval,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                var delay = Task.Delay(interval, cancellationToken);
                await Task.Run(action);
                await delay;
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
