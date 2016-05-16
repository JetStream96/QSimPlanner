using QSP.AviationTools.Coordinates;
using QSP.Core;
using QSP.LibraryExtension;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.Utilities;
using static QSP.Utilities.ConditionChecker;

namespace QSP.RouteFinding.Data
{
    public class LatLonSearcher<T> where T : ICoordinate, IEquatable<T>
    {
        private int gridSize;
        private int polarRegionSize;

        //first index: corresponds to lat; second index: corresponds to lon
        private List<T>[,] content;
        private List<T> northPoleContent;
        private List<T> southPoleContent;

        private VisitedList visited;
        private IEqualityComparer<T> equalComp;

        public LatLonSearcher(int gridSize, int polarRegSize)
            : this(gridSize, polarRegSize, EqualityComparer<T>.Default)
        { }

        public LatLonSearcher(int gridSize, int polarRegionSize,
            IEqualityComparer<T> equalComp)
        {
            init(gridSize, polarRegionSize, equalComp);
        }

        public LatLonSearcher(GridSizeOption para) : this(para, EqualityComparer<T>.Default)
        { }

        public LatLonSearcher(GridSizeOption para, IEqualityComparer<T> equalComp)
        {
            switch (para)
            {
                case GridSizeOption.Small:
                    init(2, 5, equalComp);
                    break;

                case GridSizeOption.Large:
                    init(10, 15, equalComp);
                    break;

                default:
                    throw new EnumNotSupportedException();
            }
        }

        private void init(int gridSize, int polarRegionSize,
            IEqualityComparer<T> equalComp)
        {
            //Ensure<ArgumentException>(
            //    gridSize>0 && polarRegionSize>0 && gridSize <???)

            this.gridSize = gridSize;
            this.polarRegionSize = polarRegionSize;
            this.equalComp = equalComp;
            prepareSearch();
        }

        private void prepareSearch()
        {
            content = new List<T>[(int)(Math.Ceiling(((double)(180 - 2 * polarRegionSize))) / gridSize),
                                  (int)(Math.Ceiling((360.0 / gridSize)))];
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
            if (item.Lat >= 90 - polarRegionSize)
            {
                northPoleContent.Add(item);
            }
            else if (item.Lat < -90 + polarRegionSize)
            {
                southPoleContent.Add(item);
            }
            else
            {
                addContent(indicesInContent(item.Lat, item.Lon), item);
            }
        }

        private void addContent(Pair<int, int> indices, T item)
        {
            content[indices.Item1, indices.Item2].Add(item);
        }

        private Pair<int, int> indicesInContent(double lat, double lon)
        {
            if (lon == 180.0)
            {
                lon = -180.0;
            }
            return new Pair<int, int>((int)((lat + 90.0 - polarRegionSize) / gridSize),
                                       (int)((lon + 180.0) / gridSize));
        }

        private Pair<int, int> getGrid(double lat, double lon)
        {
            if (lat >= 90 - polarRegionSize)
            {
                return new Pair<int, int>(-1, 1);
            }
            else if (lat < -90 + polarRegionSize)
            {
                return new Pair<int, int>(-1, -1);
            }
            else
            {
                return indicesInContent(lat, lon);
            }
        }

