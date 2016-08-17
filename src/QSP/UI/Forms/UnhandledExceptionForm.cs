using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Forms
{
    public partial class UnhandledExceptionForm : Form
    {
        public UnhandledExceptionForm()
        {
            InitializeComponent();
        }

        public void Init(string message)
        {
            richTextBox1.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UnhandledExceptionForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = SystemIcons.Error.ToBitmap();
        }
    }
}
