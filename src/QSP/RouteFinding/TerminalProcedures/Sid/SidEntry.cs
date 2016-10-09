using QSP.RouteFinding.Containers;
using System.Collections.Generic;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // A SidEntry represents a "section" of Proc\AXYZ.txt 
    // (AXYZ can be replaced by any ICAO code.)
    // See TestData\Proc\AXYZ.txt for examples.
    //
    // A "section" starts with a line where first word is "SID" and ends 
    // with a empty line.
    //
    // Each SidEntry belongs to one of the following types:
    // (1) A SID for a runway. (Runway-specific)
    // (2) A common part. i.e. This SID is available for multiple runways, 
    //     and all runways shared a common route.
    //     See the notes below.     
    // (3) A transition for a SID.
    //
    // Notes:
    // (1) If, for a SID, no runway-specific part exists and a common 
    //     part exist, then this SID is available for all runways.
    //     On the other hand, for a SID which has both runway-specific 
    //     and common part, then this SID is only available for
    //     the runways mentioned in the  runway-specific part.
    // (2) In constructor, the paramerter 'wpts' does not include 
    //     the runway.

    public class SidEntry : IProcedureEntry
    {
        public string RunwayOrTransition { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyList<Waypoint> Waypoints { get; private set; }
        public EntryType Type { get; private set; }
        public bool EndWithVector { get; private set; }
        
        public SidEntry(
            string RunwayOrTransition,
            string SidName,
            IReadOnlyList<Waypoint> Waypoints,
            EntryType Type,
            bool EndWithVector)
        {
            this.RunwayOrTransition = RunwayOrTransition;
            this.Name = SidName;
            this.Waypoints = Waypoints;
            this.Type = Type;
            this.EndWithVector = EndWithVector;
        }
    }
}
