using System.Drawing;
using System.Windows.Forms;
using QSP.UI.MsgBox;

namespace QSP.UI.Utilities
{
    public static class MsgBoxHelper
    {
        public static DialogResult ShowError(string text, string caption = "")
        {
            return MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ShowWarning(string text, string caption = "")
        {
            ShowDialog(text, MsgBoxIcon.Warning, caption, DefaultButton.Button1, "OK");
        }

        public static DialogResult ShowInfo(string text, string caption = "")
        {
            return MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static MsgBoxResult ShowDialog(string text, MsgBoxIcon icon, string caption,
            DefaultButton defaultBtn, params string[] buttonTxt)
        {
            using (var frm = new MsgBoxForm())
            {
                frm.Init(text, icon, caption, defaultBtn, buttonTxt);
                frm.ShowDialog();
                return frm.SelectionResult;
            }
        }
    }
}
