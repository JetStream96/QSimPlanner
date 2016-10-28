using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    /// <summary>
    /// A node representing the start of step climb.
    /// </summary>
    public class ScNode
    {
        public ICoordinate Coordinate { get; private set; }

        public ScNode(ICoordinate Coordinate)
        {
            this.Coordinate = Coordinate;
        }
    }
}