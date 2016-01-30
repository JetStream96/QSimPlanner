using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.RouteAnalyzers
{
    public static class Utilities
    {
        /// <summary>
        /// Merge the two route by appending RouteToMerge to Original.
        /// The first
        /// </summary>
        public static void MergeWith(this Route item, Route RouteToMerge)
        {
            if (item.Last.Equals(RouteToMerge.First))
            {
                item.ConnectRoute(RouteToMerge);
            }
            else
            {
                item.AppendRoute(RouteToMerge, "DCT");
            }
        }
    }
}
