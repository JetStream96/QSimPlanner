using System;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Forms
{
    public partial class UnhandledExceptionForm : Form
    {
        public bool YesClicked { get; private set; } = false;
        public bool NoClicked { get; private set; } = false;

        public UnhandledExceptionForm()
        {
            InitializeComponent();
        }

        public void Init(string message)
        {
            richTextBox1.Text = message;
        }

        private void yesBtn_Click(object sender, EventArgs e)
        {
            YesClicked = true;
            Hide();
        }

        private void noBtn_Click(object sender, EventArgs e)
        {
            NoClicked = true;
            Hide();
        }

        private void UnhandledExceptionForm_Load(object sender, EventArgs e)
        {
            var image = SystemIcons.Error.ToBitmap();
            pictureBox1.Image = image;
            pictureBox1.Size = new Size(image.Width + 15, image.Height + 15);
        }

        public enum FormDialogResult
        {
            Yes,
            No
        }

        public static FormDialogResult ShowModal(string message)
        {
            var frm = new UnhandledExceptionForm();
            frm.Init(message);
            frm.ShowDialog();
            var result = frm.YesClicked ? FormDialogResult.Yes : FormDialogResult.No;
            frm.Close();
            return result;
        }
    }
}
