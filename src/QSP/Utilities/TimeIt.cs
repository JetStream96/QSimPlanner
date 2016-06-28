using System;
using System.Diagnostics;

namespace QSP.Utilities
{
    public static class TimeIt
    {
        public static double GetMilliseconds(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
