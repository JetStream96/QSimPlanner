using System;
namespace QSP
{
    public partial class Splash
    {
        private void Splash_Load(object sender, EventArgs e)
        {
            Label2.Text = "version 0.5.0.49";

            try
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    Label2.Text = "version " + about.AppProductVersion();
                }

            }
            catch { }

        }

        public Splash()
        {
            Load += Splash_Load;
            InitializeComponent();
        }
    }
}