        public List<T> Find(double lat, double lon, double distance)
        {
            var result = new List<T>();
            var possibleGrids = new List<Pair<int, int>>();
            var pending = new Queue<Pair<int, int>>();

            pending.Enqueue(getGrid(lat, lon));
            //(-1,-1) for south pole, (-1,1) for north pole
            visited.SetVisited(pending.First());

            while (pending.Count > 0)
            {
                var current = pending.Dequeue();

                foreach (var k in itemsInGrid(current))
                {
                    if (GreatCircleDistance(lat, lon, k.Lat, k.Lon) <= distance)
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

        private List<T> itemsInGrid(Pair<int, int> item)
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
            var gridItems = itemsInGrid(getGrid(item.Lat, item.Lon));

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
        /// Among all points in the grid, finds the smallest distance 
        /// from the given lat/lon (in NM).
        /// </summary>
        private double minDis(double lat, double lon, Pair<int, int> grid)
        {
            var pt = new LatLon(lat, lon);
            var center = gridCenterLatLon(grid);

            if (grid.Item1 != -1)
            {
                //not north/south pole

                double latTop = center.Lat + gridSize / 2.0;
                double latBottom = center.Lat - gridSize / 2.0;

                double latMaxDis = (Math.Abs(latTop) >= Math.Abs(latBottom)) ? latTop : latBottom;

                return GreatCircleDistance(pt, center) - GreatCircleDistance(center.Lat, latMaxDis,
                    gridSize / 2.0);
            }
            else if (grid.Item2 == -1)
            {
                return EarthRadiusNm * ToRadian(center.Lat - (-90 + polarRegionSize));
            }
            else
            {
                return EarthRadiusNm * ToRadian((90 - polarRegionSize) - center.Lat);
            }
        }

        private LatLon gridCenterLatLon(Pair<int, int> item)
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
                return new LatLon(-90.0 + polarRegionSize + (item.Item1 + 0.5) * gridSize, -180.0 + (item.Item2 + 0.5) * gridSize);
            }
        }

        private List<Pair<int, int>> gridNeighbor(Pair<int, int> item)
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
                var result = new List<Pair<int, int>>();

                int x = content.GetLength(0);
                int y = content.GetLength(1);

                result.Add(new Pair<int, int>(item.Item1, (item.Item2 + 1) % y));
                //east
                result.Add(new Pair<int, int>(item.Item1, (item.Item2 - 1 + y) % y));
                //west

                if (item.Item1 + 1 == x)
                {
                    result.Add(new Pair<int, int>(-1, 1));
                    //north pole
                }
                else
                {
                    result.Add(new Pair<int, int>(item.Item1 + 1, item.Item2));
                    //north
                }

                if (item.Item1 == 0)
                {
                    result.Add(new Pair<int, int>(-1, -1));
                    //south pole
                }
                else
                {
                    result.Add(new Pair<int, int>(item.Item1 - 1, item.Item2));
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

        private List<Pair<int, int>> getPoleNeighbor(poleType para)
        {
            var result = new List<Pair<int, int>>();
            int firstIndex = 0;

            if (para == poleType.North)
            {
                firstIndex = content.GetLength(0) - 1;
            }
            else
            {
                firstIndex = 0;
            }

            for (int i = 0; i < content.GetLength(1); i++)
            {
                result.Add(new Pair<int, int>(firstIndex, i));
            }
            return result;
        }

        #region "HelperClass"

        private struct Grid
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public Grid(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }

            public Grid(bool IsNorthPole)
            {
                if (IsNorthPole)
                {
                    X = -1;
                    Y = 1;
                }
                else
                {
                    X = -1;
                    Y = -1;
                }
            }

            public bool IsNorthPole
            {
                get
                {
                    return X == -1 && Y == 1;
                }
            }

            public bool IsSouthPole
            {
                get
                {
                    return X == -1 && Y == -1;
                }
            }
        }

        private class VisitedList
        {
            private bool[,] visited;
            private bool northPoleVisted;
            private bool southPoleVisited;
            private List<Pair<int, int>> changedItems;

            public VisitedList(int item1, int item2)
            {
                visited = new bool[item1 + 1, item2 + 1];
                setVisitedFlag();
                changedItems = new List<Pair<int, int>>();
            }

            private void setVisitedFlag()
            {
                for (int i = 0; i < visited.GetLength(0); i++)
                {
                    for (int j = 0; j < visited.GetLength(1); j++)
                    {
                        visited[i, j] = false;
                    }
                }
                northPoleVisted = false;
                southPoleVisited = false;
            }

            public void SetVisited(Pair<int, int> item)
            {
                setVisitedProperty(item, true);
                changedItems.Add(item);
            }

            private void setVisitedProperty(Pair<int, int> item, bool value)
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

            public bool IsVisited(Pair<int, int> item)
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
