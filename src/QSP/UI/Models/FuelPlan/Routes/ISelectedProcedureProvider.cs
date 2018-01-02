using System.Collections.Generic;

namespace QSP.UI.Models.FuelPlan.Routes
{
    /// <summary>
    /// Provides the airport information selected by the user.
    /// </summary>
    public interface ISelectedProcedureProvider
    {
        string Icao { get; }
        string Rwy { get; }
        IEnumerable<string> GetSelectedProcedures();
    }
}
