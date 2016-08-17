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
            var image = SystemIcons.Error.ToBitmap();
            pictureBox1.Image = image;
            pictureBox1.Size = new Size(image.Width + 15, image.Height + 15);
        }
    }
}
