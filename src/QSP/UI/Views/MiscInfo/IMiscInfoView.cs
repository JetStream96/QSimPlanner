namespace QSP.UI.Views.MiscInfo
{
    public interface IMiscInfoView
    {
        IAirportMapView AirportMapView { get; }
        IDescentForcastView ForcastView { get; }
        IMetarViewerView MetarView { get; }
    }
}
