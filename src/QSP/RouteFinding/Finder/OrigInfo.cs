using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Sid;

namespace QSP.RouteFinding.Finder
{
    public class OrigInfo
    {
        public string Rwy { get; }
        public IReadOnlyList<string> Sids { get; }
        public SidHandler Handler { get; }

        public OrigInfo(string Rwy, IReadOnlyList<string> Sids, SidHandler Handler)
        {
            this.Rwy = Rwy;
            this.Sids = Sids;
            this.Handler = Handler;
        }
    }
}