using QSP.UI.Models.MiscInfo;

namespace QSP.UI.Views.MiscInfo
{
    public interface IMetarViewerView
    {
        string MetarText { set; }
        string LastUpdateTime { set; }
        MetarViewStatus Status { set; }
        string Icao { get; }
    }
}