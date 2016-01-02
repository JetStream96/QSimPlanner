using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.TerminalProcedures.Star
{
    // A StarEntry represents a "section" of Proc\ICAO.txt.
    //
    // A "section" starts with a line where first word is "STAR" and ends with a empty line.
    //
    // Each StarEntry belongs to one of the following types:
    // (1) A STAR for a runway. (Runway-specific)
    // (2) A common part. i.e. This Star is avail. for multiple runways, and all runways shared a common route.
    //     See the notes below.     
    // (3) A transition for a STAR.
    //
    // Notes:
    // (1) If, for a STAR, no runway-specific part exists and a common part exist, then this STAR is avail. for all runways.
    //     On the other hand, for a STAR which has both runway-specific and common part, then this STAR is only available for
    //     the runways mentioned in the runway-specific part.

    public class StarEntry : IProcedureEntry
    {
        private List<Waypoint> _wpts;

        public string RunwayOrTransition { get; private set; }
        public string Name { get; private set; }

        public ReadOnlyCollection<Waypoint> Waypoints
        {
            get
            {
                return _wpts.AsReadOnly();
            }
        }

        public EntryType Type { get; private set; }

        public StarEntry(string rwy, string starName, List<Waypoint> wpts, EntryType type)
        {
            this.RunwayOrTransition = rwy;
            this.Name = starName;
            this._wpts = wpts;
            this.Type = type;
        }
    }
}
