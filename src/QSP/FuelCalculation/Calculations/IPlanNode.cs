using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using QSP.WindAloft;
using System.Collections.Generic;

namespace QSP.FuelCalculation.Calculations
{
    public interface IPlanNode : ICoordinate
    {
        object NodeValue { get; }
        IWxTableCollection WindTable { get; }
        LinkedListNode<RouteNode> NextRouteNode { get; }
        ICoordinate NextPlanNodeCoordinate { get; }
        double Alt { get; }
        double GrossWt { get; }
        double FuelOnBoard { get; }
        double TimeRemaining { get; }
        double Kias { get; }
        double Ktas { get; }
        double Gs { get; }
        Waypoint PrevWaypoint { get; }
        LinkedListNode<RouteNode> PrevRouteNode { get; }
    }
}