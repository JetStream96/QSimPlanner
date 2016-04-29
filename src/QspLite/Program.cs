using QSP.UI.ToLdgModule.Forms;
using QspLite.Data;
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
            DataProvider.InitializeProfiles();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frm = new QspLiteForm();
            frm.Initialize(DataProvider.Profiles);

            Application.Run(frm);
        }        
    }
}
