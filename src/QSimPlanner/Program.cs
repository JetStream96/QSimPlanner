using QSimPlanner.GlobalInfo;
using QSP.Core.Options;
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
            initData();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainFrm = new QSP.MainForm();

            mainFrm.Initialize(
                Information.Profiles, 
                Information.AppSettings,
                Information.AirportList);

            Application.Run(mainFrm);
        }

        private static void initData()
        {
            // Aircraft data
            // This does not throw exception.
            Information.InitProfiles();

            // Load options.
            try
            {
                Information.InitSettings();
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);

                Information.AppSettings = new AppOptions();

                MessageBox.Show(
                    "Cannot load options.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Airports and waypoints
            Information.InitAirportList();
            //Information.InitializeWptList();
        }
    }
}
