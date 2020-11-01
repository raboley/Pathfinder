using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Pathfinder.People;
using Pathfinder.Persistence;
using Pathfinder.PrintConsole;

namespace Pathfinder.Map
{
    [Serializable]
    public class ZoneMap
    {
        private float _gridCenterX, _gridCenterY, _gridCenterZ;
        private int _gridSizeX, _gridSizeY;
        public Node[,] MapGrid;


        public string MapName { get; set; }

        public Vector2 GridWorldSize
        {
            get => new Vector2(_gridSizeX * 1f, _gridSizeY * 1f);
            set
            {
                if (NodeDiameter == 0)
                    throw new Exception(
                        "NodeRadius must be set prior to initializing the GridWorldSize. Otherwise Divide by " +
                        "zero error when trying to figure out the size wold size.");

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


        public int MaxSize => _gridSizeX * _gridSizeY;


        public List<Person> NpcList { get; set; }
        public List<Person> ThingList { get; set; }
        public List<Person> MobList { get; set; }
        public Dictionary<string, List<Vector3>> ZoneBoundaries { get; set; }
        public List<Node> UnknownNodes => MapGrid?.Cast<Node>().ToList().FindAll(n => n.Unknown);

        private float NodeRadius { get; set; } = 0.5f;
        private float NodeDiameter => NodeRadius * 2;

        /// <summary>
        ///     Initializes a new ZoneMap object and builds a MapGrid of size gridSize.
        ///     The MapGrid will be a 2D array incrementing from bottom left of the zoneMap (most negative)
        ///     to top right (most positive). The MapGrid consists of Nodes that will have a GridX and GridY which is
        ///     their address in the MapGrid, and a WorldPosition which is the Vector3 address of the point in the world.
        ///     For example in a zoneMap sized Vector2(3, 3)
        ///     the center point Node will have a WorldPosition of Vector3(0, 0, 0)
        ///     but, the GridX would be 1, and GridY would be 1
        ///     so, to address the center in the GridMap it would be GridMap[1,1]
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="nodeRadius"></param>
        /// <returns></returns>
        public static ZoneMap NewGridFromVector2(Vector2 gridSize, float nodeRadius = 0.5f)
        {
            var grid = new ZoneMap {NodeRadius = nodeRadius, GridWorldSize = gridSize};
            BuildGridMap(grid);

            return grid;
        }

        private static void BuildGridMap(ZoneMap zoneMap)
        {
            zoneMap.MapGrid = new Node[zoneMap._gridSizeX, zoneMap._gridSizeY];
            SetupAllLists(zoneMap);
            var worldBottomLeft = zoneMap.GetBottomLeftNodeFromGridWorldSize();
            zoneMap.BuildMapGridFromBottomLeftToTopRight(worldBottomLeft);
        }

        private static void SetupAllLists(ZoneMap zoneMap)
        {
            zoneMap.NpcList = new List<Person>();
            zoneMap.ThingList = new List<Person>();
            zoneMap.MobList = new List<Person>();
            zoneMap.ZoneBoundaries = new Dictionary<string, List<Vector3>>();
        }

        public void BuildGridMap()
        {
            MapGrid = new Node[_gridSizeX, _gridSizeY];
            NpcList = new List<Person>();
            ThingList = new List<Person>();
            MobList = new List<Person>();
            ZoneBoundaries = new Dictionary<string, List<Vector3>>();
            var worldBottomLeft = GetBottomLeftNodeFromGridWorldSize();
            BuildMapGridFromBottomLeftToTopRight(worldBottomLeft);
        }

        private Vector3 GetBottomLeftNodeFromGridWorldSize()
        {
            var biggestX = VectorRight() * GridWorldSize.X / 2;
            var biggestY = VectorForward() * GridWorldSize.Y / 2;
            var worldBottomLeft = GridCenter - biggestX - biggestY;
            return worldBottomLeft;
        }

        /// <summary>
        ///     Starting from the bottom left, increment x and y up until we reach the appropriate world sizes for both coords.
        ///     This works because by starting at the most negative, we can then just build up till we reach the most positive.
        /// </summary>
        /// <param name="worldBottomLeft"></param>
        private void BuildMapGridFromBottomLeftToTopRight(Vector3 worldBottomLeft)
        {
            for (var x = 0; x < _gridSizeX; x++)
            for (var y = 0; y < _gridSizeY; y++)
            {
                var worldPoint = worldBottomLeft
                                 + VectorRight() * (x * NodeDiameter + NodeRadius)
                                 + VectorForward() * (y * NodeDiameter + NodeRadius);
                var gridNode = new Node(worldPoint);
                gridNode.GridX = x;
                gridNode.GridY = y;
                MapGrid[x, y] = gridNode;
            }
        }

        private Vector3 VectorRight()
        {
            return new Vector3(1f, 0f, 0f);
        }

        private Vector3 VectorForward()
        {
            return new Vector3(0f, 0f, 1f);
        }


        public static ZoneMap GetGridMap(string mapName, IPersister persister)
        {
            var grid = persister.Load<ZoneMap>();
            return grid;
        }

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    neighbours.Add(MapGrid[checkX, checkY]);
            }

            return neighbours;
        }


