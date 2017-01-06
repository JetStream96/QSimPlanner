using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QSP.LibraryExtension;
using QSP.UI.Utilities;

namespace QSP.UI.MsgBox
{
    public partial class MsgBoxForm : Form
    {
        public MsgBoxResult SelectionResult { get; private set; }

        public MsgBoxForm()
        {
            InitializeComponent();
        }

        public void Init(string text, MsgBoxIcon icon, string caption,
            string[] buttonTxt, int defaultBtn)
        {
            msgLbl.Text = text;
            pictureBox1.Image = GetImage(icon);
            Text = caption;
            SetButtonText(buttonTxt, defaultBtn);
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            Buttons.ForEach((btn, index) => btn.Click += (s, e) =>
            {
                SelectionResult = (MsgBoxResult)index;
                Close();
            });
        }

        private Button[] Buttons => new[] { button1, button2, button3 };

        private void SetButtonText(string[] texts, int defaultBtn)
        {
            var len = texts.Length;
            Debug.Assert(len == 3);
            var btns = Buttons;

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

        private Image GetImage(MsgBoxIcon icon)
        {
            Image[] img =
            {
                Properties.Resources.okIcon,
                Properties.Resources.CautionIcon,
                Properties.Resources.errorIcon
            };

            return ImageUtil.Resize(img[(int)icon], new Size(48, 48));
        }
    }
}
