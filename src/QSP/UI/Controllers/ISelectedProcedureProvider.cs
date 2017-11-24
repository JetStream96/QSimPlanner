using QSP.UI.Presenters.FuelPlan.Route;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Controllers
{
    /// <summary>
    /// Provides the airport information selected by the user.
    /// </summary>
    public interface ISelectedProcedureProvider
    {
        string Icao { get; }
        string Rwy { get; }
        List<string> GetSelectedProcedures();//TODO:Use IEnumerable instead?
    }
}
