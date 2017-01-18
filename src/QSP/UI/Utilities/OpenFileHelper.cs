using System;
using System.Diagnostics;
using System.Windows.Forms;
using QSP.UI.MsgBox;
using QSP.Utilities;

namespace QSP.UI.Utilities
{
    public static class OpenFileHelper
    {
        public static void TryOpenFile(string fileName, Control parentControl = null)
        {
            try
            {
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
                MsgBoxHelper.ShowWarning(parentControl, "Cannot open the specified file.");
            }
        }
    }
}