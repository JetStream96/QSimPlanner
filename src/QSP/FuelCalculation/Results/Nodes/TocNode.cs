using QSP.RouteFinding.Data.Interfaces;

namespace QSP.FuelCalculation.Results.Nodes
{
    /// <summary>
    /// A node representing top of climb (TOC).
    /// </summary>
    public class TocNode : ICoordinate
    {
        public double Lat { get; }
        public double Lon { get; }

        public TocNode(ICoordinate c)
        {
            this.Lat = c.Lat;
            this.Lon = c.Lon;
        }
    }
}
