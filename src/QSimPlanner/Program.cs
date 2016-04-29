using System;
using System.Windows.Forms;
using QSimPlanner.Data;
using QSP.Core.Options;

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
            importData();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainFrm = new QSP.MainForm();
            mainFrm.InitializeAircraftData(DataProvider.Profiles);
            mainFrm.AppSettings = DataProvider.AppSettings;

            Application.Run(mainFrm);
        }

        private static void importData()
        {
            // Aircraft data
            // This does not throw exception.
            DataProvider.InitializeProfiles();

            // Load options.
            try
            {
                DataProvider.InitializeSettings();
            }
            catch (Exception ex)
            {
                // TODO: log
                DataProvider.AppSettings = new AppOptions();

                MessageBox.Show(
                    "Cannot load options.", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
