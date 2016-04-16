namespace QSP.RouteFinding.Data.Interfaces
{
    /// <summary>
    /// Represents any data that has latitude and longitude property.
    /// </summary>
    public interface ICoordinate
    {
        double Lat { get; }
        double Lon { get; }
    }
}
