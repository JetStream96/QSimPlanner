using QSP.AircraftProfiles.Configs;
using QSP.UI.ToLdgModule.Forms;
using QSP.UI.Utilities;
using QSP.Utilities;
using QspLite.GlobalInfo;
using System;
using System.Windows.Forms;

namespace QspLite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var domain = AppDomain.CurrentDomain;
            domain.UnhandledException += (sender, e) =>
            {
                var msg = ((Exception)e.ExceptionObject).Message;
                LoggerInstance.WriteToLog(msg);
                MsgBoxHelper.ShowError(
                    "An unexpected error occurred. " +
                    "The application will now quit.");
                Environment.Exit(0);
            };

            try
            {
                Information.InitializeProfiles();
            }
            catch (PerfFileNotFoundException ex)
            {
                LoggerInstance.WriteToLog(ex);
                MsgBoxHelper.ShowWarning(ex.Message);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frm = new QspLiteForm();
            frm.Initialize(Information.Profiles);

            Application.Run(frm);
        }
    }
}
