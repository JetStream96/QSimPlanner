namespace QSP.UI.Views.MiscInfo
{
    public interface IDescentForcastView
    {
        string LastUpdateTime { set; }
        string DestinationIcao { set; }
        string Forcast { set; }
    }
}
