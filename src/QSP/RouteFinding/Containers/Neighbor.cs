namespace QSP.RouteFinding.Containers
{
    // This class is immutable.
    public class Neighbor
    {
        public string Airway { get; private set; }
        public AirwayType AirwayType { get; private set; }
        public double Distance { get; private set; }

        public Neighbor(string Airway, AirwayType AirwayType, double Distance)
        {
            this.Airway = Airway;
            this.AirwayType = AirwayType;
            this.Distance = Distance;
        }
    }

    public enum AirwayType // TODO: This actually means: Supress wind correction.
    {
        Enroute,
        Terminal    // SID or STAR
    }
}
