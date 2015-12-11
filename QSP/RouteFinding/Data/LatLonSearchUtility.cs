using System;
using System.Collections.Generic;
using System.Linq;
using QSP.AviationTools;
using static QSP.MathTools.MathTools;
using static QSP.AviationTools.AviationConstants;

namespace QSP.RouteFinding
{

    public class LatLonSearchUtility<T>
    {
        //lat, lon must be defined on T

        public delegate LatLon LatLonGetter(T item);

        private LatLonGetter latLonOfT;
        private readonly int GRID_SIZE;
        private readonly int POLAR_REGION_SIZE;

        //first index: corresponds to lat; second index: corresponds to lon
        private List<T>[,] content;
        private List<T> northPoleContent;
        private List<T> southPoleContent;

        private VisitedList visited;

        public LatLonSearchUtility(int gridSize, int polarRegSize, LatLonGetter latLonDelegate)
        {
            GRID_SIZE = gridSize;
            POLAR_REGION_SIZE = polarRegSize;
            latLonOfT = latLonDelegate;

            prepareSearch();
        }

        public LatLonSearchUtility(GridSizeOption para, LatLonGetter latLonDelegate)
        {
            switch (para)
            {
                case GridSizeOption.Small:

                    GRID_SIZE = 2;
                    POLAR_REGION_SIZE = 5;
                    break;

                case GridSizeOption.Large:
                    GRID_SIZE = 10;
                    POLAR_REGION_SIZE = 15;
                    break;
            }

            latLonOfT = latLonDelegate;
            prepareSearch();
        }


        private void prepareSearch()
        {
            content = new List<T>[(int)(Math.Ceiling(((double)(180 - 2 * POLAR_REGION_SIZE))) / GRID_SIZE),
                                  (int)(Math.Ceiling((360.0 / GRID_SIZE)))];
            initContent();

            visited = new VisitedList(content.GetLength(0), content.GetLength(1));
        }

        public enum GridSizeOption
        {
            Small,
            Large
        }

        private void initContent()
        {
            for (int i = 0; i < content.GetLength(0); i++)
            {
                for (int j = 0; j < content.GetLength(1); j++)
                {
                    content[i, j] = new List<T>();
                }
            }
            northPoleContent = new List<T>();
            southPoleContent = new List<T>();
        }

        public void Add(T item)
        {
            var latLon = latLonOfT(item);

            if (latLon.Lat >= 90 - POLAR_REGION_SIZE)
            {
                northPoleContent.Add(item);
            }
            else if (latLon.Lat < -90 + POLAR_REGION_SIZE)
            {
                southPoleContent.Add(item);
            }
            else
            {
                addContent(indicesInContent(latLon.Lat, latLon.Lon), item);
            }
        }

        private void addContent(Tuple<int, int> indices, T item)
        {
            content[indices.Item1, indices.Item2].Add(item);
        }

        private Tuple<int, int> indicesInContent(double lat, double lon)
        {
            if (lon == 180.0)
            {
                lon = -180.0;
            }
            return new Tuple<int, int>((int)(Math.Floor((lat + 90.0 - POLAR_REGION_SIZE) / GRID_SIZE)),
                                       (int)(Math.Floor((lon + 180.0) / GRID_SIZE)));
        }

        private Tuple<int, int> getGrid(double lat, double lon)
        {
            if (lat >= 90 - POLAR_REGION_SIZE)
            {
                return new Tuple<int, int>(-1, 1);
            }
            else if (lat < -90 + POLAR_REGION_SIZE)
            {
                return new Tuple<int, int>(-1, -1);
            }
            else
            {
                return indicesInContent(lat, lon);
            }
        }

        public List<T> Find(double lat, double lon, double distance)
        {
            var result = new List<T>();
            var possibleGrids = new List<Tuple<int, int>>();
            var pending = new Queue<Tuple<int, int>>();

            pending.Enqueue(getGrid(lat, lon));
            //(-1,-1) for south pole, (-1,1) for north pole
            visited.SetVisited(pending.First());

            while (pending.Count > 0)
            {
                var current = pending.Dequeue();

                foreach (var k in itemsInGrid(current))
                {
                    if (GreatCircleDistance(new LatLon(lat, lon), latLonOfT(k)) <= distance)
                    {
                        result.Add(k);
                    }
                }

                //find grids next to current grid
                //add items within the specified distance into the result
                //also checks if the node is visited
                foreach (var i in gridNeighbor(current))
                {
                    if (visited.IsVisited(i) == false && minDis(lat, lon, i) <= distance)
                    {
                        pending.Enqueue(i);
                        visited.SetVisited(i);
                    }
                }
            }

            visited.Undo();
            return result;
        }

        private List<T> itemsInGrid(Tuple<int, int> item)
        {
            if (item.Item1 == -1)
            {
                if (item.Item2 == 1)
                {
                    return northPoleContent;
                }
                else
                {
                    return southPoleContent;
                }
            }
            else
            {
                return content[item.Item1, item.Item2];
            }
        }

