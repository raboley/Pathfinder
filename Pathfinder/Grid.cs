using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Pathfinder.Pathfinder
{
    public class Grid
    {
        public Grid(Vector2 _gridSize, float _nodeRadius = 0.5f)
        {
            GridWorldSize = _gridSize;
            NodeRadius = _nodeRadius;
            nodeDiameter = _nodeRadius * 2;
            gridSizeX = GridMath.ConvertFromFloatToInt(GridWorldSize.X / nodeDiameter);
            gridSizeY = GridMath.ConvertFromFloatToInt(GridWorldSize.Y / nodeDiameter);
            GridCenter = Vector3.Zero;
        }

        //     public bool displayGridGizmos;
        public float NodeRadius;
        float nodeDiameter;

        public Vector2 GridWorldSize;
        public Vector3 GridCenter;

        public Node[,] grid;
        //     public bool unknownGrid;
        //     public Node[,] unwalkableNodes;
        //

        int gridSizeX, gridSizeY;
        //
        //     void Awake()
        //     {
        //         nodeDiameter = nodeRadius * 2;

        //         unwalkableNodes = new Node[gridSizeX, gridSizeY];
        //         CreateGrid(unknownGrid);
        //     }
        //
        public int MaxSize => gridSizeX * gridSizeY;

        //
        public void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = GetBottomLeftNodeFromGridWorldSize();
            BuildAndSetGridFromBottomLeftToTopRight(worldBottomLeft);
        }

        private Vector3 GetBottomLeftNodeFromGridWorldSize()
        {
            Vector3 biggestX = vectorRight() * GridWorldSize.X / 2;
            Vector3 biggestY = vectorForward() * GridWorldSize.Y / 2;
            Vector3 worldBottomLeft = GridCenter - biggestX - biggestY;
            return worldBottomLeft;
        }

        private void BuildAndSetGridFromBottomLeftToTopRight(Vector3 worldBottomLeft)
        {
            // Starting from the bottom left, increment x and y up until we reach the appropriate world sizes for both coords.
            // This works because by starting at the most negative, we can then just build up till we reach the most positive.
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft
                                         + vectorRight() * (x * nodeDiameter + NodeRadius)
                                         + vectorForward() * (y * nodeDiameter + NodeRadius);
                    Node node = new Node(worldPoint, true);
                    node.gridX = x;
                    node.gridY = y;
                    grid[x, y] = node;
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


        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

       
        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            // Because our 2D array Grid starts at negative and goes to positive, and you can't have a negative index,
            // we basically take the value from the world, and if it is negative it must be in the bottom half of the 
            // array, and if it is positive it is the top half. So we have to move the index to all be in the positives.
            int x = GetGridPosX(worldPosition.X);
            int y = GetGridPosY(worldPosition.Z);
            return grid[x, y];
        }

        public int GetGridPosX(float VectorX)
        {
            float percentX = (VectorX + GridWorldSize.X / 2) / GridWorldSize.X;
            percentX = GridMath.Clamp(percentX, 0, 1);
            int x = GridMath.ConvertFromFloatToInt((gridSizeX - 1) * percentX);
            return x;
        }
        public int GetGridPosY(float VectorY)
        {
            float percentY = (VectorY + GridWorldSize.Y / 2) / GridWorldSize.Y;
            percentY = GridMath.Clamp(percentY, 0, 1);
            int y = GridMath.ConvertFromFloatToInt((gridSizeY - 1) * percentY);
            return y;
        }
        
        public void AddUnWalkableNode(Vector3 position)
        {
            Node node = NodeFromWorldPoint(position);
            node.walkable = false;
            grid[node.gridX, node.gridY] = node;
        }

        /// <summary>
        /// Returns a string representation of the current grid
        /// </summary>
        public string Print(bool withCoords = false)
        {
            string result = "";
            
            int row = grid.GetLength(0);
            int column = grid.GetLength(1);

            string columnTop;
            if (withCoords)
            {
                columnTop = "--------";
            }
            else
            {
                columnTop = "------";
            }

            string header = Environment.NewLine + "-" + string.Concat(Enumerable.Repeat(columnTop, column)) + Environment.NewLine;
            result += header;

            for (int y = row/2; y >= -1*row/2; y--)
            {
                result += "|";
                for (int x = -1*column/2; x <= column/2; x++)
                {
                    var node = NodeFromWorldPoint(new Vector3(x, 0, y));
                    if (withCoords)
                    {
                        result += x.ToString().PadLeft(3, ' ') + "," + y.ToString().PadRight(3, ' ') + "|"; 
                    }
                    else
                    {
                        if (node.walkable == false)
                        {
                            result += "  x  |";
                        }
                        else
                        {
                            result += "     |";
                        }
                    }

                }
                result += header;
            }

            // result = buildBottomLeftToTopRight(withCoords, row, result, column, header);

            // result += BuildBottomRight(withCoords, row, result, column, header);
            // result += someQuad(withCoords, row, result, column, header); 
            
            return result;
        }

        private string buildBottomLeftToTopRight(bool withCoords, int row, string result, int column, string header)
        {
            for (int i = row - 1; i >= 0; i--)
            {
                result += "|";
                for (int j = 0; j < column; j++)
                    // for (int j = column - 1; j >= 0; j--)
                {
                    if (withCoords)
                    {
                        result += " " + i + "," + j + " |";
                    }
                    else
                    {
                        if (grid[i, j].walkable == false)
                        {
                            result += "  x  |";
                        }
                        else
                        {
                            result += "     |";
                        }
                    }
                }

                result += header;
            }

            return result;
        }

        //
        //     public List<Node> path;
        //     void OnDrawGizmos()
        //     {
        //         Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        //         if (grid != null && displayGridGizmos)
        //         {
        //             foreach (Node n in grid)
        //             {
        //                 Gizmos.color = (n.walkable) ? Color.white : Color.red;
        //                 Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
        //             }
        //         }
        //     }
    }
}