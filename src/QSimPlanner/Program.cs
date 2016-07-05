using QSP.UI.Utilities;
using QSP.Utilities;
using System;
using System.Windows.Forms;

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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var mainFrm = new QSP.UI.Forms.QspForm();
            var mainFrm = new QSP.MainForm();
            mainFrm.Init();
            
            Application.Run(mainFrm);
        }        
    }
}
