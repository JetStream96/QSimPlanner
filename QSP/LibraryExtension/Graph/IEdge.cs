namespace QSP.LibraryExtension.Graph
{
    public interface IEdge<TEdge>
    {
        TEdge value { get; }
        int FromNodeIndex { get; }
        int FromIndexInList { get; }
        int ToNodeIndex { get; }
        int ToIndexInList { get; }
    }
}
