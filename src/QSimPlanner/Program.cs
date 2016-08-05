using QSP.UI.Utilities;
using QSP.Utilities;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace QSimPlanner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var mutex = new Mutex(false, $"Global\\{GetGuid()}"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MsgBoxHelper.ShowError("QSimPlanner is already running.");
                    return;
                }

                var domain = AppDomain.CurrentDomain;
                domain.UnhandledException += (sender, e) =>
                {
                    LoggerInstance.WriteToLog((Exception)e.ExceptionObject);
                    MsgBoxHelper.ShowError(
                        "An unexpected error occurred. " +
                        "The application will now quit.");
                    Environment.Exit(1);
                };

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var mainFrm = new QSP.UI.Forms.QspForm();
                mainFrm.Init();

                Application.Run(mainFrm);
            }
        }

        static string GetGuid()
        {
            var attributes = typeof(Program).Assembly
                .GetCustomAttributes(typeof(GuidAttribute), true);

            return ((GuidAttribute)attributes[0]).Value;
        }
    }
}