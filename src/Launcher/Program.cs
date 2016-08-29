using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StartApp();
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

        private static void StartApp()
        {
            var ver = GetVersion();
            var info = new ProcessStartInfo();
            info.WorkingDirectory = ver;
            info.FileName = "QSimPlan.exe";

            if (Environment.OSVersion.Version.Major >= 6)
            {
                info.Verb = "runas";
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
