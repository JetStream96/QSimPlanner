using System.Collections.Generic;

namespace QSP.UI.Controllers
{
    public interface ISelectedProcedureProvider
    {
        string Icao { get; }
        string Rwy { get; }
        List<string> GetSelectedProcedures();
    }
}
