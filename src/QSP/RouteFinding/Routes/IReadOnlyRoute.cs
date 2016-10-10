using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Routes
{
    public interface IReadOnlyRoute
    {
        RouteNode First { get; }
        RouteNode Last { get; }

    }
}
