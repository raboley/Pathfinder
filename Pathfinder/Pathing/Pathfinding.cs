using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Pathfinder.Map;

namespace Pathfinder.Pathing
{
    public static class PathSmoother
    {
        public static Vector3[] PathToArray(Vector3 start, Vector3 end, List<Node> path)
        {
            var waypoints = new List<Vector3>();
            var directionOld = Vector2.Zero;
            waypoints.Add(end);

            for (var i = 1; i < path.Count; i++)
            {
                // waypoints.Add(path[i].WorldPosition);

                // Simplify path
                var directionNew =
                    new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
                if (directionNew != directionOld) waypoints.Add(path[i].WorldPosition);

                directionOld = directionNew;
            }

            // waypoints.Add(start);

            return waypoints.ToArray();
        }
    }

    public static class PathNotSmoother
    {
        public static Vector3[] PathToArray(Vector3 start, Vector3 end, List<Node> path)
        {
            var waypoints = new List<Vector3>();
            waypoints.Add(end);

            for (var i = 1; i < path.Count; i++)
            {
                waypoints.Add(path[i].WorldPosition);
            }

            // waypoints.Add(start);

            return waypoints.ToArray();
        }
    }

    public class ExactMatchStyle : IPathfindingStyle
    {
        public bool PathIsFound(Node currentGridNode, Node targetGridNode)
        {
            if (currentGridNode == targetGridNode)
                return true;

            return false;
        }
    }

    public class CloseEnoughStyle : IPathfindingStyle
    {
        public int DistanceTolerance = 1;

        public bool PathIsFound(Node currentGridNode, Node targetGridNode)
        {
            if (currentGridNode == targetGridNode)
                return true;

            if (GridMath.GetDistance(currentGridNode, targetGridNode) <= DistanceTolerance)
                return true;

            return false;
        }
    }


    public static class Pathfinding
    {
        /// <summary>
        /// Distance Tolerance is in 10x range, so if you want 1 it should be 10.
        /// </summary>
        /// <param name="zoneMap"></param>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="pathfindingStyle"></param>
        /// <returns></returns>
        public static Vector3[] FindWaypoints(ZoneMap zoneMap, Vector3 startPos, Vector3 targetPos,
            IPathfindingStyle pathfindingStyle = null)
        {
            if (pathfindingStyle == null)
                pathfindingStyle = new ExactMatchStyle();

            var sw = new Stopwatch();
            sw.Start();

            var pathSuccess = false;

            var startGridNode = zoneMap.GetNodeFromWorldPoint(startPos);
            var targetGridNode = zoneMap.GetNodeFromWorldPoint(targetPos);

            // // startGridNode.Walkable &&
            // if (targetGridNode.Walkable)
            // {
            var openSet = new Heap<Node>(zoneMap.MaxSize);
            var closedSet = new HashSet<Node>();

            openSet.Add(startGridNode);

            while (openSet.Count > 0)
            {
                var currentGridNode = openSet.RemoveFirst();
                closedSet.Add(currentGridNode);


                // var distance = GridMath.GetDistance(currentGridNode, targetGridNode);
                // if (distance <= distanceTolerance)
                if (pathfindingStyle.PathIsFound(currentGridNode, targetGridNode))
                {
                    sw.Stop();
                    Console.WriteLine("Path found: " + sw.ElapsedMilliseconds + "ms");

                    pathSuccess = true;
                    var waypoints = RetracePath(startGridNode, currentGridNode);
                    return waypoints;
                }

                foreach (var neighbour in zoneMap.GetNeighbours(currentGridNode))
                {
                    if ((!neighbour.Walkable && neighbour != targetGridNode) || closedSet.Contains(neighbour)) continue;

                    int newMovementCostToNeighbour =
                        currentGridNode.GCost + GridMath.GetDistance(currentGridNode, neighbour) +
                        neighbour.MovementPenalty;

                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GridMath.GetDistance(neighbour, targetGridNode);
                        neighbour.Parent = currentGridNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
            // }

            if (!pathSuccess) return null;

            // var waypoints = RetracePath(startGridNode, targetGridNode);
            return null;
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

            // var waypoints = PathSmoother.PathToArray(startNode.WorldPosition, endNode.WorldPosition, path);
            var waypoints = PathNotSmoother.PathToArray(startNode.WorldPosition, endNode.WorldPosition, path);
            Array.Reverse(waypoints);
            return waypoints;
        }
    }
}