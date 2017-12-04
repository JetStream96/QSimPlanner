using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.TerminalProcedures;
using System;

namespace QSP.UI.Models.FuelPlan.Routes
{
    public interface IFinderOptionModel
    {
        bool IsDepartureAirport { get; }
        Locator<AppOptions> AppOptions { get; }
        Func<AirportManager> AirportList { get; }
        Func<WaypointList> WptList { get; }
        ProcedureFilter ProcFilter { get; }
    }

    public class FinderOptionModel : IFinderOptionModel
    {
        public bool IsDepartureAirport { get; private set; }
        public Locator<AppOptions> AppOptions { get; private set; }
        public Func<AirportManager> AirportList { get; private set; }
        public Func<WaypointList> WptList { get; private set; }
        public ProcedureFilter ProcFilter { get; private set; }

        public FinderOptionModel(
            bool IsDepartureAirport,
            Locator<AppOptions> AppOptions,
            Func<AirportManager> AirportList,
            Func<WaypointList> WptList,
            ProcedureFilter ProcFilter)
        {
            this.IsDepartureAirport = IsDepartureAirport;
            this.AppOptions = AppOptions;
            this.AirportList = AirportList;
            this.WptList = WptList;
            this.ProcFilter = ProcFilter;
        }
    }
}
