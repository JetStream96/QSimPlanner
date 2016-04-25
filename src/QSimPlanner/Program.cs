using System;
using System.Windows.Forms;
using QSimPlanner.Data;

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
            AircraftProfiles.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainFrm = new QSP.MainForm();
            mainFrm.InitializeAircraftData(AircraftProfiles.Profiles);

            Application.Run(mainFrm);
        }
    }
}
