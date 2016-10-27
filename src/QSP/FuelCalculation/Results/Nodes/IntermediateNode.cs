using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    public class IntermediateNode
    {
        public ICoordinate Coordinate { get; private set; }

        public IntermediateNode(ICoordinate Coordinate)
        {
            this.Coordinate = Coordinate;
        }
    }
}
