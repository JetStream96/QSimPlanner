using System.Collections.Generic;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Airports;

namespace QSP.UI.Views.MiscInfo
{
    public interface IAirportMapView
    {
        string IcaoText { get; }
        IEnumerable<string> IcaoList { set; }
        string MetarText { set; }
        bool TransitionAltExist { set; }
        string AirportName { set; }
        ICoordinate LatLon { set; }
        int ElevationFt { set; }
        (int,int) TransitionAltLevel { set; }
        void ShowMap(ICoordinate c);
        IReadOnlyList<IRwyData> Runways { set; }
    }
}