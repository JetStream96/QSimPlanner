using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common.TDM.Parser
{
    public sealed class ParseResult
    {
        public string Ident { get; private set; }
        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }
        public string Remarks { get; private set; }
        public string MainRoute { get; private set; }
        public IReadOnlyList<string> ConnectionRoutes { get; private set; }

        public ParseResult(
            string Ident,
            string TimeStart,
            string TimeEnd,
            string Remarks,
            string MainRoute,
            IReadOnlyList<string> ConnectionRoutes)
        {
            this.Ident = Ident;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
            this.Remarks = Remarks;
            this.MainRoute = MainRoute;
            this.ConnectionRoutes = ConnectionRoutes;
        }
    }
}
