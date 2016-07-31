using QSP.AviationTools.Coordinates;
using QSP.LibraryExtension;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Data.Interfaces;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Common
{
    public static class Utilities
    {
        public static int GetClosest(
            double prevLat,
            double prevLon,
            List<int> candidates,
            WaypointList wptList)
        {
            return candidates
                .MinBy(i => wptList[i].Distance(prevLat, prevLon));
        }

        public static void ConvertLatLonFormat(string[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                LatLon result;

                if (Format7Letter.TryReadFrom7LetterFormat(item[i], out result))
                {
                    item[i] = result.To5LetterFormat();
                }
            }
        }        
    }
}
