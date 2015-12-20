namespace QSP.RouteFinding.Containers
{

    public class Neighbor
    {
        // This class is immutable.

        private string _airway;
        private double _distance;

        public string Airway
        {
            get
            {
                return _airway;
            }
        }

        public double Distance
        {
            get
            {
                return _distance;
            }
        }

        public Neighbor(string Airway, double Distance)
        {
            _airway = Airway;
            _distance = Distance;
        }

        /// <summary>
        /// Value comparison of two Neighbors.
        /// </summary>
        public bool Equals(Neighbor x)
        {
            return (_airway == x.Airway && _distance == x.Distance);
        }

    }

}
