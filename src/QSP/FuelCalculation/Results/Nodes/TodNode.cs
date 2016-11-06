using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    /// <summary>
    /// A node representing top of descent (TOD).
    /// </summary>
    public class TodNode : ICoordinate
    {
        public double Lat { get; }
        public double Lon { get; }

        public TodNode(ICoordinate c)
        {
            this.Lat = c.Lat;
            this.Lon = c.Lon;
        }
    }
}
