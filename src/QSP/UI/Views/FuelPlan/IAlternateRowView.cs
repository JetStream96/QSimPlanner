using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateRowView
    {
        string ICAO { set; }

        // Can be "AUTO", or "AUTO (10)" if the runway is automatically computed.
        IEnumerable<string> RunwayList { set; }

        string DistanceInfo { set; }
    }
}
