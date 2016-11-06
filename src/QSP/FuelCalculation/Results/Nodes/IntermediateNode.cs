using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    public class IntermediateNode : ICoordinate
    {
        public double Lat { get; }
        public double Lon { get; }

        public IntermediateNode(ICoordinate c)
        {
            this.Lat = c.Lat;
            this.Lon = c.Lon;
        }
    }
}
