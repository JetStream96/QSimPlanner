using QSP.UI.Models.Wind;

namespace QSP.UI.UserControls
{
    public interface IWindDataView
    {
        void ShowWarning(string message);
        void ShowWindStatus(WindDownloadStatus item);
        WindDownloadStatus ToolStripWindStatus { get; }
    }
}