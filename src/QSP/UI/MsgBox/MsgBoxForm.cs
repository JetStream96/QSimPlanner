using QSP.LibraryExtension;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Math;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.UI.MsgBox
{
    // Supports 1, 2, or 3 buttons.
    public partial class MsgBoxForm : Form
    {
        private List<Button> activeButtons;

        public MsgBoxResult SelectionResult { get; private set; } = MsgBoxResult.Cancelled;

        public MsgBoxForm()
        {
            InitializeComponent();
        }

        public void Init(string text, MsgBoxIcon icon, string caption,
            DefaultButton btn, string[] buttonTxt)
        {
            var len = buttonTxt.Length;
            Ensure<ArgumentException>(len > (int)btn);

            SetLabelText(text);
            picBox.Image = GetImage(icon);
            Text = caption;
            activeButtons = new[] { button1, button2, button3 }.Take(len).ToList();

            ArrangeLayout(len);
            SetButtonText(buttonTxt, btn);
            SubscribeEvents();
        }

        private void SetLabelText(string text)
        {
            msgLbl.Text = text;
            var desiredWidth = msgLbl.Width + 35 + tableLayoutPanel4.GetColumnWidths()[0];
            SetWidth(Max(400, Min(550, desiredWidth)));
        }

        private void SetWidth(int width)
        {
            var p3 = tableLayoutPanel3;
            var p4 = tableLayoutPanel4;

            p3.MinimumSize = new Size(width, p3.MinimumSize.Height);
            p4.MinimumSize = new Size(width, p4.MinimumSize.Height);
            p3.MaximumSize = new Size(width, p3.MaximumSize.Height);
            p4.MaximumSize = new Size(width, p4.MaximumSize.Height);
        }

        private void ArrangeLayout(int length)
        {
            if (length == 3) return;
            button3.Visible = false;
            if (length == 2) return;
            button2.Visible = false;
            tableLayoutPanel2.Anchor = AnchorStyles.Right;

            var margin = tableLayoutPanel2.Margin;
            tableLayoutPanel2.Margin = Padding.Add(margin, new Padding(0, 0, 10, 0));
        }

        private void SubscribeEvents()
        {
            activeButtons.ForEach((btn, index) => btn.Click += (s, e) =>
            {
                SelectionResult = (MsgBoxResult)index;
                Close();
            });
        }

        private void SetButtonText(string[] texts, DefaultButton defaultBtn)
        {
            var btnWidth = activeButtons.Max(b => b.Width);

            activeButtons.ForEach((b, index) =>
            {
                b.Text = texts[index];
                b.AutoSize = false;
                b.Width = btnWidth;
            });

            activeButtons[(int)defaultBtn].Select();
        }

        private Image GetImage(MsgBoxIcon icon)
        {
            Image[] img =
            {
                Properties.Resources.infoIcon,
                Properties.Resources.CautionIcon,
                Properties.Resources.errorIcon
            };

            return ImageUtil.Resize(img[(int)icon], new Size(48, 48));
        }
    }
}
