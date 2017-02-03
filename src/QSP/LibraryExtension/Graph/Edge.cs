namespace QSP.LibraryExtension.Graph
{
    public class Edge<TEdge>
    {
        public TEdge Value { get; }
        public int FromNodeIndex { get; }
        public int FromIndexInList { get; }
        public int ToNodeIndex { get; }
        public int ToIndexInList { get; }

        private Edge() { }

        public Edge(TEdge Value, int FromNodeIndex, int FromIndexInList,
            int ToNodeIndex, int ToIndexInList)
        {
            this.Value = Value;
            this.FromNodeIndex = FromNodeIndex;
            this.FromIndexInList = FromIndexInList;
            this.ToNodeIndex = ToNodeIndex;
            this.ToIndexInList = ToIndexInList;
        }

        public static Edge<TEdge> Empty => new Edge<TEdge>();
    }
}
