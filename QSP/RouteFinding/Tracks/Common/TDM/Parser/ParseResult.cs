using System;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.Tracks.Common.Parser
{
    public sealed class ParseResult
    {
        private string[] _connectionRoutes;

        public string Ident { get; private set; }
        public string TimeStart { get; private set; }
        public string TimeEnd { get; private set; }
        public string Remarks { get; private set; }
        public string MainRoute { get; private set; }

        public ReadOnlyCollection<string> ConnectionRoutes
        {
            get
            {
                return Array.AsReadOnly(_connectionRoutes);
            }
        }

        public ParseResult(string Ident,
                           string TimeStart,
                           string TimeEnd,
                           string Remarks,
                           string MainRoute,
                           string[] ConnectionRoutes)
        {
            this.Ident = Ident;
            this.TimeStart = TimeStart;
            this.TimeEnd = TimeEnd;
            this.Remarks = Remarks;
            this.MainRoute = MainRoute;
            this._connectionRoutes = ConnectionRoutes;
        }
    }
}
