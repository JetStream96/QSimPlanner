using QSP.AviationTools.Coordinates;
using QSP.Common;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.GCDis;

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

        private void addContent(Grid indices, T item)
        {
            content[indices.X, indices.Y].Add(item);
        }

        private Grid indicesInContent(double lat, double lon)
        {
            if (lon == 180.0)
            {
                lon = -180.0;
            }
            return new Grid(
                (int)((lat + 90.0 - polarRegionSize) / gridSize),
                (int)((lon + 180.0) / gridSize));
        }

        private Grid getGrid(double lat, double lon)
        {
            if (lat >= 90 - polarRegionSize)
            {
                return new Grid(true);
            }
            else if (lat < -90 + polarRegionSize)
            {
                return new Grid(false);
            }
            else
            {
                return indicesInContent(lat, lon);
            }
        }

        public List<T> Find(double lat, double lon, double distance)
        {
            var result = new List<T>();
            var possibleGrids = new List<Grid>();
            var pending = new Queue<Grid>();

            pending.Enqueue(getGrid(lat, lon));
            visited.SetVisited(pending.First());

            while (pending.Count > 0)
            {
                var current = pending.Dequeue();

                foreach (var k in itemsInGrid(current))
                {
                    if (Distance(lat, lon, k.Lat, k.Lon) <= distance)
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

        private List<T> itemsInGrid(Grid item)
        {
            if (item.IsNorthPole)
            {
                return northPoleContent;
            }
            else if (item.IsSouthPole)
            {
                return southPoleContent;
            }
            else
            {
                return content[item.X, item.Y];
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
        private double minDis(double lat, double lon, Grid grid)
        {
            var pt = new LatLon(lat, lon);
            var center = gridCenterLatLon(grid);

            if (grid.IsNorthPole)
            {
                return EarthRadiusNm * ToRadian((90 - polarRegionSize) - center.Lat);
            }
            else if (grid.IsSouthPole)
            {
                return EarthRadiusNm * ToRadian(center.Lat - (-90 + polarRegionSize));
            }
            else
            {
                double latTop = center.Lat + gridSize / 2.0;
                double latBottom = center.Lat - gridSize / 2.0;

                double latMaxDis = (Math.Abs(latTop) >= Math.Abs(latBottom)) ? latTop : latBottom;

                return Distance(pt, center) - Distance(center.Lat, latMaxDis,
                    gridSize / 2.0);
            }
        }

        private LatLon gridCenterLatLon(Grid item)
        {
            if (item.IsNorthPole)
            {
                return new LatLon(90.0, 0.0);
            }
            else if (item.IsSouthPole)
            {
                return new LatLon(-90.0, 0.0);
            }
            else
            {
                return new LatLon(
                    -90.0 + polarRegionSize + (item.X + 0.5) * gridSize,
                    -180.0 + (item.Y + 0.5) * gridSize);
            }
        }

        private List<Grid> gridNeighbor(Grid item)
        {
            if (item.IsNorthPole)
            {
                return getPoleNeighbor(true);
            }
            else if (item.IsSouthPole)
            {
                return getPoleNeighbor(false);
            }
            else
            {
                var result = new List<Grid>();

                int x = content.GetLength(0);
                int y = content.GetLength(1);

                result.Add(new Grid(item.X, (item.Y + 1) % y));
                //east
                result.Add(new Grid(item.X, (item.Y - 1 + y) % y));
                //west

                if (item.X + 1 == x)
                {
                    result.Add(new Grid(true));
                    //north pole
                }
                else
                {
                    result.Add(new Grid(item.X + 1, item.Y));
                    //north
                }

                if (item.X == 0)
                {
                    result.Add(new Grid(false));
                    //south pole
                }
                else
                {
                    result.Add(new Grid(item.X - 1, item.Y));
                    //south
                }
                return result;
            }
        }
        
        private List<Grid> getPoleNeighbor(bool isNorthPole)
        {
            var result = new List<Grid>();
            int firstIndex = 0;

            if (isNorthPole)
            {
                firstIndex = content.GetLength(0) - 1;
            }
            else
            {
                firstIndex = 0;
            }

            for (int i = 0; i < content.GetLength(1); i++)
            {
                result.Add(new Grid(firstIndex, i));
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
                    Y = 0;
                }
                else
                {
                    X = -2;
                    Y = 0;
                }
            }

            public bool IsNorthPole
            {
                get
                {
                    return X == -1;
                }
            }

            public bool IsSouthPole
            {
                get
                {
                    return X == -2;
                }
            }
        }

        private class VisitedList
        {
            private bool[,] visited;
            private bool northPoleVisted;
            private bool southPoleVisited;
            private List<Grid> changedItems;

            public VisitedList(int item1, int item2)
            {
                visited = new bool[item1 + 1, item2 + 1];
                setVisitedFlag();
                changedItems = new List<Grid>();
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

            public void SetVisited(Grid item)
            {
                setVisitedProperty(item, true);
                changedItems.Add(item);
            }

            private void setVisitedProperty(Grid item, bool value)
            {
                if (item.IsNorthPole)
                {
                    northPoleVisted = value;
                }
                else if (item.IsSouthPole)
                {
                    southPoleVisited = value;
                }
                else
                {
                    visited[item.X, item.Y] = value;
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

            public bool IsVisited(Grid item)
            {
                if (item.IsNorthPole)
                {
                    return northPoleVisted;
                }
                else if (item.IsSouthPole)
                {
                    return southPoleVisited;
                }
                else
                {
                    return visited[item.X, item.Y];
                }
            }
        }

        #endregion

    }


}
