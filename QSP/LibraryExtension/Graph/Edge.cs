namespace QSP.LibraryExtension.Graph
{
    public class Edge<TEdge> 
    {
        public TEdge value { get; private set; }
        public int FromNodeIndex { get; private set; }
        public int FromIndexInList { get; private set; }
        public int ToNodeIndex { get; private set; }
        public int ToIndexInList { get; private set; }

        public Edge() { }

        public Edge(TEdge value, int FromNodeIndex, int FromIndexInList, int ToNodeIndex, int ToIndexInList)
        {
            this.value = value;
            this.FromNodeIndex = FromNodeIndex;
            this.FromIndexInList = FromIndexInList;
            this.ToNodeIndex = ToNodeIndex;
            this.ToIndexInList = ToIndexInList;
        }
    }
}
