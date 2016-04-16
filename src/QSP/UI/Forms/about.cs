using System;
using System.Diagnostics;
using System.Reflection;

namespace QSP
{
    public partial class about
    {
        private void about_Load(object sender, EventArgs e)
        {
            Label2.Text = "";

            try
            {
                if (!Debugger.IsAttached)
                {
                    Label2.Text = "version " + AppProductVersion();
                }


            }
            catch { }
        }

        public static string AppProductVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }


        public about()
        {
            Load += about_Load;
            InitializeComponent();
        }
    }
}
