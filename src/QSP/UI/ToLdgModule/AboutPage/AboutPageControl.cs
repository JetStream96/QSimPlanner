using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace QSP.UI.ToLdgModule.AboutPage
{
    public partial class AboutPageControl : UserControl
    {
        public AboutPageControl()
        {
            InitializeComponent();
            initControls();
        }

        private void initControls()
        {
            panel1.BackColor = Color.FromArgb(160, Color.White);
        }

        private void licenseBtn_Click(object sender, EventArgs e)
        {
            Process.Start("LICENSE.txt");
        }

        private void siteBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://qsimplan.wordpress.com/");
        }

        private void githubBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/JetStream96/QSimPlanner");
        }
    }
}
