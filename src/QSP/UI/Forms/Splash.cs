using System;
using System.Diagnostics;
using System.Reflection;

namespace QSP
{
    public partial class Splash
    {
        public Splash()
        {
            Load += Splash_Load;
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            Label2.Text = "version 0.5.0.49";

            try
            {
                if (Debugger.IsAttached == false)
                {
                    Label2.Text = "version " + AppProductVersion();
                }

            }
            catch { }
        }

        public static string AppProductVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return FileVersionInfo.GetVersionInfo(assembly.Location)
                .ProductVersion;
        }
    }
}
