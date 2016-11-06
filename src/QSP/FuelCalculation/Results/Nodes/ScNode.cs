using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    /// <summary>
    /// A node representing the start of step climb.
    /// </summary>
    public class ScNode : ICoordinate
    {
        public double Lat { get; }
        public double Lon { get; }

        public ScNode(ICoordinate c)
        {
            this.Lat = c.Lat;
            this.Lon = c.Lon;
        }
    }
}