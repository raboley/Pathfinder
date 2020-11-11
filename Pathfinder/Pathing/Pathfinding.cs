using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Pathfinder.Map;

namespace Pathfinder.Pathing
{
    public static class Pathfinding
    {
        public static Vector3[] FindWaypoints(ZoneMap zoneMap, Vector3 startPos, Vector3 targetPos)
        {
            var sw = new Stopwatch();
            sw.Start();

            var pathSuccess = false;

            var startGridNode = zoneMap.GetNodeFromWorldPoint(startPos);
            var targetGridNode = zoneMap.GetNodeFromWorldPoint(targetPos);
            
            // startGridNode.Walkable &&
            if ( targetGridNode.Walkable)
            {
                var openSet = new Heap<Node>(zoneMap.MaxSize);
                var closedSet = new HashSet<Node>();

                openSet.Add(startGridNode);

                while (openSet.Count > 0)
                {
                    var currentGridNode = openSet.RemoveFirst();
                    closedSet.Add(currentGridNode);


                    if (currentGridNode == targetGridNode)
                    {
                        sw.Stop();
                        Console.WriteLine("Path found: " + sw.ElapsedMilliseconds + "ms");
                        pathSuccess = true;
                        break;
                    }

                    foreach (var neighbour in zoneMap.GetNeighbours(currentGridNode))
                    {
                        if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                        int newMovementCostToNeighbour =
                            currentGridNode.GCost + GetDistance(currentGridNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                        {
                            neighbour.GCost = newMovementCostToNeighbour;
                            neighbour.HCost = GetDistance(neighbour, targetGridNode);
                            neighbour.Parent = currentGridNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }

            if (!pathSuccess) return null;

            var waypoints = RetracePath(startGridNode, targetGridNode);
            return waypoints;
        }

        private static Vector3[] RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentGridNode = endNode;

            while (currentGridNode != startNode)
            {
                path.Add(currentGridNode);
                currentGridNode = currentGridNode.Parent;
            }

            var waypoints = SimplifyPath(startNode.WorldPosition, endNode.WorldPosition, path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        private static Vector3[] SimplifyPath(Vector3 start, Vector3 end, List<Node> path)
        {
            var waypoints = new List<Vector3>();
            var directionOld = Vector2.Zero;
            waypoints.Add(end);

            for (var i = 1; i < path.Count; i++)
            {
                waypoints.Add(path[i].WorldPosition);
                
                // // Simplify path
                // var directionNew =
                //     new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
                // if (directionNew != directionOld) waypoints.Add(path[i].WorldPosition);
                //
                // directionOld = directionNew;
            }

            // waypoints.Add(start);

            return waypoints.ToArray();
        }

        public static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Math.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Math.Abs(nodeA.GridY - nodeB.GridY);

            if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);

            return 14 * dstX + 10 * (dstY - dstX);
        }

        public static int GetDistancePos(Vector3 start, Vector3 end)
        {
            int dstX = GridMath.ConvertFromFloatToInt(Math.Abs(start.X - end.X));
            int dstY = GridMath.ConvertFromFloatToInt(Math.Abs(start.Y - end.Y));

            if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);

            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}