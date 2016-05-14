using System.Windows.Forms;

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

        public static DialogResult ShowWarning(string text, string caption = "")
        {
            return MessageBox.Show(
                text,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
