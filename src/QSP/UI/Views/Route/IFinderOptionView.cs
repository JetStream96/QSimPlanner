using System.Collections.Generic;

namespace QSP.UI.Views.Route
{
    public interface IFinderOptionView
    {
        string Icao { get; }

        // Can be "AUTO", or "AUTO (10)" if the runway is automatically computed.
        IEnumerable<string> Runways { get; set; }

        // Can be "AUTO" or "NONE".
        IEnumerable<string> Procedures { get; set; }

        string SelectedRwy { get; set; }
        string SelectedProcedure { get; set; }

        void ShowFilter();

        void ShowError(string error);
    }
}