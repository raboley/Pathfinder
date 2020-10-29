using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace Pathfinder
{
    public class Pathfinding
    {
        public Grid Grid;

        // PathRequestManager requestManager;
        // void Awake() {
        //     requestManager = GetComponent<PathRequestManager>();
        //     MapGrid = GetComponent<Grid>();
        // }
        //
        // public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        //     StartCoroutine(FindPath(startPos, targetPos));
        // }
        //
        public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            yield return FindWaypoints(startPos, targetPos);
            // requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        }

        public Vector3[] FindWaypoints(Vector3 startPos, Vector3 targetPos)
        {
            var sw = new Stopwatch();
            sw.Start();

            var waypoints = new Vector3[0];
            var pathSuccess = false;

            var startGridNode = Grid.GetNodeFromWorldPoint(startPos);
            var targetGridNode = Grid.GetNodeFromWorldPoint(targetPos);

            if (startGridNode.Walkable && targetGridNode.Walkable)
            {
                var openSet = new Heap<GridNode>(Grid.MaxSize);
                var closedSet = new HashSet<GridNode>();

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

                    foreach (var neighbour in Grid.GetNeighbours(currentGridNode))
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

            waypoints = RetracePath(startGridNode, targetGridNode);
            return waypoints;
        }

        private Vector3[] RetracePath(GridNode startGridNode, GridNode endGridNode)
        {
            var path = new List<GridNode>();
            var currentGridNode = endGridNode;

            while (currentGridNode != startGridNode)
            {
                path.Add(currentGridNode);
                currentGridNode = currentGridNode.Parent;
            }

            var waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        private Vector3[] SimplifyPath(List<GridNode> path)
        {
            var waypoints = new List<Vector3>();
            var directionOld = Vector2.Zero;

            for (var i = 1; i < path.Count; i++)
            {
                var directionNew =
                    new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
                if (directionNew != directionOld) waypoints.Add(path[i].WorldPosition);

                directionOld = directionNew;
            }

            return waypoints.ToArray();
        }

        private int GetDistance(GridNode gridNodeA, GridNode gridNodeB)
        {
            int dstX = Math.Abs(gridNodeA.GridX - gridNodeB.GridX);
            int dstY = Math.Abs(gridNodeA.GridY - gridNodeB.GridY);

            if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);

            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}