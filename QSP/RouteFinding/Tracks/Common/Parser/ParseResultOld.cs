using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common.Parser
{
    public sealed class ParseResultOld
    {
        public string Ident { get; private set; }
        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }
        public string Remarks { get; private set; }

        public string[] MainRoute { get; private set; }
        public List<string[]> RouteFrom { get; private set; }
        public List<string[]> RouteTo { get; private set; }

        public ParseResultOld(string Ident,
                           string TimeStart,
                           string TimeEnd,
                           string Remarks,
                           string[] MainRoute,
                           List<string[]> RouteFrom,
                           List<string[]> RouteTo)
        {
            this.Ident = Ident;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
            this.Remarks = Remarks;
            this.MainRoute = MainRoute;
            this.RouteFrom = RouteFrom;
            this.RouteTo = RouteTo;
        }
    }
}
