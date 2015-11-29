using System.Collections.Generic;
namespace QSP.RouteFinding.Tracks.Nats
{

    public class NorthAtlanticTrack
    {

        public NATsDir Direction { get; set; }
        public char Ident { get; set; }
        // A, B, C, etc
        public List<string> WptIdent;
        //index of each waypoint in trackedWptList
        public List<int> WptIndex;
        //TODO: This seems bad. Should have a list of Waypoints instead.

        public NorthAtlanticTrack()
        {
            WptIdent = new List<string>();
            WptIndex = new List<int>();
        }


        public NorthAtlanticTrack(NorthAtlanticTrack item)
        {
            Direction = item.Direction;
            Ident = item.Ident;
            WptIdent = new List<string>(item.WptIdent);
            WptIndex = new List<int>(item.WptIndex);

        }

    }

}
