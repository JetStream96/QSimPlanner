using QSP.RouteFinding.TerminalProcedures.Sid;
using System.Collections.Generic;

namespace QSP.UI.Controllers
{
    public class DestinationSidSelection : ISelectedProcedureProvider
    {
        private RouteFinderSelection destination;

        public DestinationSidSelection(RouteFinderSelection destination)
        {
            this.destination = destination;
        }

        public string Icao => destination.Icao;

        public string Rwy => destination.Rwy;

        public List<string> GetSelectedProcedures()
        {
            return SidHandlerFactory.GetHandler(
                Icao,
                destination.NavDataLocation,
                destination.WptList,
                destination.WptList.GetEditor(),
                destination.AirportList)
                .GetSidList(Rwy);
        }
    }
}
