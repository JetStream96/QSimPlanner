using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    /// <summary>
    /// A node representing top of climb (TOC).
    /// </summary>
    public class TocNode
    {
        public ICoordinate Coordinate { get; private set; }

        public TocNode(ICoordinate Coordinate)
        {
            this.Coordinate = Coordinate;
        }
    }
}