        public bool Remove(T item)
        {
            var latlon = latLonOfT(item);
            var gridItems = itemsInGrid(getGrid(latlon.Lat, latlon.Lon));

            for (int i = 0; i < gridItems.Count; i++)
            {
                if (item.Equals(gridItems[i]))
                {
                    gridItems.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Among all points in the grid, finds the smallest distance from the given lat/lon (in NM).
        /// </summary>
        private double minDis(double lat, double lon, Tuple<int, int> grid)
        {
            var pt = new LatLon(lat, lon);
            var center = gridCenterLatLon(grid);

            if (grid.Item1 != -1)
            {
                //not north/south pole

                double latTop = center.Lat + ((double)GRID_SIZE) / 2;
                double latBottom = center.Lat - ((double)GRID_SIZE) / 2;

                double latMaxDis = (Math.Abs(latTop) >= Math.Abs(latBottom)) ? latTop : latBottom;

                return GreatCircleDistance(pt, center) - GreatCircleDistance(center.Lat, latMaxDis, ((double)GRID_SIZE) / 2);
            }
            else if (grid.Item2 == -1)
            {
                return RADIUS_EARTH_NM * ToRadian(center.Lat - (-90 + POLAR_REGION_SIZE));
            }
            else
            {
                return RADIUS_EARTH_NM * ToRadian((90 - POLAR_REGION_SIZE) - center.Lat);
            }
        }

        private LatLon gridCenterLatLon(Tuple<int, int> item)
        {
            if (item.Item1 == -1)
            {
                if (item.Item2 == 1)
                {
                    return new LatLon(90.0, 0.0);
                }
                else
                {
                    return new LatLon(-90.0, 0.0);
                }
            }
            else
            {
                return new LatLon(-90.0 + POLAR_REGION_SIZE + (item.Item1 + 0.5) * GRID_SIZE, -180.0 + (item.Item2 + 0.5) * GRID_SIZE);
            }
        }

        private List<Tuple<int, int>> gridNeighbor(Tuple<int, int> item)
        {
            if (item.Item1 == -1)
            {
                if (item.Item2 == -1)
                {
                    return getPoleNeighbor(poleType.South);
                }
                else
                {
                    //i.e. item.Item2 = 1 
                    return getPoleNeighbor(poleType.North);
                }
            }
            else
            {
                List<Tuple<int, int>> result = new List<Tuple<int, int>>();

                int x = content.GetLength(0);
                int y = content.GetLength(1);

                result.Add(new Tuple<int, int>(item.Item1, (item.Item2 + 1) % y));
                //east
                result.Add(new Tuple<int, int>(item.Item1, (item.Item2 - 1 + y) % y));
                //west

                if (item.Item1 + 1 == x)
                {
                    result.Add(new Tuple<int, int>(-1, 1));
                    //north pole
                }
                else
                {
                    result.Add(new Tuple<int, int>(item.Item1 + 1, item.Item2));
                    //north
                }

                if (item.Item1 == 0)
                {
                    result.Add(new Tuple<int, int>(-1, -1));
                    //south pole
                }
                else
                {
                    result.Add(new Tuple<int, int>(item.Item1 - 1, item.Item2));
                    //south
                }
                return result;
            }
        }

        private enum poleType
        {
            South,
            North
        }

        private List<Tuple<int, int>> getPoleNeighbor(poleType para)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            int firstIndex = 0;

            if (para == poleType.North)
            {
                firstIndex = content.GetLength(0) - 1;
            }
            else
            {
                firstIndex = 0;
            }

            for (int i = 0; i <= content.GetLength(1) - 1; i++)
            {
                result.Add(new Tuple<int, int>(firstIndex, i));
            }
            return result;
        }

        #region "HelperClass"

        private class VisitedList
        {

            private bool[,] visited;
            private bool northPoleVisted;
            private bool southPoleVisited;
            private List<Tuple<int, int>> changedItems;

            public VisitedList(int item1, int item2)
            {
                visited = new bool[item1 + 1, item2 + 1];
                setVisitedFlag();
                changedItems = new List<Tuple<int, int>>();
            }


            private void setVisitedFlag()
            {
                for (int i = 0; i <= visited.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j <= visited.GetLength(1) - 1; j++)
                    {
                        visited[i, j] = false;
                    }
                }
                northPoleVisted = false;
                southPoleVisited = false;
            }


            public void SetVisited(Tuple<int, int> item)
            {
                setVisitedProperty(item, true);
                changedItems.Add(item);
            }


            private void setVisitedProperty(Tuple<int, int> item, bool value)
            {
                if (item.Item1 == -1)
                {
                    if (item.Item2 == 1)
                    {
                        northPoleVisted = value;
                    }
                    else
                    {
                        southPoleVisited = value;
                    }

                }
                else
                {
                    visited[item.Item1, item.Item2] = value;
                }
            }


            public void Undo()
            {
                foreach (var i in changedItems)
                {
                    setVisitedProperty(i, false);
                }
                changedItems.Clear();
            }

            public bool IsVisited(Tuple<int, int> item)
            {
                if (item.Item1 == -1)
                {
                    if (item.Item2 == -1)
                    {
                        return southPoleVisited;
                    }
                    else
                    {
                        return northPoleVisted;
                    }
                }
                else
                {
                    return visited[item.Item1, item.Item2];
                }
            }
        }

        #endregion

    }


}