        /// <summary>
        ///     Because our 2D array ZoneMap starts at negative and goes to positive, and you can't have a negative index,
        ///     we basically take the value from the world, and if it is negative it must be in the bottom half of the
        ///     array, and if it is positive it is the top half. So we have to move the index to all be in the positives.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
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
            var gridNode = GetNodeFromWorldPoint(position);
            gridNode.Walkable = false;
            MapGrid[gridNode.GridX, gridNode.GridY] = gridNode;
        }

        public void AddKnownNode(Vector3 worldPoint)
        {
            var gridNode = GetNodeFromWorldPoint(worldPoint);
            MapGrid[gridNode.GridX, gridNode.GridY].Unknown = false;
        }

        public void AddZoneBoundary(string ZonesTo, Vector3 worldPoint)
        {
            List<Vector3> zoneBoundaries;

            if (ZoneBoundaries != null)
                if (ZoneBoundaries.ContainsKey(ZonesTo))
                {
                    zoneBoundaries = ZoneBoundaries[ZonesTo];
                    zoneBoundaries.Add(worldPoint);
                    ZoneBoundaries[ZonesTo] = zoneBoundaries;
                    return;
                }

            zoneBoundaries = new List<Vector3>();
            zoneBoundaries.Add(worldPoint);
            ZoneBoundaries.Add(ZonesTo, zoneBoundaries);
        }

        public void AddNpc(Person person)
        {
            person.MapName = MapName;
            NpcList.Add(person);
        }

        public void AddInanimateObject(Person entity)
        {
            entity.MapName = MapName;
            ThingList.Add(entity);
        }

        /// <summary>
        ///     Returns a string representation of the current MapGrid's walkable and not walkable Nodes.
        /// </summary>
        public string Print()
        {
            INodePrinter printer = new PrintWalkable();
            string printedGrid = PrintMap(printer);
            return printedGrid;
        }

        public string PrintWithCoords()
        {
            var columnTop = "--------";
            INodePrinter printer = new PrintCoordinates();
            string printedGrid = PrintMap(printer, columnTop: columnTop);
            return printedGrid;
        }

        public string PrintPath(Vector3[] path)
        {
            const string legend = @"
Visualization of the path
s = start
e = end
w = waypoint
x = obstacle";
            var startPos = path.FirstOrDefault();
            var endPos = path.Last();

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
                    var node = GetNodeFromWorldPoint(new Vector3(x, 0, y));
                    result += nodePrinter.PrintNode(node);
                }

                result += header;
            }

            return result;
        }

        public static ZoneMap TinyMap()
        {
            return NewGridFromVector2(new Vector2(3f, 3f));
        }
    }
}