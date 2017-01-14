using System.Collections.Generic;
using QSP.RouteFinding.TerminalProcedures.Star;

namespace QSP.RouteFinding.Finder
{
    public class DestInfo
    {
        public string Rwy { get; }
        public IReadOnlyList<string> Stars { get; }
        public StarHandler Handler { get; }

        public DestInfo(string Rwy, IReadOnlyList<string> Stars, StarHandler Handler)
        {
            this.Rwy = Rwy;
            this.Stars = Stars;
            this.Handler = Handler;
        }
    }
}