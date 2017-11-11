using System.Collections.Generic;
using Routes= QSP.RouteFinding.Routes;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateRowView
    {
        string ICAO { set; }

        // Can be "AUTO", or "AUTO (10)" if the runway is automatically computed.
        IEnumerable<string> RunwayList { set; }

        string DistanceInfo { set; }
        string Route { get; set; }

        void ShowMap(Routes.Route route);
        void ShowMapBrowser(Routes.Route route);
        void ShowInfo(string info);
        void ShowWarning(string warning);
    }
}
