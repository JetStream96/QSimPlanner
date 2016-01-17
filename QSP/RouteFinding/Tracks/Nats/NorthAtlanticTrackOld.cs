using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NorthAtlanticTrackOld
    {
        public NatsDirection Direction { get; private set; }
        public char Ident { get; private set; }     // A, B, C, etc        
        public List<string> WptIdent;       // Indices of each waypoint in trackedWptList        
        public List<int> WptIndex;          //TODO: This seems bad. Should have a list of Waypoints instead.

        public NorthAtlanticTrackOld()
        {
            WptIdent = new List<string>();
            WptIndex = new List<int>();
        }

        public NorthAtlanticTrackOld(NatsDirection Direction, char Ident, List<string> WptIdent, List<int> WptIndex)
        {
            this.Direction = Direction;
            this.Ident = Ident;
            this.WptIdent = WptIdent;
            this.WptIndex = WptIndex;
        }

        public NorthAtlanticTrackOld(NorthAtlanticTrackOld item)
        {
            Direction = item.Direction;
            Ident = item.Ident;
            WptIdent = new List<string>(item.WptIdent);
            WptIndex = new List<int>(item.WptIndex);
        }
    }
}
