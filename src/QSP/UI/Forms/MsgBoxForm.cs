using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QSP.LibraryExtension;

namespace QSP.UI.Forms
{
    public partial class MsgBoxForm : Form
    {
        public int SelectedButton { get; private set; }

        public MsgBoxForm()
        {
            InitializeComponent();
        }

        public void Init(string text, Icon icon, string caption, string[] buttonTxt, int defaultBtn)
        {
            msgLbl.Text = text;
            SetIcon(icon);
            Text = caption;
            SetButtons(buttonTxt, defaultBtn);
        }

        private void SetButtons(string[] texts, int defaultBtn)
        {
            var len = texts.Length;
            Debug.Assert(len == 3);
            Button[] btns = { button1, button2, button3 };

            for (int i = 0; i < len; i++)
            {
                btns[i].Text = texts[i];
            }

            var btnWidth = btns.Max(b => b.Width);
            btns.ForEach(b =>
            {
                b.AutoSize = false;
                // b.MinimumSize = new Size(0, b.MinimumSize.Height);
                b.Width = btnWidth;
            });

            btns[defaultBtn].Select();
        }

        private void SetIcon(Icon image)
        {
            pictureBox1.Image = new Icon(image, 48, 48).ToBitmap();
            pictureBox1.Size = new Size(image.Width + 15, image.Height + 15);
        }
    }
}
