using QSP.AviationTools.Coordinates;
using QSP.RouteFinding.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Angles;
using static QSP.MathTools.GCDis;
using static QSP.MathTools.Modulo;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.RouteFinding.Data
{
    public class LatLonSearcher<T> where T : ICoordinate, IEquatable<T>
    {
        private int gridSize;
        private int polarRegionSize;

        // First index: corresponds to lat; second index: corresponds to lon
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
            Init(gridSize, polarRegionSize, equalComp);
        }

        public LatLonSearcher(GridSizeOption para)
            : this(para, EqualityComparer<T>.Default)
        { }

        public LatLonSearcher(GridSizeOption para, IEqualityComparer<T> equalComp)
        {
            switch (para)
            {
                case GridSizeOption.Small:
                    Init(2, 5, equalComp);
                    break;

                case GridSizeOption.Large:
                    Init(10, 15, equalComp);
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        private void Init(int gridSize, int polarRegionSize,
            IEqualityComparer<T> equalComp)
        {
            Ensure<ArgumentException>(
                0 < gridSize &&
                0 < polarRegionSize && polarRegionSize < 90);

            this.gridSize = gridSize;
            this.polarRegionSize = polarRegionSize;
            this.equalComp = equalComp;

            int latCount = (int)(
                Math.Ceiling(180.0 - 2.0 * polarRegionSize) / gridSize);
            int lonCount = (int)(Math.Ceiling(360.0 / gridSize));

            content = new List<T>[latCount, lonCount];
            InitContent();

            visited = new VisitedList(content.GetLength(0), content.GetLength(1));
        }
        
        public enum GridSizeOption
        {
            Small,
            Large
        }

        private void InitContent()
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
            if (item.Lat >= 90.0 - polarRegionSize)
            {
                northPoleContent.Add(item);
            }
            else if (item.Lat < -90.0 + polarRegionSize)
            {
                southPoleContent.Add(item);
            }
            else
            {
                AddContent(IndicesInContent(item.Lat, item.Lon), item);
            }
        }

        private void AddContent(Grid indices, T item)
        {
            content[indices.X, indices.Y].Add(item);
        }

        private Grid IndicesInContent(double lat, double lon)
        {
            if (lon == 180.0)
            {
                lon = -180.0;
            }

            return new Grid(
                (int)((lat + 90.0 - polarRegionSize) / gridSize),
                (int)((lon + 180.0) / gridSize));
        }

        private Grid GetGrid(double lat, double lon)
        {
            if (lat >= 90.0 - polarRegionSize)
            {
                return Grid.NorthPole;
            }
            else if (lat < -90.0 + polarRegionSize)
            {
                return Grid.SouthPole;
            }
            else
            {
                return IndicesInContent(lat, lon);
            }
        }

        public List<T> Find(double lat, double lon, double distance)
        {
            var result = new List<T>();
            var possibleGrids = new List<Grid>();
            var pending = new Queue<Grid>();

            pending.Enqueue(GetGrid(lat, lon));
            visited.SetVisited(pending.First());

            while (pending.Count > 0)
            {
                var current = pending.Dequeue();

                foreach (var k in ItemsInGrid(current))
                {
                    if (Distance(lat, lon, k.Lat, k.Lon) <= distance)
                    {
                        result.Add(k);
                    }
                }

                // Find grids next to current grid.
                // Add items within the specified distance into the result.
                // Also checks if the node is visited.
                foreach (var i in GridNeighbor(current))
                {
                    if (visited.IsVisited(i) == false &&
                        DistanceLowerBound(lat, lon, i) <= distance)
                    {
                        pending.Enqueue(i);
                        visited.SetVisited(i);
                    }
                }
            }

            visited.Undo();
            return result;
        }

        private List<T> ItemsInGrid(Grid item)
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
            var gridItems = ItemsInGrid(GetGrid(item.Lat, item.Lon));
            return gridItems.Remove(item);
        }

        /// <summary>
        /// Finds a lower bound of distances from the (lat, lon) to 
        /// any points in the gird (in NM).
        /// </summary>
        private double DistanceLowerBound(double lat, double lon, Grid grid)
        {
            var pt = new LatLon(lat, lon);
            var center = GridCenterLatLon(grid);

            if (grid.IsNorthPole)
            {
                return EarthRadiusNm *
                    ToRadian((90.0 - polarRegionSize) - center.Lat);
            }
            else if (grid.IsSouthPole)
            {
                return EarthRadiusNm *
                    ToRadian(center.Lat - (-90.0 + polarRegionSize));
            }
            else
            {
                double latTop = center.Lat + gridSize / 2.0;
                double latBottom = center.Lat - gridSize / 2.0;
                double latMin = (Math.Abs(latTop) >= Math.Abs(latBottom)) ?
                    latBottom : latTop;

                return Distance(pt, center) -
                    Distance(center.Lat, latMin, gridSize / 2.0);
            }
        }

        private LatLon GridCenterLatLon(Grid item)
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

        private List<Grid> GridNeighbor(Grid item)
        {
            if (item.IsNorthPole)
            {
                return GetPoleNeighbor(true);
            }
            else if (item.IsSouthPole)
            {
                return GetPoleNeighbor(false);
            }
            else
            {
                var result = new List<Grid>();

                int x = content.GetLength(0);
                int y = content.GetLength(1);

                // East
                result.Add(new Grid(item.X, (item.Y + 1) % y));

                // West
                result.Add(new Grid(item.X, (item.Y - 1).Mod(y)));

                if (item.X + 1 == x)
                {
                    // North pole
                    result.Add(Grid.NorthPole);
                }
                else
                {
                    // North
                    result.Add(new Grid(item.X + 1, item.Y));
                }

                if (item.X == 0)
                {
                    // South pole
                    result.Add(Grid.SouthPole);
                }
                else
                {
                    // South
                    result.Add(new Grid(item.X - 1, item.Y));
                }

                return result;
            }
        }

        private List<Grid> GetPoleNeighbor(bool isNorthPole)
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

            public static Grid NorthPole
            {
                get
                {
                    return new Grid(-1, 0);
                }
            }

            public static Grid SouthPole
            {
                get
                {
                    return new Grid(-2, 0);
                }
            }

            public bool IsNorthPole
            {
                get
                {
                    return X == -1 && Y == 0;
                }
            }

            public bool IsSouthPole
            {
                get
                {
                    return X == -2 && Y == 0;
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
                SetVisitedFlag();
                changedItems = new List<Grid>();
            }

            private void SetVisitedFlag()
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
                SetVisitedProperty(item, true);
                changedItems.Add(item);
            }

            private void SetVisitedProperty(Grid item, bool value)
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
                    SetVisitedProperty(i, false);
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
