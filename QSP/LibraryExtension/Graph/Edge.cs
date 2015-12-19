using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    public class Edge<TEdge> : IEdge<TEdge>
    {
        public TEdge value;
        public int FromNodeIndex;
        public int FromIndexInList;
        public int ToNodeIndex;
        public int ToIndexInList;

        TEdge IEdge<TEdge>.value
        {
            get
            {
                return value;
            }
        }

        int IEdge<TEdge>.FromNodeIndex
        {
            get
            {
                return FromNodeIndex;
            }
        }

        int IEdge<TEdge>.FromIndexInList
        {
            get
            {
                return FromIndexInList;
            }
        }

        int IEdge<TEdge>.ToNodeIndex
        {
            get
            {
                return ToNodeIndex;
            }
        }

        int IEdge<TEdge>.ToIndexInList
        {
            get
            {
                return ToIndexInList;
            }
        }

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
