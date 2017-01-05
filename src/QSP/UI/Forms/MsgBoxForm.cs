using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Forms
{
    public partial class MsgBoxForm : Form
    {
        public MsgBoxForm()
        {
            InitializeComponent();
        }

        public void Init(string text, Icon icon, string caption, params string[] buttonTxt)
        {
            msgLbl.Text = text;
            SetIcon(icon);
            Text = caption;

            Debug.Assert(buttonTxt.Length <= 3);
            Button[] btns = {button1, button2, button3};
        }

        private void SetIcon(Icon image)
        {
            pictureBox1.Image = image.ToBitmap();
            pictureBox1.Size = new Size(image.Width + 15, image.Height + 15);
        }

        public enum MsgBoxIcon
        {
            Info = 0,
            Warning = 1,
            Error = 2
        }
    }
}
