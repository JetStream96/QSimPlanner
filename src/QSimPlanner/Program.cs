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
            mainFrm.Initialize(Information.Profiles);
            mainFrm.AppSettings = Information.AppSettings;

            Application.Run(mainFrm);
        }

        private static void initData()
        {
            // Aircraft data
            // This does not throw exception.
            Information.InitializeProfiles();

            // Load options.
            try
            {
                Information.InitializeSettings();
            }
            catch (Exception ex)
            {
                LoggerInstance.WriteToLog(ex);

                Information.AppSettings = new AppOptions();

                MessageBox.Show(
                    "Cannot load options.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
