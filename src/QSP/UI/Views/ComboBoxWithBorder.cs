using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QSP.UI.Views
{
    public class ComboBoxWithBorder : ComboBox
    {
        private Color _borderColor = Color.DimGray;
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private int myLocalSelectedIndex = -1;

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (myLocalSelectedIndex == SelectedIndex) return;
            myLocalSelectedIndex = SelectedIndex;
            base.OnSelectedIndexChanged(e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F)  //WM_PAINT
            {
                ControlPaint.DrawBorder(Graphics.FromHwnd(Handle),
                                        new Rectangle(0, 0, Width, Height),
                                        _borderColor,
                                        _borderStyle);
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }

            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get
            {
                return _borderStyle;
            }

            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }
    }
}
