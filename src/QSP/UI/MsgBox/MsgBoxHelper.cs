namespace QSP.UI.MsgBox
{
    public static class MsgBoxHelper
    {
        public static void ShowError(string text, string caption = "")
        {
            ShowDialog(text, MsgBoxIcon.Error, caption, DefaultButton.Button1, "OK");
        }

        public static void ShowWarning(string text, string caption = "")
        {
            ShowDialog(text, MsgBoxIcon.Warning, caption, DefaultButton.Button1, "OK");
        }

        public static void ShowInfo(string text, string caption = "")
        {
            ShowDialog(text, MsgBoxIcon.Info, caption, DefaultButton.Button1, "OK");
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
