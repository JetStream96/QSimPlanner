using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.Tasks
{
   public static class Util
    {
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

        // Runs the action on another thread.
        public static async Task RunPeriodicAsync(Action action, TimeSpan interval, 
            CancellationToken cancellationToken)
        {
            while (true)
            {
                await Task.Factory.StartNew(action);
                await Task.Delay(interval, cancellationToken);
            }
        }
    }
}
