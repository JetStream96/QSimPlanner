using QSP.UI.Models.Wind;

namespace QSP.UI.Forms
{
    public interface IWindDataView
    {
        void ShowWarning(string message);
        void ShowWindStatus(WindDownloadStatus item);
        WindDownloadStatus ToolStripWindStatus { get; }
    }
}