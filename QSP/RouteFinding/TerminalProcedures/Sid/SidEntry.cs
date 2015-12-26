using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.TerminalProcedures.Sid
{
    // A SidEntry represents a "section" of Proc\AXYZ.txt (AXYZ can be replaced by any ICAO code.)
    // See TestData\Proc\AXYZ.txt for examples.
    //
    // A "section" starts with a line where first word is "SID" and ends with a empty line.
    //
    // Each SidEntry belongs to one of the following types:
    // (1) A SID for a runway. (Runway-specific)
    // (2) A common part. i.e. This SID is avail. for multiple runways, and all runways shared a common route.
    // (3) A transition for a SID.

    public class SidEntry
    {
        private string _rwy;
        private string _sidName;
        private List<Waypoint> _wpts;
        private EntryType _type;
        private bool _endWithVector;

        public string Runway
        {
            get
            {
                return _rwy;
            }
        }

        public string Name
        {
            get
            {
                return _sidName;
            }
        }

        public ReadOnlyCollection<Waypoint> Waypoints
        {
            get
            {
                return _wpts.AsReadOnly();
            }
        }

        public EntryType Type
        {
            get
            {
                return _type;
            }
        }

        public bool EndWithVector
        {
            get
            {
                return _endWithVector;
            }
        }

        public SidEntry(string rwy, string sidName, List<Waypoint> wpts, EntryType type, bool endWithVector)
        {
            _rwy = rwy;
            _sidName = sidName;
            _wpts = wpts;
            _type = type;
            _endWithVector = endWithVector;
        }

    }
}
