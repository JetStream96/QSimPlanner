using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Containers;
using static QSP.AviationTools.LatLonConversion;

namespace Tests.RouteFindingTest.TestDataGenerators
{
    public class WptListGenerator
    {
        WaypointList wptlist = new WaypointList();

        public WaypointList Generate()
        {
            addWpt(90, 0);
            addWpt(-90, 0);

            for (int i = -89; i <= 89; i++)
            {
                for (int j = -180; j <= 180; j++)
                {
                    addWpt(i, j);
                }
            }

            // Establish neighbors
            addNorthPoleNeighbor();
            addSouthPoleNeighbor();

            for (int i = -89; i <= 89; i++)
            {
                for (int j = -180; j <= 180; j++)
                {
                    addWptNeighbor(i, j);
                }
            }

            return wptlist;
        }

        private void addWptNeighbor(int lat, int lon)
        {
            string ID = To7DigitFormat(lat, lon);
            int index = wptlist.FindByID(ID);

            addEastWestNeighbor(index, lat, lon, true);
            addEastWestNeighbor(index, lat, lon, false);

            addNorthSouthNeighbor(index, lat, lon, true);
            addNorthSouthNeighbor(index, lat, lon, false);
        }

        private void addNorthSouthNeighbor(int index, int lat, int lon, bool isNorth)
        {
            if (isNorth)
            {
                // North neighbor
                lat++;
                lon = (lon == 89) ? 0 : lon;
            }
            else
            {
                lat--;
                lon = (lon == -89) ? 0 : lon;
            }

            int index1 = wptlist.FindByID(To7DigitFormat(lat, lon));
            string awy = airwayName(lon, false);
            double dis = wptlist.Distance(index, index1);
            wptlist.AddNeighbor(index, index1, new Neighbor(awy, dis));
        }

        private void addEastWestNeighbor(int index, int lat, int lon, bool isWest)
        {
            if (isWest)
            {
                // West neighbor
                lon = (lon == -180) ? 179 : lon - 1;
            }
            else
            {
                lon = (lon == 180) ? -179 : lon + 1;
            }

            int index1 = wptlist.FindByID(To7DigitFormat(lat, lon));
            string awy = airwayName(lat, true);
            double dis = wptlist.Distance(index, index1);
            wptlist.AddNeighbor(index, index1, new Neighbor(awy, dis));
        }

        private void addPoleNeighbor(bool isNorthPole)
        {
            string ID = isNorthPole ? "N90E000" : "S90E000";
            int LAT = isNorthPole ? 89 : -89;

            int index = wptlist.FindByID(ID);

            var neighbors = new List<Neighbor>();

            for (int i = -180; i <= 180; i++)
            {
                int secondWpt = wptlist.FindByID(To7DigitFormat(LAT, i));
                string awy = airwayName(i, false);
                double dis = wptlist.Distance(index, secondWpt);

                wptlist.AddNeighbor(index, secondWpt, new Neighbor(awy, dis));
            }
        }

        private void addNorthPoleNeighbor()
        {
            addPoleNeighbor(true);
        }

        private void addSouthPoleNeighbor()
        {
            addPoleNeighbor(false);
        }

        private void addWpt(int lat, int lon)
        {
            wptlist.AddWpt(To7DigitFormat(lat, lon), lat, lon);
        }

        private string airwayName(int latOrLon, bool isLat)
        {
            return (isLat ? "Lat" : "Lon") + latOrLon.ToString();
        }
    }
}
