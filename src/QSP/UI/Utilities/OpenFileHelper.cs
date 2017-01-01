using System;
using System.Diagnostics;
using QSP.Utilities;

namespace QSP.UI.Utilities
{
    public static class OpenFileHelper
    {
        public static void TryOpenFile(string fileName)
        {
            try
            {
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                LoggerInstance.Log(ex);
                MsgBoxHelper.ShowWarning("Cannot open the specified file.");
            }
        }
    }
}