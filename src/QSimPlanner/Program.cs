using QSimPlanner.GlobalInfo;
using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
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

            var mainFrm = new QSP.MainForm();
            prepareApp(mainFrm);

            Application.Run(mainFrm);
        }

        private static void prepareApp(QSP.MainForm mainFrm)
        {
            var splash = new QSP.Splash();
            splash.Show();
            splash.Refresh();

            initData();

            mainFrm.Init(
                Information.Profiles,
                Information.AppSettings,
                Information.AirportList,
                Information.WptList);

            splash.Close();
        }

        private static void initData()
        {
            try
            {
                // Aircraft data
                Information.InitProfiles();
            }
            catch (PerfFileNotFoundException ex)
            {
                LoggerInstance.WriteToLog(ex);
                MsgBoxHelper.ShowWarning(ex.Message);
            }

            // Load options.
            try
            {
                Information.InitSettings();
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);

                MessageBox.Show(
                    "Cannot load options.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Airports and waypoints
            // TODO: exceptions?
            try
            {
                Information.InitAirportList();
                Information.InitWptList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
