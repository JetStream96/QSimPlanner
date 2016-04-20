using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.ToLdgModule.Forms;
using QspLite.Data;

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
            AircraftProfiles.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var frm = new QspLiteForm();
            frm.Initialize(AircraftProfiles.Profiles);

            Application.Run(frm);
        }
    }
}
