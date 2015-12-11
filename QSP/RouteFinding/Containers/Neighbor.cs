namespace QSP.RouteFinding.Containers
{

    public class Neighbor
    {
        public int Index { get; set; }
        public string Airway { get; set; }
        public double Distance { get; set; }

        public Neighbor() { }

        public Neighbor(int Index, string Airway, double Distance)
        {
            this.Index = Index;
            this.Airway = Airway;
            this.Distance = Distance;
        }

        /// <summary>
        /// Value comparison of two Neighbors.
        /// </summary>
        public bool Equals(Neighbor x)
        {
            if (Index == x.Index && Airway == x.Airway && Distance == x.Distance)
            {
                return true;
            }
            return false;
        }

    }

}
