using QSP.UI.ToLdgModule.Forms;
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
            Information.InitializeProfiles();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frm = new QspLiteForm();
            frm.Initialize(Information.Profiles);

            Application.Run(frm);
        }        
    }
}
