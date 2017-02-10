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
                FileName = "QSimPlanner.exe",
                Arguments = "-launcher"
            };

            if (Environment.OSVersion.Version.Major >= 6) info.Verb = "runas";

            if (!waitForExit)
            {
                Process.Start(info);
                return;
            }

            if (WaitAppToExit())
            {
                Process.Start(info);
            }
            else
            {
                MessageBox.Show("QSimPlanner is still running. Please close the application " +
                    "before starting another instance.", "",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
            }
        }

        // Returns whether the app eventually exits.
        private static bool WaitAppToExit()
        {
            for (int i = 0; i < 50; i++)
            {
                if (!Process.GetProcesses().Any(p => p.ProcessName == "QSimPlanner")) return true;
                Thread.Sleep(500);
            }

            return false;
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
