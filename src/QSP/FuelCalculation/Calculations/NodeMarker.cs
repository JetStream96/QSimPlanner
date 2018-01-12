using CommonLibrary.LibraryExtension;
using QSP.FuelCalculation.Results.Nodes;
using System.Collections.Generic;
using static QSP.FuelCalculation.Calculations.InitialPlanCreator;
using static System.Math;

namespace QSP.FuelCalculation.Calculations
{
    public static class NodeMarker
    {
        /// <summary>
        /// Gets the index of TOC (top of climb). i.e. The first node such that
        /// the altitude no longer increases. If the altitudes is an increasing
        /// sequence, returns the index of last node.
        /// 
        /// The input (nodes) needs to have at least 2 items.
        /// </summary>
        public static int TocIndex(IReadOnlyList<IPlanNode> nodes)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                if (nodes[i + 1].Alt - nodes[i].Alt < AltDiffCriteria) return i;
            }

            return nodes.Count - 1;
        }

        /// <summary>
        /// Gets the index of SC (step climb).
        /// </summary>
        public static IEnumerable<int> ScIndices(IReadOnlyList<IPlanNode> n)
        {
            for (int i = 1; i < n.Count - 1; i++)
            {
                var prev = n[i - 1].Alt;
                var current = n[i].Alt;
                var next = n[i + 1].Alt;

                if (Abs(prev - current) < AltDiffCriteria &&
                    next - current > AltDiffCriteria)
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        /// Gets the index of TOD (top of descent). i.e. The last node such that
        /// the altitude the altitude is no higher than the previous node. If the 
        /// altitudes is an increasing sequence, returns the index of first node (i.e. 0).
        /// 
        /// The input (nodes) needs to have at least 2 items.
        /// </summary>
        public static int TodIndex(IReadOnlyList<IPlanNode> n)
        {
            for (int i = n.Count - 1; i > 0; i--)
            {
                if (n[i - 1].Alt - n[i].Alt < AltDiffCriteria) return i;
            }

            return 0;
        }

        /// <summary>
        /// Mark the TOC and TOD. If SC nodes exist, they are marked as well.
        /// </summary>
        public static IReadOnlyList<IPlanNode> Mark(IReadOnlyList<IPlanNode> n)
        {
            // TOC can be the same as TOD, but SC is never the same as TOC
            // or TOD.
            var toc = TocIndex(n);
            var tod = TodIndex(n);
            var sc = ScIndices(n).ToHashSet();

            var result = new List<IPlanNode>();

            for (int i = 0; i < n.Count; i++)
            {
                var node = n[i];
                bool added = false;

                if (i == toc)
                {
                    result.Add(ChangeValue(node, new TocNode(node)));
                    added = true;
                }

                if (i == tod)
                {
                    result.Add(ChangeValue(node, new TodNode(node)));
                    added = true;
                }

                if (sc.Contains(i))
                {
                    result.Add(ChangeValue(node, new ScNode(node)));
                    added = true;
                }

                if (!added) result.Add(node);
            }

            return result;
        }

        private static IPlanNode ChangeValue(IPlanNode n, object nodeValue)
        {
            var node = PlanNode.Copy(n);
            node.NodeValue = nodeValue;
            return node;
        }
    }
}