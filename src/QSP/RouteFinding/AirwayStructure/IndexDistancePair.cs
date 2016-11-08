namespace QSP.RouteFinding.AirwayStructure
{
    public struct IndexDistancePair
    {
        public int Index { get; }
        public double Distance { get; }

        public IndexDistancePair(int Index, double Distance)
        {
            this.Index = Index;
            this.Distance = Distance;
        }
    }
}
