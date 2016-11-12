using System;
using System.Collections.Generic;
using QSP.FuelCalculation.Results.Nodes;
using QSP.LibraryExtension;
using static System.Math;
using static QSP.FuelCalculation.Calculations.InitialPlanCreator;

namespace QSP.FuelCalculation.Calculations
{
    public static class NodeMarker
    {
        /// <summary>
        /// Gets the index of TOC (top of climb). i.e. The first node such that
        /// the altitude no longer increases. The input (nodes) needs to
        /// have at least 2 items. Throws exception if TOC is not found.
        /// </summary>
        public static int TocIndex(IReadOnlyList<PlanNode> nodes)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                if (nodes[i + 1].Alt - nodes[i].Alt < AltDiffCriteria) return i;
            }

            throw new ArgumentException();
        }

        public static IEnumerable<int> ScIndices(IReadOnlyList<PlanNode> n)
        {
            for (int i = 1; i < n.Count - 1; i++)
            {
                if (Abs(n[i - 1].Alt - n[i].Alt) < AltDiffCriteria &&
                    n[i + 1].Alt - n[i].Alt > AltDiffCriteria)
                {
                    yield return i;
                }
            }
        }

        public static int TodIndex(IReadOnlyList<PlanNode> n)
        {
            for (int i = n.Count - 1; i > 0; i--)
            {
                if (n[i - 1].Alt - n[i].Alt < AltDiffCriteria) return i;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Mark the TOC, SC and TOD nodes.
        /// </summary>
        public static IReadOnlyList<PlanNode> Mark(IReadOnlyList<PlanNode> n)
        {
            // TOC can be the same as TOD, but SC is never the same as TOC
            // or TOD.
            var toc = TocIndex(n);
            var tod = TodIndex(n);
            var sc = ScIndices(n).ToHashSet();

            var result = new List<PlanNode>();

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

        private static PlanNode ChangeValue(PlanNode n, object nodeValue)
        {
            return new PlanNode(
                nodeValue,
                n.WindTable,
                n.NextRouteNode,
                n.NextPlanNodeCoordinate,
                n.Alt,
                n.GrossWt,
                n.FuelOnBoard,
                n.TimeRemaining,
                n.Kias,
                n.Ktas,
                n.Gs);
        }
    }
}