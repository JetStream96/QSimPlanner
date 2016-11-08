using QSP.RouteFinding.Routes.TrackInUse;

namespace QSP.RouteFinding.Routes
{
    // The use of this class is to display a route with tracks (e.g. NATs) in 
    // 2 different forms:
    // (1) Folded form: ... ERAKA NATA SAVRY ...
    // (2) Expanded form: ... ERAKA N60W020 N62W030 N62W040 N61W050 SAVRY ...
    //
    public class RouteGroup
    {
        public Route Folded { get; }
        public Route Expanded { get; }

        public RouteGroup(Route Folded, TrackInUseCollection tracks)
        {
            this.Folded = Folded;
            Expanded = new Route(Folded);

            foreach (var i in tracks.AllEntries)
            {
                Expanded.Nodes.InsertRoute(i.Route, i.RouteName);
            }
        }
    }
}
