using QSP.UI.Forms;
using QSP.Utilities;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using QSP.UI.MsgBox;

namespace QSP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            using (var mutex = new Mutex(false, $"Global\\{GetGuid()}"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MsgBoxHelper.ShowDialogCenterScreen("QSimPlanner is already running.",
                        MsgBoxIcon.Error, "", DefaultButton.Button1, "OK");
                    return;
                }

                SetExceptionHandler();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var mainFrm = new QspForm();
                mainFrm.Init();

                Application.Run(mainFrm);
            }
        }

        private static void SetExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                HandleException((Exception)e.ExceptionObject);
            };

            Application.ThreadException += (sender, e) =>
            {
                HandleException(e.Exception);
            };

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        private static void HandleException(Exception ex)
        {
            LoggerInstance.Log(ex);

            var frm = new UnhandledExceptionForm();
            frm.Init(ex.ToString());
            frm.ShowDialog();

            Environment.Exit(1);
        }

        private static string GetGuid()
        {
            var attributes = typeof(Program).Assembly
                .GetCustomAttributes(typeof(GuidAttribute), true);

            return ((GuidAttribute)attributes[0]).Value;
        }
    }
}