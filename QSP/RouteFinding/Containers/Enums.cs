using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Containers
{
    public enum ChangeCategory
    {
        Normal,
        Nats,
        Pacots,
        Ausots
    }

    public enum TrackChangesOption
    {
        Yes,
        No,
        AddingNATs,
        AddingPacots,
        AddingAusots
    }
}
