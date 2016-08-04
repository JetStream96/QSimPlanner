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

        public static void PrintTime(Action action)
        {
            var time = GetMilliseconds(action);
            Console.WriteLine($"Time elapsed: {time} ms.");
        }

        public static void ShowTimeInMessageBox(Action action)
        {
            var time = GetMilliseconds(action);
            System.Windows.Forms.MessageBox.Show($"Time elapsed: {time} ms.");
        }
    }
}
