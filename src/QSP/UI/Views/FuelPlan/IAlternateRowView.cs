using QSP.UI.Views.Route.Actions;
using System.Collections.Generic;

namespace QSP.UI.Views.FuelPlan
{
    public interface IAlternateRowView : ISupportActionContextMenu
    {
        /// <summary>
        /// Uppercase Icao code that contains no space.
        /// </summary>
        string ICAO { get; set; }

        /// <summary>
        /// Can be "AUTO", or "AUTO (10)" if the runway is automatically computed. 
        /// </summary>
        IEnumerable<string> RunwayList { set; }
    }
}
