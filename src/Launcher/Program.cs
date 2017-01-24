using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                StartApp(args.Contains("-wait"));
            }
            catch (Exception ex)
            {
                Log(ex);

                MessageBox.Show("An error occurred:\n" + ex.Message,
                    "QSimPlanner launcher error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void StartApp(bool waitForExit)
        {
            var info = new ProcessStartInfo()
            {
                WorkingDirectory = GetVersion(),
                FileName = "QSimPlanner.exe"
            };

            if (Environment.OSVersion.Version.Major >= 6) info.Verb = "runas";

            if (waitForExit)
            {
                while (Process.GetProcesses().Any(p => p.ProcessName == "QSimPlanner"))
                {
                    Thread.Sleep(500);
                }
            }

            Process.Start(info);
        }

        private static string GetVersion()
        {
            var doc = XDocument.Load("version.xml");
            return doc.Root.Element("current").Value;
        }

        private static void Log(Exception ex)
        {
            try
            {
                var msg = DateTime.UtcNow.ToString() + "\n" + ex.ToString();
                File.AppendAllText("Log.txt", msg);
            }
            catch { }
        }
    }
}
