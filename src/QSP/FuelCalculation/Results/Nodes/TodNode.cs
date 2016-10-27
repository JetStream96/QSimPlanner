using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    /// <summary>
    /// A node representing top of descent (TOD).
    /// </summary>
    public class TodNode
    {
        public ICoordinate Coordinate { get; private set; }

        public TodNode(ICoordinate Coordinate)
        {
            this.Coordinate = Coordinate;
        }
    }
}
