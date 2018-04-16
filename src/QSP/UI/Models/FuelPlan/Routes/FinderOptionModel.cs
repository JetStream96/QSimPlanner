using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using System;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public class FinderOptionModel
    {
        public bool IsDepartureAirport { get; set; }
        public Locator<AppOptions> AppOptions { get; set; }
        public Func<AirportManager> AirportList { get; set; }
        public Func<WaypointList> WptList { get; set; }
        public ProcedureFilter ProcFilter { get; set; }
    }
}
