using System;
using System.IO;
using System.Windows.Forms;

namespace QSP.UI.Forms
{
    public partial class LicenseForm : Form
    {
        public bool Agreed { get; private set; } = false;

        public LicenseForm()
        {
            InitializeComponent();
        }

        public void Init()
        {
            try
            {
                richTextBox1.Text = File.ReadAllText("LICENSE.txt");
            }
            catch
            {
                richTextBox1.Text =
                    "Build the application with InstallerBuilder to have this work properly.";
            }
        }

        private void agreeBtn_Click(object sender, EventArgs e)
        {
            Agreed = true;
            this.Close();
        }
    }
}
