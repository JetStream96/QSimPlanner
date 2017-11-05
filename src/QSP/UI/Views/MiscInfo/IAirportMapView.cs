using System.Collections.Generic;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.UI.Views.MiscInfo
{
    public interface IAirportMapView
    {
        string IcaoText { get; }
        IEnumerable<string> IcaoList { set; }
        string MetarText { set; }
        bool TransitionAltExist { set; }
        string AirportName { set; }
        string LatLon { set; }
        string Elevation { set; }
        string TransitionAltLevel { set; }
        void ShowMap(ICoordinate c);
    }
}