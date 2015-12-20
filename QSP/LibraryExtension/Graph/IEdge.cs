using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
