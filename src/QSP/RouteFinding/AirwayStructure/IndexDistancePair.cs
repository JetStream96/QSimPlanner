namespace QSP.RouteFinding.AirwayStructure
{
    public struct IndexDistancePair
    {
        public int Index { get; private set; }
        public double Distance { get; private set; }

        public IndexDistancePair(int Index, double Distance)
        {
            this.Index = Index;
            this.Distance = Distance;
        }
    }
}
