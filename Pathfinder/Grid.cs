﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Pathfinder.Pathfinder
{
    [Serializable]
    public class Grid
    {
        /// <summary>
        /// Initializes a new Grid object and builds a MapGrid of size gridSize.
        /// The MapGrid will be a 2D array incrementing from bottom left of the grid (most negative)
        /// to top right (most positive). The MapGrid consists of Nodes that will have a GridX and GridY which is
        /// their address in the MapGrid, and a WorldPosition which is the Vector3 address of the point in the world.
        /// 
        /// For example in a grid sized Vector2(3, 3)
        ///     the center point Node will have a WorldPosition of Vector3(0, 0, 0)
        ///     but, the GridX would be 1, and GridY would be 1
        ///     so, to address the center in the GridMap it would be GridMap[1,1]
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="nodeRadius"></param>
        /// <returns></returns>
        public static Grid NewGridFromVector2(Vector2 gridSize, float nodeRadius = 0.5f)
        {
            var grid = new Grid();
            grid.NodeRadius = nodeRadius;
            grid.GridWorldSize = gridSize;
            grid.MapGrid = new GridNode[grid._gridSizeX, grid._gridSizeY];
            var worldBottomLeft = grid.GetBottomLeftNodeFromGridWorldSize();
            grid.BuildMapGridFromBottomLeftToTopRight(worldBottomLeft);

            return grid;
        }

        private Vector3 GetBottomLeftNodeFromGridWorldSize()
        {
            var biggestX = vectorRight() * GridWorldSize.X / 2;
            var biggestY = vectorForward() * GridWorldSize.Y / 2;
            var worldBottomLeft = GridCenter - biggestX - biggestY;
            return worldBottomLeft;
        }

        /// <summary>
        /// Starting from the bottom left, increment x and y up until we reach the appropriate world sizes for both coords.
        /// This works because by starting at the most negative, we can then just build up till we reach the most positive.
        /// </summary>
        /// <param name="worldBottomLeft"></param>
        private void BuildMapGridFromBottomLeftToTopRight(Vector3 worldBottomLeft)
        {
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft
                                         + vectorRight() * (x * NodeDiameter + NodeRadius)
                                         + vectorForward() * (y * NodeDiameter + NodeRadius);
                    GridNode gridNode = new GridNode(worldPoint, true);
                    gridNode.GridX = x;
                    gridNode.GridY = y;
                    MapGrid[x, y] = gridNode;
                }
            }
        }

        private Vector3 vectorRight()
        {
            return new Vector3(1f, 0f, 0f);
        }

        private Vector3 vectorForward()
        {
            return new Vector3(0f, 0f, 1f);
        }

        public static Grid GetGridMap(string mapName, IPersister persister)
        {
            var grid = persister.Load<Grid>();
            return grid;
        }

        public List<GridNode> GetNeighbours(GridNode gridNode)
        {
            List<GridNode> neighbours = new List<GridNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = gridNode.GridX + x;
                    int checkY = gridNode.GridY + y;

                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    {
                        neighbours.Add(MapGrid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }


        /// <summary>
        /// Because our 2D array Grid starts at negative and goes to positive, and you can't have a negative index,
        /// we basically take the value from the world, and if it is negative it must be in the bottom half of the 
        /// array, and if it is positive it is the top half. So we have to move the index to all be in the positives.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public GridNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x = GetGridPosX(worldPosition.X);
            // Note: Since in 3D space Y is height, and don't traverse the world via the height, this should be Z.
            int y = GetGridPosY(worldPosition.Z);
            return MapGrid[x, y];
        }

        public int GetGridPosX(float vectorX)
        {
            float percentX = (vectorX + GridWorldSize.X / 2) / GridWorldSize.X;
            percentX = GridMath.Clamp(percentX, 0, 1);
            int x = GridMath.ConvertFromFloatToInt((_gridSizeX - 1) * percentX);
            return x;
        }

        public int GetGridPosY(float vectorY)
        {
            float percentY = (vectorY + GridWorldSize.Y / 2) / GridWorldSize.Y;
            percentY = GridMath.Clamp(percentY, 0, 1);
            int y = GridMath.ConvertFromFloatToInt((_gridSizeY - 1) * percentY);
            return y;
        }

        public void AddUnWalkableNode(Vector3 position)
        {
            var gridNode = NodeFromWorldPoint(position);
            gridNode.Walkable = false;
            MapGrid[gridNode.GridX, gridNode.GridY] = gridNode;
        }

        public void AddKnownNode(Vector3 worldPoint)
        {
            var gridNode = NodeFromWorldPoint(worldPoint);
            gridNode.Unknown = false;
            MapGrid[gridNode.GridX, gridNode.GridY] = gridNode;
        }

        public void AddEntities(Vector3 position, IEnumerable<IEntity> entities)
        {
            var gridNode = NodeFromWorldPoint(position);

            var listEntities = entities.ToList();
            foreach (var entity in listEntities)
            {
                entity.Position = position;
                entity.MapName = MapName;
            }

            gridNode.Entities.AddRange(listEntities);
            MapGrid[gridNode.GridX, gridNode.GridY] = gridNode;
        }

        /// <summary>
        /// Returns a string representation of the current MapGrid's walkable and not walkable Nodes.
        /// </summary>
        public string Print()
        {
            INodePrinter printer = new PrintWalkable();
            string printedGrid = PrintMap(printer);
            return printedGrid;
        }

        public string PrintWithCoords()
        {
            string columnTop = "--------";
            INodePrinter printer = new PrintCoordinates();
            string printedGrid = PrintMap(printer, columnTop: columnTop);
            return printedGrid;
        }

        public string PrintPath(Vector3 startPos, Vector3 endPos, Vector3[] path)
        {
            var legend = @"
Visualization of the path
s = start
e = end
w = waypoint
x = obstacle";

            var printer = new PrintPath {Start = startPos, End = endPos, Path = path};
            string printedGrid = PrintMap(printer, legend);
            return printedGrid;
        }

        public string PrintKnown()
        {
            INodePrinter printer = new PrintKnown();
            string printedGrid = PrintMap(printer);
            return printedGrid;
        }

        private string PrintMap(INodePrinter nodePrinter, string result = "", string columnTop = "------")
        {
            int row = MapGrid.GetLength(0);
            int column = MapGrid.GetLength(1);

            string header = Environment.NewLine + "-" + string.Concat(Enumerable.Repeat(columnTop, column)) +
                            Environment.NewLine;
            result += header;

            for (int y = row / 2; y >= -1 * row / 2; y--)
            {
                result += "|";
                for (int x = -1 * column / 2; x <= column / 2; x++)
                {
                    var node = NodeFromWorldPoint(new Vector3(x, 0, y));
                    result += nodePrinter.PrintNode(node);
                }

                result += header;
            }

            return result;
        }

        public GridNode[,] MapGrid;
        private float NodeRadius { get; set; }
        private float NodeDiameter => NodeRadius * 2;

        public Vector2 GridWorldSize
        {
            get => new Vector2(_gridSizeX * 1f, _gridSizeY * 1f);
            set
            {
                if (NodeDiameter == 0)
                {
                    throw new Exception(
                        "NodeRadius must be set prior to initializing the GridWorldSize. Otherwise Divide by " +
                        "zero error when trying to figure out the size wold size.");
                }

                _gridSizeX = GridMath.ConvertFromFloatToInt(value.X / NodeDiameter);
                _gridSizeY = GridMath.ConvertFromFloatToInt(value.Y / NodeDiameter);
            }
        }

        public Vector3 GridCenter
        {
            get => new Vector3(_gridCenterX, _gridCenterY, _gridCenterZ);
            set
            {
                _gridCenterX = value.X;
                _gridCenterY = value.Y;
                _gridCenterZ = value.Z;
            }
        }

        private float _gridCenterX, _gridCenterY, _gridCenterZ;


        public int MaxSize => _gridSizeX * _gridSizeY;
        private int _gridSizeX, _gridSizeY;

        public string MapName { get; set; }

        public Vector2 DefaultGridSize
        {
            get => new Vector2(_defaultGridSizeX, _defaultGridSizeY);
            set
            {
                _defaultGridSizeX = value.X;
                _defaultGridSizeY = value.Y;
            }
        }

        private float _defaultGridSizeX, _defaultGridSizeY;

        public IPersister Persister { get; set; }
    }
}