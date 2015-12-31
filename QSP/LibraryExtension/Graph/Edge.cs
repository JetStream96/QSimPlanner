using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.Graph
{
    public class Edge<TEdge> : IEdge<TEdge>
    {
        public TEdge value { get; set; }
        public int FromNodeIndex { get; set; }
        public int FromIndexInList { get; set; }
        public int ToNodeIndex { get; set; }
        public int ToIndexInList { get; set; }               

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
